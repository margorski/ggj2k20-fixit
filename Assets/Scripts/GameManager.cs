using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Dictionary<MovementAction, KeyCode> keycodes = new Dictionary<MovementAction, KeyCode>() {
        { MovementAction.ACTION1, KeyCode.Space },
        { MovementAction.ACTION2, KeyCode.LeftAlt },
        { MovementAction.LEFT, KeyCode.LeftArrow },
        { MovementAction.RIGHT, KeyCode.RightArrow },
        { MovementAction.UP, KeyCode.UpArrow },
        { MovementAction.DOWN, KeyCode.DownArrow }
    };
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
        }
    }


}
