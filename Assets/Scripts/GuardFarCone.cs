using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFarCone : MonoBehaviour
{

    public GuardBehavior behaviorScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            behaviorScript.guard_far_detection_cone_active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            behaviorScript.guard_far_detection_cone_active = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
