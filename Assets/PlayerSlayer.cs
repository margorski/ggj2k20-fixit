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
    public CapsuleCollider2D gdzieBoliChuj;
    public float Gravity = 5f;
    public float JumpingTime = 0.5f;


    private bool canMove = true;
    private Vector2 backUp;
    private bool Grounded = false;
    private bool IsJumping = false;
    private float JumpTimestamp = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            mojCHuj.velocity = backUp;
            return;
        }
        Vector3 gdzie = new Vector3();
        if(Input.GetKey(KeyCode.RightArrow))
        {
            gdzie += Vector3.right * CHUJ;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
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
            Input.GetKey(KeyCode.UpArrow))
        {
            JumpTimestamp = Time.time;
            IsJumping = true;
        }

        if(IsJumping &&
           Input.GetKey(KeyCode.UpArrow) &&
           (Time.time - JumpTimestamp) > JumpingTime)
        {
            IsJumping = false;
        }

        if (IsJumping &&
            !Input.GetKey(KeyCode.UpArrow))
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
