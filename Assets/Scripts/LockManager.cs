using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public GameObject player;
    public GameObject thisDoor;
    private bool encountered_locked_door = false;
    private bool unlockIt = false;
    private string locked_text = "The door is locked.";
    private int lock_window_width = 400;
    private int lock_window_height = 200;
    public GUIStyle lockStyle;
    private bool door_is_open = false;
    public bool door_is_locked = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGUI()
    {
        int windowXVal = ( Screen.width / 2 - lock_window_width / 2 );
        int windowYVal = ( Screen.height / 2 - lock_window_height / 2 );

        if ( encountered_locked_door )
        {
            GUI.Box( new Rect( windowXVal, windowYVal, lock_window_width, lock_window_height ), locked_text );

            if ( player.GetComponent<InventoryManager>().num_keys > 0 )
            {
                if ( GUI.Button( new Rect( windowXVal + 10, windowYVal + 50, 100, 50 ), "Ulock" ) )
                {
                    print( "Click!" );
                    encountered_locked_door = false;
                    door_is_locked = false;
                }
                else if ( GUI.Button( new Rect( windowXVal + 150, windowYVal + 50, 100, 50 ), "Leave" ) )
                {
                   print( "You leave the door locked" );
                }
            }  
        }
        
    }

    private void OpenDoor()
    {
        thisDoor.transform.position += new Vector3( 3.8f, 0, 0 );
    }

    private void CloseDoor()
    {
        thisDoor.transform.position -= new Vector3( 3.8f, 0, 0 );
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.tag == "Player" && door_is_locked )
        {
            encountered_locked_door = true;
        }
        else if ( other.tag == "Player" && !door_is_locked)
        {
            OpenDoor();
            door_is_open = true;
        }
       
    }

    private void OnTriggerExit( Collider other )
    {
        if ( door_is_open )
        {
            CloseDoor();
            door_is_open = false;
        }
    }
}
