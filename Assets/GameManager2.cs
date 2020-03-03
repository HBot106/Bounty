using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{

    public GameObject Player;
    public GameObject KnifeCount;
    public GameObject RockCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KnifeCount.GetComponent<Text>().text = ""+PlayerMovement.number_of_knives;
        RockCount.GetComponent<Text>().text = "" + PlayerMovement.number_of_rocks;
    }
}
