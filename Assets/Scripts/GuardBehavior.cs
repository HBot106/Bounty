using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehavior : MonoBehaviour
{
    public GameObject guardEye;

    public GameObject[] patrolPath;
    public NavMeshAgent guardAgent;
    // Start is called before the first frame update

    private bool isPatroling;
    private int patrolPathIndex;
    private float stopTime;
    private float elapsedTime;

    void Start()
    {
        isPatroling = true;
        patrolPathIndex = 0;
        stopTime = Time.time;
        guardAgent.SetDestination(patrolPath[patrolPathIndex].transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime = Time.time - stopTime;
        if (guardAgent.pathPending)
        {
            return;
        }

        if (guardAgent.remainingDistance <= guardAgent.stoppingDistance)
        {
            if (isPatroling)
            {
                stopTime = Time.time;
                patrolPathIndex = patrolPathIndex + 1 % patrolPath.Length;
            }
            else
            {
                //attack player you have reached them
            }
        }

        if (isPatroling && (elapsedTime > 1.0f))
        {
            guardAgent.SetDestination(patrolPath[patrolPathIndex].transform.position);
        }

    }
}
