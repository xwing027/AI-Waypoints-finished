using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAI : MonoBehaviour
{
    #region Variables
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
    #endregion

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
        if (target == null) //if there is not target to chase
        {
            if (isAIMoving)
            {
                Wander(currentGoal, speed); //wander
            }
            else if (isAIMovingCross)
            {
                Cross(currentGoalCross, speed); //cross - only if wander is not activated
            }
        }
        else
        {
            Chase(target, speed); //otherwise chase target
        }
        
    }

    void Chase(GameObject goal, float currentSpeed) //not in use for the assessment
    {
        //this finds the direction to the goal
        Vector2 direction = (goal.transform.position - transform.position).normalized;
        Vector2 position = transform.position;

        // this movies the ai towards the direction set (aka the goal)
        position += (direction * currentSpeed * Time.deltaTime);
        transform.position = position;
    }

    #region Wander State
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
            NextGoal(); //if not close enough to chase the target, find the next goal
        }
    }

    void NextGoal()
    {
        goalIndex ++;

        //the -1 prevents the index out of range error
        if (goalIndex > goal.Length - 1) //this gets the length of the array, then resets it to 0 when the end is reached
        {
            goalIndex = 0;
        }

        currentGoal = goal[goalIndex];
    }
    #endregion

    #region Cross State
    void Cross(GameObject goalCross, float currentSpeed) //does the same as wander, but follows goals in a different order
    {
        float distance = Vector2.Distance(transform.position, goalCross.transform.position);

        if (distance > 0.01f)
        {
            Chase(goalCross, currentSpeed);
        }
        else
        {
            NextgoalCross();
        }
    }

    void NextgoalCross() //the same as NextGoal but with the Cross order of goals instead
    {
        goalIndexCross++; 

       
        if (goalIndexCross > goalCross.Length - 1) 
        {
            goalIndexCross = 0;
        }

        currentGoalCross = goalCross[goalIndexCross];
    }
    #endregion
}
