using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    private bool inAir = true;
    private Vector2 speed = Vector2.zero;

    public const float MOVE_SPEED = 2.0f;
    public const float JUMP_SPEED = 5.0f;
    public const float GRAVITY = -25.0f;
    private GameManager gameManager;
    private Rigidbody rigidBody;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigidBody = GetComponent<Rigidbody>();
    }


    private void HandleInput()
    {
        if (gameManager == null) return;

        var keycodes = gameManager.gamestate.keycodes;
        var isEnabledForWholeEternity = gameManager.gamestate.isEnabledForWholeEternity;

        if ((keycodes.ContainsKey(MovementAction.LEFT) && keycodes[MovementAction.LEFT].HasValue && Input.GetKey(keycodes[MovementAction.LEFT].Value)) ||
            isEnabledForWholeEternity.Exists(ma => ma == MovementAction.LEFT))
        {
            speed.x = -MOVE_SPEED;
        }
        else if ((keycodes.ContainsKey(MovementAction.RIGHT) && keycodes[MovementAction.RIGHT].HasValue && Input.GetKey(keycodes[MovementAction.RIGHT].Value)) ||
                isEnabledForWholeEternity.Exists(ma => ma == MovementAction.RIGHT))

        {
            speed.x = MOVE_SPEED;
        }
        else
        {
            speed.x = 0.0f;
        }

        if ((keycodes.ContainsKey(MovementAction.UP) && keycodes[MovementAction.UP].HasValue && Input.GetKey(keycodes[MovementAction.UP].Value)) ||
            isEnabledForWholeEternity.Exists(ma => ma == MovementAction.UP))

        {
            // Do nothing
        }
        if ((keycodes.ContainsKey(MovementAction.DOWN) && keycodes[MovementAction.DOWN].HasValue && Input.GetKey(keycodes[MovementAction.DOWN].Value)) ||
            isEnabledForWholeEternity.Exists(ma => ma == MovementAction.DOWN))
                
        {
            // Duck
        }
        if ((keycodes.ContainsKey(MovementAction.JUMP) && keycodes[MovementAction.JUMP].HasValue && Input.GetKeyDown(keycodes[MovementAction.JUMP].Value)) ||
            isEnabledForWholeEternity.Exists(ma => ma == MovementAction.JUMP))

        {
            if (!inAir)
            {
                inAir = true;
                speed.y = JUMP_SPEED;
            }
        }
        if ((keycodes.ContainsKey(MovementAction.ACTION2) && keycodes[MovementAction.ACTION2].HasValue && Input.GetKeyDown(keycodes[MovementAction.ACTION2].Value)) ||
            isEnabledForWholeEternity.Exists(ma => ma == MovementAction.ACTION2))

        {
            // Do nothing
        }
    }

    private void FixedUpdate()
    {
        HandleInput();

        var gamestate = gameManager.gamestate;
        //if (inAir) speed.y += GRAVITY * Time.deltaTime;

        rigidBody.velocity = new Vector3(
            speed.x,
            speed.y,
            0.0f
        );
    }
}
