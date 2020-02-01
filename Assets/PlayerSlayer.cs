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
    public Rigidbody2D mojCHuj;
    public CapsuleCollider2D gdzieBoliChuj;

    // Update is called once per frame
    void Update()
    {
        Vector3 gdzie = new Vector3();
        if(Input.GetKey(KeyCode.RightArrow))
        {
            gdzie += Vector3.right * CHUJ;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gdzie += Vector3.left * CHUJ;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            gdzie += Vector3.up * CHUJ;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            gdzie += Vector3.down* CHUJ;
        }
        mojCHuj.velocity = gdzie;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //var gdzietenchuj = collision.attachedRigidbody.position - gdzieBoliChuj.attachedRigidbody.position;
        //mojCHuj.MovePosition(mojCHuj.position + gdzietenchuj);
    }
}
