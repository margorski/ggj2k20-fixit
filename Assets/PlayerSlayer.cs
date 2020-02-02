using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float CHUJ = 1f;
    public float Skok = 10f;
    public Rigidbody2D mojCHuj;
    public float Gravity = 5f;
    public float JumpingTime = 0.5f;


    private bool canMove = true;
    private Vector2 backUp;
    private bool Grounded = false;
    private bool IsJumping = false;
    private float JumpTimestamp = 0f;

    private GameManager gameManager;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            mojCHuj.velocity = backUp;
            return;
        }
        Vector3 gdzie = new Vector3();
        if(isPressed(MovementAction.RIGHT))
        {
            gdzie += Vector3.right * CHUJ;
        }
        if (isPressed(MovementAction.LEFT))
        {
            gdzie += Vector3.left * CHUJ;
        }

        if (!IsJumping)
        {
            gdzie += Vector3.down * Gravity;
        }

        RaycastHit2D[] hits = new RaycastHit2D[1];
        var hitsCount = mojCHuj.Cast(Vector2.down, hits, 0.2f);
        if(hitsCount != 0)
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

        if(Grounded)
        {
            gdzie.y = 0;
        }

        if (Grounded &&
            !IsJumping &&
            isPressed(MovementAction.JUMP))
        {
            JumpTimestamp = Time.time;
            IsJumping = true;
        }

        if(IsJumping &&
           isPressed(MovementAction.JUMP) &&
           (Time.time - JumpTimestamp) > JumpingTime)
        {
            IsJumping = false;
        }

        if (IsJumping &&
            !isPressed(MovementAction.JUMP))
        {
            IsJumping = false;
        }


        hits = new RaycastHit2D[1];
        hitsCount = mojCHuj.Cast(gdzie, hits, gdzie.magnitude * Time.deltaTime);
        if (hitsCount == 0)
        {
            if (IsJumping)
            {
                gdzie += Vector3.up * Skok * (1f - (Time.time - JumpTimestamp)/JumpingTime);
            }
            if (!Grounded)
            {
                mojCHuj.velocity = Vector2.Lerp(mojCHuj.velocity, gdzie, JumpingTime);
            }
            else
            {
                mojCHuj.velocity = gdzie;
            }
        }
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private bool isPressed(MovementAction movementAction)
    {
        if (gameManager == null) return false;

        var keycodes = gameManager.gamestate.keycodes;
        var isEnabledForWholeEternity = gameManager.gamestate.isEnabledForWholeEternity;

        return ((keycodes.ContainsKey(movementAction) && keycodes[movementAction].HasValue && Input.GetKey(keycodes[movementAction].Value)) ||
                isEnabledForWholeEternity.Exists(ma => ma == movementAction));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        canMove = false;
        backUp = -mojCHuj.velocity;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        canMove = true;
    }
}
