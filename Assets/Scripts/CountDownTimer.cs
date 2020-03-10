using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public float currentTime = 0;
    public float startingTime = 600; //in seconds
    public float mod = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
        StartCoroutine( "SubtractFromBonusRoutine" );
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubFromTimeBonus( float amount )
    {
        gameObject.GetComponent<LevelComplete>().time_bonus_gold -= amount;
    }

    IEnumerator SubtractFromBonusRoutine()
    {
        yield return new WaitForSeconds( 60 );
        startingTime -= 60;
        SubFromTimeBonus( 100 );

        if ( startingTime != 0 )
        {
            StartCoroutine( "SubtractFromBonusRoutine" );
        }
    }
}
