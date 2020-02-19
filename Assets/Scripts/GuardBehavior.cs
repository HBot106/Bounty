using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehavior : MonoBehaviour
{
    public GameObject guardEye;
    public GameObject swordHand;

    public GameObject[] patrolPath;
    public NavMeshAgent guardAgent;

    public GameObject player;
    // Start is called before the first frame update

    public bool isPatroling;
    private int patrolPathIndex;
    private float stopTime;
    private float elapsedTime;
    public bool canSeePlayer;
    private float stoppingDistance = 2f;

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
        canSeePlayer = false;
        isPatroling = true;
        patrolPathIndex = 0;
        swingDelayCounter = 0;
        stopTime = Time.time;
        guardAgent.SetDestination(patrolPath[patrolPathIndex].transform.position);
        rgdbdy = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canSeePlayer)
        {
            swingSword();
        }
        if (guardAgent.pathPending)
        {
            return;
        }

        elapsedTime = Time.time - stopTime;

        Vector3 lookingAt = guardAgent.destination - transform.position;
        Vector3 toPlayer = player.transform.position - transform.position;
        float visualAngle = Vector3.Angle(Vector3.Normalize(lookingAt), Vector3.Normalize(toPlayer));

        // NavMeshHit hit;
        // if (!guardAgent.Raycast(player.transform.position, out hit))
        // {
        //     canSeePlayer = true;
        // }
        // else
        // {
        //     canSeePlayer = false;
        // }

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(guardEye.transform.position, Vector3.Normalize(player.transform.position - guardEye.transform.position), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(guardEye.transform.position, Vector3.Normalize(player.transform.position - guardEye.transform.position) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
                canSeePlayer = true;
            }
            else
            {
                Debug.DrawRay(guardEye.transform.position, Vector3.Normalize(player.transform.position - guardEye.transform.position) * 1000, Color.white);
                //Debug.Log("Did not Hit");
                canSeePlayer = false;
                // isPatroling = true;
            }

        }

        if (canSeePlayer)
        {
            isPatroling = false;
            guardAgent.SetDestination(player.transform.position - transform.forward * stoppingDistance);
        }
        
        if (guardAgent.remainingDistance <= guardAgent.stoppingDistance)
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
            guardAgent.SetDestination(patrolPath[patrolPathIndex].transform.position);
        }

    }

    private void swingSword()
    {
        if(swingState == 1)
        {
            swordHand.transform.Rotate(Vector3.up, -swingSpeed);
            if (swordHand.transform.localEulerAngles.y >= swingEndAngle &&
                swordHand.transform.localEulerAngles.y <= swingStartAngle)
            {
                swingState = 4;
            }
        }
        else if(swingState == 2)
        {
            swordHand.transform.Rotate(Vector3.up, swingSpeed);
            if (swordHand.transform.localEulerAngles.y <= swingStartAngle &&
                swordHand.transform.localEulerAngles.y >= swingEndAngle)
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
