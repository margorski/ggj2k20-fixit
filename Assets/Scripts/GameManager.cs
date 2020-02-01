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
    
    private void Awake()
    {
        gamestate = new GameState();
        Object.DontDestroyOnLoad(this.gameObject);
    }
}
