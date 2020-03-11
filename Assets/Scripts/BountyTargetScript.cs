using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyTargetScript : MonoBehaviour
{
    public GameObject capturePopup;
    public GameObject levelComplete;
    public GameObject target;
    public GameObject player;
    public int hit_points = 1;
    public Vector3 offset_to_player = new Vector3( 0, 8, 0 );

    private Animator targetAnimator;
    private bool capture_is_go = false;
    public bool target_is_captured = false;
    public bool target_killed = false;

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

        if ( target_is_captured )
        {
            target.transform.position = player.transform.position + offset_to_player;
            target.transform.rotation = player.transform.rotation;

            player.GetComponent<PlayerMovement>().playerSpeed = 3.0f;
        }
        else if ( hit_points == 0 )
        {
            targetAnimator.SetBool( "Death_b", true );
            targetAnimator.SetInteger( "DeathType_int", 1 );
            capturePopup.SetActive( false );

            target_killed = true;
            //player.GetComponent<LevelComplete>().level_is_complete = true;
            //levelComplete.SetActive( true );
            //Cursor.lockState = CursorLockMode.None;
        }

        if ( player.GetComponent<LevelComplete>().level_is_complete )
        {
            levelComplete.SetActive( true );
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag( "PlayerSword" ) || other.gameObject.CompareTag( "PlayerKnifeProjectile" ) )
        {
            hit_points -= 1;
        }
        
        if ( other.gameObject.CompareTag( "Ladder" ) )
        {
            player.GetComponent<LevelComplete>().level_is_complete = true;
            levelComplete.SetActive( true );
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerStay( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) )
        {
            capturePopup.SetActive( true );

            if ( capture_is_go )
            {
                targetAnimator.SetBool( "Death_b", true );
                targetAnimator.SetInteger( "DeathType_int", 2 );

                target_is_captured = true;
                capturePopup.SetActive( false );
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

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject.CompareTag( "Ladder" ) )
        {
            
        }
    }
}
