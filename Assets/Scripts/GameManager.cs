using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.FixItEditor.Models;
using Assets.Scripts.FixItEditor.Enums;
using Assets.Scripts.Platformer;

public enum MovementAction
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    JUMP,
    ACTION2
}
public class GameState
{
    public Dictionary<MovementAction, KeyCode?> keycodes = new Dictionary<MovementAction, KeyCode?>() {
        { MovementAction.JUMP, null },
        { MovementAction.ACTION2, null },
        { MovementAction.DOWN, null },
        { MovementAction.LEFT, null },
        { MovementAction.RIGHT, null },
        { MovementAction.UP, null }
    };

    public List<MovementAction> isEnabledForWholeEternity = new List<MovementAction>();
}

public class GameManager : MonoBehaviour
{
    public static Dictionary<BoxType, MovementAction> movementActionsMapping = new Dictionary<BoxType, MovementAction>()
    {
        { BoxType.ACTION_DUCK, MovementAction.DOWN },
        { BoxType.ACTION_GO_LEFT, MovementAction.LEFT },
        { BoxType.ACTION_GO_RIGHT, MovementAction.RIGHT },
        { BoxType.ACTION_JUMP, MovementAction.JUMP}
    };

    public static Dictionary<BoxType, KeyCode> keycodeMapping = new Dictionary<BoxType, KeyCode>()
    {
        { BoxType.INPUT_ACTION_1, KeyCode.Space },
        { BoxType.INPUT_ACTION_2, KeyCode.LeftAlt },
        { BoxType.INPUT_DOWN, KeyCode.DownArrow },
        { BoxType.INPUT_LEFT, KeyCode.LeftArrow },
        { BoxType.INPUT_RIGHT, KeyCode.RightArrow },
        { BoxType.INPUT_UP, KeyCode.UpArrow }
    };

    public GameState gamestate { private set; get; }
    private NetworkManager networkManager;

    private void Awake()
    {
        gamestate = new GameState();
        networkManager = (NetworkManager)GameObject.FindObjectOfType<NetworkManager>();
        Object.DontDestroyOnLoad(this.gameObject);


        InvokeRepeating("OnPeriodicalUpdate", 0, 0.1f);
    }

    private void OnPeriodicalUpdate()
    {
        var networkStatus = networkManager.GetStatus();
        if (networkStatus.connected && networkStatus.networkType == NetworkType.CLIENT)
        {
            var message = networkManager.GetMessage();
            UpdateGamestate(message);
        }

    }

    public void UpdateGamestate(NetworkBoxModel boxModel) 
    {
        gamestate.isEnabledForWholeEternity.Clear();
        if (boxModel == null || boxModel.Boxes == null) return;

        var startBoxes = boxModel.Boxes.FindAll(box => box.SourceBoxes.Count == 0 && box.TargetBoxes.Count == 1);
        foreach (var box in startBoxes)
        {
            ProcessConnection(box, 0.0f, boxModel.Boxes);
        }
    }

    private void ProcessConnection(BoxModelSerializable box, float input, List<BoxModelSerializable> allboxes)
    {
        var targetBox = allboxes.Find(b => b.Id == box.TargetBoxes[0]);

        var actionTypes = new List<BoxType>() { BoxType.ACTION_DUCK, BoxType.ACTION_GO_LEFT, BoxType.ACTION_GO_RIGHT, BoxType.ACTION_JUMP };
        var inputTypes = new List<BoxType>() { BoxType.INPUT_ACTION_1, BoxType.INPUT_ACTION_2, BoxType.INPUT_DOWN, BoxType.INPUT_LEFT, BoxType.INPUT_RIGHT, BoxType.INPUT_UP, BoxType.INPUT_ONE };

        if (!inputTypes.Exists(t => t == box.BoxType)) return;
        if (!actionTypes.Exists(t => t == targetBox.BoxType)) return;

        if (box.BoxType == BoxType.INPUT_ONE) gamestate.isEnabledForWholeEternity.Add(movementActionsMapping[box.BoxType]);
        else gamestate.keycodes[movementActionsMapping[targetBox.BoxType]] = keycodeMapping[box.BoxType];
    }
}


