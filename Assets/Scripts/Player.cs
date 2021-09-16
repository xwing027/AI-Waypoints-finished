using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//not used for the assessment

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5;

    void Update()
    {
        Vector3 movementDelta = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            movementDelta.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementDelta.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementDelta.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementDelta.x += 1;
        }

        transform.position += movementDelta.normalized * speed * Time.deltaTime;
    }
}
