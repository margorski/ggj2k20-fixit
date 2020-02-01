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
    ACTION1,
    ACTION2
}
public class GameState
{
    public Dictionary<MovementAction, KeyCode> keycodes = new Dictionary<MovementAction, KeyCode>() { };
    public Vector2 playerSpeed = Vector2.zero;
    public float GravityModifier = 0.0f;
}

public class GameManager : MonoBehaviour
{
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
        if (boxModel == null || boxModel.Boxes == null) return;

        var startBoxes = boxModel.Boxes.FindAll(box => box.SourceBoxes.Count == 0);
        foreach (var box in startBoxes)
        {
            RecurrentProcessBox(box, 0.0f, boxModel.Boxes);

        }
    }

    private void RecurrentProcessBox(BoxModelSerializable box, float input, List<BoxModelSerializable> allboxes)
    {
        switch (box.BoxType)
        {
            case BoxType.ACTION_GO_LEFT:
                gamestate.playerSpeed.x = -input;
                break;
            case BoxType.ACTION_GO_RIGHT:
                gamestate.playerSpeed.x = input;
                break;
            case BoxType.ACTION_JUMP:
                gamestate.playerSpeed.y = input;
                break;
            case BoxType.ENVIRONMENT_GRAVITY:
                gamestate.GravityModifier = input;
                break;
        }

        var output = ActionBlocks.BoxActions[box.BoxType](input);

        foreach(var targetBoxId in box.TargetBoxes)
        {
            var targetBox = allboxes.Find(b => b.Id == targetBoxId);
            if (targetBox == null) continue;

            RecurrentProcessBox(targetBox, output, allboxes);
        }
    }
}


