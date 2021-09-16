using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAI : MonoBehaviour
{
    [SerializeField] public float speed = 1f;
    [SerializeField] private GameObject[] goal;
    [SerializeField] private GameObject[] goalCross;
    private GameObject currentGoal;
    private GameObject currentGoalCross;
    private int goalIndex = 0;
    private int goalIndexCross = 0;

    public bool isAIMoving = true;
    public bool isAIMovingCross = true;

    public GameObject target;

    void Start()
    {
        currentGoal = goal[goalIndex];
        currentGoalCross = goalCross[goalIndexCross];
    }

    void Update()
    {
        if (isAIMoving == false && isAIMovingCross == false)
        {
            return; //this exits the method early, if the ai is not moving
        }
        if (target == null)
        {
            if (isAIMoving)
            {
                Wander(currentGoal, speed);
            }
            else if (isAIMovingCross)
            {
                Cross(currentGoalCross, speed);
            }
        }
        else
        {
            Chase(target, speed);
        }
        
    }

    void Chase(GameObject goal, float currentSpeed)
    {
        //direction from a to b
        //direction = b-a 

        //this finds the direction to the goal
        Vector2 direction = (goal.transform.position - transform.position).normalized;
        Vector2 position = transform.position;

        // this movies the ai towards the direction set (aka the goal)
        position += (direction * currentSpeed * Time.deltaTime);
        transform.position = position;
    }

    void Wander(GameObject goal, float currentSpeed)
    {
        //this gets the distance to the goal
        float distance = Vector2.Distance(transform.position, goal.transform.position);

        if (distance > 0.01f) //this makes the square stop when it touches the circles, instead of bouncing around
        {
            Chase(goal, currentSpeed);
        }
        else
        {
            NextGoal();
        }
    }

    void NextGoal()
    {
        goalIndex ++; //goalindex+1 also increases by 1

        //the -1 prevents the index out of range error
        if (goalIndex > goal.Length - 1) //this gets the length of the array, then resets it to 0 when the end is reached
        {
            goalIndex = 0;
        }

        currentGoal = goal[goalIndex];
    }

    void Cross(GameObject goalCross, float currentSpeed)
    {
        //this gets the distance to the goal
        float distance = Vector2.Distance(transform.position, goalCross.transform.position);

        if (distance > 0.01f) //this makes the square stop when it touches the circles, instead of bouncing around
        {
            Chase(goalCross, currentSpeed);
        }
        else
        {
            NextgoalCross();
        }
    }

    void NextgoalCross()
    {
        goalIndexCross++; //goalindex+1 also increases by 1

        //the -1 prevents the index out of range error
        if (goalIndexCross > goalCross.Length - 1) //this gets the length of the array, then resets it to 0 when the end is reached
        {
            goalIndexCross = 0;
        }

        currentGoalCross = goalCross[goalIndexCross];
    }
}
