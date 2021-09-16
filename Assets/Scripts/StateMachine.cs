using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
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
        waypointAI.isAIMoving = true;
        waypointAI.isAIMovingCross = false;

        while (state == State.Wander)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < chaseDistance && player.activeSelf == true) 
            {
                state = State.Chase;
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
        waypointAI.isAIMoving = false;
        
        while (state == State.Stop)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (Time.time > startTime + waitTime)
            {
                state = State.Wander;
            }
            else if (distance < chaseDistance)
            {
                state = State.Chase;
            }


            yield return null;
            //yield return new WaitForSeconds(3f); makes it wait for three seconds
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
            waypointAI.target = player;
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < waypointAI.speed * Time.deltaTime)
            {
                player.SetActive(false); //this 'kills' the player when the AI catches up to it
                state = State.Wander;
            }
            
            if (distance > chaseDistance)
            {
                state = State.Stop;
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
        waypointAI.isAIMovingCross = true;
        waypointAI.isAIMoving = false;

        while (state == State.Cross)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < chaseDistance && player.activeSelf == true)
            {
                state = State.Chase;
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
        switch (state)
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
