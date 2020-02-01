using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    private bool inAir = true;
    private Vector2 speed = Vector2.zero;

    public const float MOVE_SPEED = 2.0f;
    public const float JUMP_SPEED = 5.0f;
    public const float GRAVITY = -15.0f;
    private GameManager gameManager;
    private Rigidbody rigidBody;
    private void Awake()
    {
        gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var playerCollider = GetComponent<Collider>();
        if (collision.collider.bounds.max.y >= playerCollider.bounds.min.y)
        {
            inAir = false;
            speed.y = 0.0f;
            float deltaMove = playerCollider.bounds.min.y - collision.collider.bounds.max.y;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        inAir = true;    
    }

    private void HandleInput()
    {
        if (gameManager == null) return;

        var keycodes = gameManager.gamestate.keycodes;

        if (keycodes.ContainsKey(MovementAction.LEFT) && Input.GetKey(keycodes[MovementAction.LEFT]))
        {
            speed.x = -MOVE_SPEED;
        }
        else if (keycodes.ContainsKey(MovementAction.RIGHT) && Input.GetKey(keycodes[MovementAction.RIGHT]))
        {
            speed.x = MOVE_SPEED;
        }
        else
        {
            speed.x = 0.0f;
        }

        if (keycodes.ContainsKey(MovementAction.UP) && Input.GetKey(keycodes[MovementAction.UP]))
        {
            // Do nothing
        }
        if (keycodes.ContainsKey(MovementAction.DOWN) && Input.GetKey(keycodes[MovementAction.DOWN]))
        {
            // Duck
        }
        if (keycodes.ContainsKey(MovementAction.ACTION1) && Input.GetKeyDown(keycodes[MovementAction.ACTION1]))
        {
            if (!inAir)
            {
                inAir = true;
                speed.y = JUMP_SPEED;
            }
        }
        if (keycodes.ContainsKey(MovementAction.ACTION2) && Input.GetKeyDown(keycodes[MovementAction.ACTION2]))
        {
            // Do nothing
        }
    }

    private void FixedUpdate()
    {
        HandleInput();

        if (inAir) speed.y += GRAVITY * Time.deltaTime;
        rigidBody.velocity = new Vector3(
            speed.x,
            speed.y,
            0.0f
        );
    }
}
