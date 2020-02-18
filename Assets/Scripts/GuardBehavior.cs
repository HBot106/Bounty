using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class GuardBehavior : MonoBehaviour
{

    // STATE MACROS
    private static int STATE_GUARDING = 0;
    private static int STATE_PATROLLING = 1;
    private static int STATE_CHASING = 2;
    private static int STATE_FIGHTING = 3;
    private static int STATE_DYING = 4;

    // guards state information
    private int guardState;
    private int guardHealth;
    private bool guardIsInvestigating;
    private bool guardCanSeePlayer;
    private float guardStoppingDistance = 2.0f;
    public GameObject guardEye;
    public GameObject guardSwordHand;
    public NavMeshAgent guardNavAgent;
    public GameObject[] guardPatrolPoints;
    private int patrolPointIndex;
    public int patrolPointCount;
    private Vector3 pointOfInterest;
    private GameObject player;
    

    // 0 = not swinging, 1 = swinging right, 2 = swinging left, 
    // 3 = waiting to swing right, 4 = waiting to swing left
    public int swingState; 

    public float swingSpeed;
    public float swingDelay;
    private float swingDelayCounter;
    private float swingStartAngle = 250f;
    private float swingEndAngle = 100f;
    private Rigidbody rgdbdy;
    public float playerHitForce;
    private float life = 2;

    void Start()
    {
        guardCanSeePlayer = false;
        isPatroling = true;
        patrolPathIndex = 0;
        swingDelayCounter = 0;
        stopTime = Time.time;
        guardNavAgent.SetDestination(patrolPath[patrolPathIndex].transform.position);
        rgdbdy = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(guardCanSeePlayer)
        {
            swingSword();
        }

        if (guardNavAgent.pathPending)
        {
            return;
        }

        elapsedTime = Time.time - stopTime;

        Vector3 lookingAt = guardNavAgent.destination - transform.position;
        Vector3 toPlayer = player.transform.position - transform.position;
        float visualAngle = Vector3.Angle(Vector3.Normalize(lookingAt), Vector3.Normalize(toPlayer));

        // NavMeshHit hit;
        // if (!guardNavAgent.Raycast(player.transform.position, out hit))
        // {
        //     guardCanSeePlayer = true;
        // }
        // else
        // {
        //     guardCanSeePlayer = false;
        // }

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(guardEye.transform.position, Vector3.Normalize(player.transform.position - guardEye.transform.position), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(guardEye.transform.position, Vector3.Normalize(player.transform.position - guardEye.transform.position) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
                guardCanSeePlayer = true;
            }
            else
            {
                Debug.DrawRay(guardEye.transform.position, Vector3.Normalize(player.transform.position - guardEye.transform.position) * 1000, Color.white);
                //Debug.Log("Did not Hit");
                guardCanSeePlayer = false;
                // isPatroling = true;
            }

        }

        if (guardCanSeePlayer)
        {
            isPatroling = false;
            guardNavAgent.SetDestination(player.transform.position - transform.forward * guardStoppingDistance);
        }
        
        if (guardNavAgent.remainingDistance <= guardNavAgent.guardStoppingDistance)
        {
            if (isPatroling)
            {
                stopTime = Time.time;
                patrolPathIndex = (patrolPathIndex + 1) % 5;
            }
            else
            {
                //attack player you have reached them
                stopTime = Time.time;
                //isPatroling = true;
            }
        }

        if (isPatroling && (elapsedTime > 1.0f))
        {
            guardNavAgent.SetDestination(patrolPath[patrolPathIndex].transform.position);
        }

    }

    private void swingSword()
    {
        if(swingState == 1)
        {
            guardSwordHand.transform.Rotate(Vector3.up, -swingSpeed);
            if (guardSwordHand.transform.localEulerAngles.y >= swingEndAngle &&
                guardSwordHand.transform.localEulerAngles.y <= swingStartAngle)
            {
                swingState = 4;
            }
        }
        else if(swingState == 2)
        {
            guardSwordHand.transform.Rotate(Vector3.up, swingSpeed);
            if (guardSwordHand.transform.localEulerAngles.y <= swingStartAngle &&
                guardSwordHand.transform.localEulerAngles.y >= swingEndAngle)
            {
                swingState = 3;
            }
        }
        else if(swingState == 3 || swingState == 4)
        {
            swingDelayCounter++;
            if (swingDelayCounter >= swingDelay)
            {
                swingState -= 2;
                swingDelayCounter = 0;
            }
        }
        else
        {
            swingState = 1;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSword"))
        {
            if(isPatroling)
            {
                Debug.Log("Guard Assassinated!");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Guard Hit!");

                Vector3 hitDirection = (transform.position - other.transform.root.transform.position).normalized + Vector3.up;
                rgdbdy.AddForce(hitDirection * playerHitForce, ForceMode.Impulse);
                life--;
                if(life == 0)
                {
                    Debug.Log("Guard Killed!");
                    Destroy(gameObject);
                }
            }
        }
    }
}
