using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyTargetScript : MonoBehaviour
{
    public GameObject capturePopup;
    public int hit_points = 3;
    private Animator targetAnimator;
    private bool capture_is_go = false;

    // Start is called before the first frame update
    void Start()
    {
        targetAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.R ) )
        {
            capture_is_go = true;
        }
    }

    private void OnTriggerStay( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) && hit_points == 1 )
        {
            capturePopup.SetActive( true );

            if ( capture_is_go )
            {
                targetAnimator.SetBool( "Death_b", true );
                targetAnimator.SetInteger( "DeathType_int", 2 );
            }
        }
    }

    private void OnTriggerExit( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) )
        {
            capturePopup.SetActive( false );
        }
    }
}
