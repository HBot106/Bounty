using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehavior : MonoBehaviour
{
    public GameObject guardEye;

    public GameObject[] patrolPath;
    public NavMeshAgent guardAgent;

    public GameObject player;
    // Start is called before the first frame update

    private bool isPatroling;
    private int patrolPathIndex;
    private float stopTime;
    private float elapsedTime;
    private bool canSeePlayer;

    void Start()
    {
        canSeePlayer = false;
        isPatroling = true;
        patrolPathIndex = 0;
        stopTime = Time.time;
        guardAgent.SetDestination(patrolPath[patrolPathIndex].transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
                Debug.Log("Did Hit");
                canSeePlayer = true;
            }
            else
            {
                Debug.DrawRay(guardEye.transform.position, Vector3.Normalize(player.transform.position - guardEye.transform.position) * 1000, Color.white);
                Debug.Log("Did not Hit");
                canSeePlayer = false;
                isPatroling = true;
            }

        }
        

        if (canSeePlayer)
        {
            isPatroling = false;
            guardAgent.SetDestination(player.transform.position);
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
                isPatroling = true;
            }
        }

        if (isPatroling && (elapsedTime > 1.0f))
        {
            guardAgent.SetDestination(patrolPath[patrolPathIndex].transform.position);
        }

    }
}
