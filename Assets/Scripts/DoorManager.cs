using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public GameObject player;
    public GameObject thisDoor;
    public GameObject unlockPopup;
    public GameObject popupScript;
    public float transformXVal = 0;
    public float transformYVal = 0;
    public float transformZVal = 0;
    public GUIStyle lockStyle;
    private bool door_is_open = false;
    public bool door_is_locked = true;
    public bool unlock_current_door = false;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = ( GameManager ) FindObjectOfType( typeof( GameManager ) );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenDoor()
    {
        Destroy( thisDoor.gameObject );
        gm.UseKey( gm.goldKey );
    }

    IEnumerator WaitForUIRoutine()
    {
        Debug.Log( "returned from button click" );
        yield return new WaitForSeconds( 5 );
        Debug.Log( "coroutine finished" );
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.tag == "Player" && door_is_locked )
        {
            if ( player.GetComponent<InventoryManager>().num_keys > 0 )
            {
                door_is_locked = false;
                player.GetComponent<InventoryManager>().num_keys -= 1;
            }
            else
            {
                unlockPopup.SetActive( true );
                Cursor.lockState = CursorLockMode.None;
            }   
        }
        
        if ( other.tag == "Player" && !door_is_locked )
        {
            OpenDoor();
            door_is_open = true;
        }
    }
}
