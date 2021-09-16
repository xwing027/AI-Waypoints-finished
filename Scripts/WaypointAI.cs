using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAI : MonoBehaviour
{
    [SerializeField] private float speedX = 1f;
    [SerializeField] private float speedY = 1f;
    private GameObject currentGoal;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject goal2;

    void Start()
    {
        currentGoal = goal;
    }

    void Update()
    {
        //this gets the distance to the goal
        float distance = Vector2.Distance(transform.position, currentGoal.transform.position);

        //this finds the direction to the goal
        Vector2 direction = (currentGoal.transform.position - transform.position).normalized;
        Vector2 position = transform.position;

        // this movies the ai towards the direction set (aka the goal)
        position.x += (direction.x * speedX * Time.deltaTime);
        position.y += (direction.y * speedY * Time.deltaTime);

        if (distance > 0.01f) //this makes the square stop when it touches the circles, instead of bouncing around
        {
            transform.position = position;
        }
        else
        {
            currentGoal = goal2;
        }
        
        












        /*
        speedX = 0f; //makes the square stop moving when not pressing
        speedY = 0f;

        if(Input.GetKey(KeyCode.W)) //this is where you press the key W, causing the square to go on the Y axis, at 1 speed
        {
            speedY = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            speedY = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            speedX = 1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            speedX = -1f; 
        }*/
    }
}
