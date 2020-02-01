using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    bool isMoveRight = true;
    public bool canJump = false;
    // Update is called once per frame

    void OnCollisionEnter(Collision colliderName)
    {
        if (colliderName.collider.tag == "Ground")
        {
            canJump = true;
        }
    }

    void OnCollisionExit(Collision colliderName)
    {
        
        if (colliderName.collider.tag == "Ground")
        {
            canJump = false;
        }
    }
    void FixedUpdate()
    {
        //rb.AddForce(10000 * Time.deltaTime, 0, 0);
        if (Input.GetKey("d") /*&& buttonactive*/)
        {
            isMoveRight = true;
            rb.AddForce(1000*Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("space"))
        {
            if (canJump)
            {
                Debug.Log("dupa");
                if (isMoveRight)
                {
                    rb.AddForce(0, 150f, 0);
                }
                else
                {
                    rb.AddForce(0, 150f, 0);
                }
            }
        }
        if (Input.GetKey("a"))
        {
            isMoveRight = false;
            rb.AddForce(-1000 * Time.deltaTime, 0, 0);
        }
        
    }
}
