using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotions : MonoBehaviour
{
    public HeartsHealthVisual healthUI;
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        healthUI = GameObject.Find( "HeartHealthVisual" ).GetComponent<HeartsHealthVisual>();
        playerMovement = GameObject.Find( "Player" ).GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) && playerMovement.health < 3 )
        {
            Debug.Log( "Health pot picked up!" );
            Destroy( gameObject );
            
            healthUI.Heal( 4 );
            playerMovement.health++;
        }
    }
}
