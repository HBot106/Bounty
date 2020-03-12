using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] potions;
    public GameObject potionPrefab;

    public Vector3[] potionSpawnPoints;

    public GameObject libraryPotion;
    public GameObject mainBuildingPotion;
    public GameObject leaderRoomPotion;
    public PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GameObject.Find( "Player" ).GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( playerMovement.health < 3 )
        {
            for ( int i = 0; i < 3; i++ )
            {
                if ( potions[i] == null )
                {
                   potions[i] = Instantiate( potionPrefab, potionSpawnPoints[i], transform.rotation );
                }
            }
        }
    }
}
