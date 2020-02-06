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
    private bool patrolTargetReached;
    private bool isInvestigating;
    private bool investigationTargetReached;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
