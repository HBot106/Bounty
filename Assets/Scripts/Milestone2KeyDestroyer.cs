using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milestone2KeyDestroyer : MonoBehaviour
{
    public GameObject player;
    public GameObject key_lighting;

    private GameManager gm;
    private GameManager GM
    {
        get
        {
            if (gm == null)
            {
                gm = (GameManager)FindObjectOfType(typeof(GameManager));
                //^ this is the important line.
            }
            return gm;
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

    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.gameObject.CompareTag( "Player" ) )
        {
            Debug.Log( "Key got!" );
            player.GetComponent<InventoryManager>().num_keys++;
            GM.AquireGoldKey();
            Destroy( gameObject );
            Destroy( key_lighting );
        }
        
    }
}
