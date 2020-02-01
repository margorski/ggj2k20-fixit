using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinished : MonoBehaviour
{
    void OnCollisionEnter(Collision colliderName)
    {
        if (colliderName.collider.name == "Player")
        {
            Debug.Log("Level Succesful!");
        }
    }
}
