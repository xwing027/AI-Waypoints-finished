using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State //this sets up the states that will be used
{
    Wander,
    Stop,
    Cross,
    Chase,
}

public class StateMachine : MonoBehaviour
{
    #region Variables

    public State state;
    public float chaseDistance = 5f;

    private SpriteRenderer sprite = null;
    private WaypointAI waypointAI = null;
    private GameObject player;

    private Vector2 target = new Vector2(6, 4);
    #endregion

    #region States
    private IEnumerator WanderState()
    {
        Debug.Log("Wander: Enter");
        sprite.color = Color.green; //the square will turn green when in a wander state (moving)
        waypointAI.isAIMoving = true; //wander state is active
        waypointAI.isAIMovingCross = false; //cross state is inactive

        while (state == State.Wander)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position); //this finds the distance to the player

            if (distance < chaseDistance && player.activeSelf == true) 
            {
                state = State.Chase; //if close enough, and the player is active, chase the player
            }
            yield return null;
        }
        
        Debug.Log("Wander: Exit");
        NextState();
    }

    private IEnumerator StopState()
    {
        float startTime = Time.time;
        float waitTime = 3f;
        
        Debug.Log("Stop: Enter");
        sprite.color = Color.red; //the square will turn red when in a stop state (not moving)
        waypointAI.isAIMoving = false; //stops wander state
        waypointAI.isAIMovingCross = false; //stops cross state
        
        while (state == State.Stop)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position); //finds the distance to the player
            
            if (Time.time > startTime + waitTime)
            {
                state = State.Wander; //after waiting, and if the player is still far, wander
            }
            else if (distance < chaseDistance)
            {
                state = State.Chase; //if the player is close, stop waiting and chase
            }
            yield return null;
        }
        
        waypointAI.isAIMoving = true;
        Debug.Log("Stop: Exit");
        NextState();
    }

    private IEnumerator ChaseState()
    {
        Debug.Log("Chase: Enter");
        sprite.color = Color.blue; //the square will turn red when in a stop state (not moving)
        while (state == State.Chase)
        {
            waypointAI.target = player; //player becomes the target
            
            float distance = Vector2.Distance(transform.position, player.transform.position); //getting the distance from the player

            if (distance < waypointAI.speed * Time.deltaTime)
            {
                player.SetActive(false); //this 'kills' the player when the AI catches up to it
                state = State.Wander;
            }
            
            if (distance > chaseDistance)
            {
                state = State.Stop; //if the player is too far, enter stop state
            }
                
            yield return null;
        }
        
        waypointAI.target = null;
        Debug.Log("Chase: Exit");
        NextState();
    }

    public IEnumerator CrossState()
    {
        Debug.Log("Cross: Enter");
        sprite.color = Color.magenta; //the square will turn green when in a wander state (moving)
        waypointAI.isAIMovingCross = true; //cross state is active
        waypointAI.isAIMoving = false; //wander state is inactive

        while (state == State.Cross)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position); //find distance to the player

            if (distance < chaseDistance && player.activeSelf == true)
            {
                state = State.Chase; //if close enough to player, enter chase state
            }
            yield return null;
        }
        Debug.Log("Cross: Exit");
        NextState();
    }
    #endregion

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.LogError("Sprite is null");
        }
        
        waypointAI = GetComponent<WaypointAI>();
        if (waypointAI == null)
        {
            Debug.LogError("waypointAI is null, there is none on this object");
        }

        Player playerFound = FindObjectOfType<Player>();
        if (playerFound != null)
        {
            player = playerFound.gameObject;
        }

        NextState();
    }

    private void NextState()
    {
        switch (state) //this allows us to switch through the states without breaking
        {
            case State.Wander:
                StartCoroutine(WanderState());
                break;
            case State.Stop:
                StartCoroutine(StopState());
                break;
            case State.Cross:
                StartCoroutine(CrossState());
                break;
            case State.Chase:
                StartCoroutine(ChaseState());
                break;
            default:
                StartCoroutine(StopState());
                break;

        }
    }
}
