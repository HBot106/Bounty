using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    public TextMeshProUGUI total_gold_text;
    public TextMeshProUGUI bounty_gold_text;
    public TextMeshProUGUI capture_bonus_gold_text;
    public TextMeshProUGUI time_bonus_gold_text;

    public float contract_gold = 2000.0f;
    public float capture_bonus_gold = 1000.0f;
    public float capture_bonus_earned = 0;
    public float time_bonus_gold = 0;
    public float total_gold = 0;

    public GameObject bounty;
    public bool level_is_complete = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( level_is_complete )
        {
            CalcTotalGold();
            DesplayPlayerGold();
        }
    }

    public void CalcTotalGold()
    {
        if ( bounty.GetComponent<BountyTargetScript>().target_is_captured )
        {
            capture_bonus_earned = capture_bonus_gold;
        }

        total_gold = contract_gold + capture_bonus_earned + time_bonus_gold;
    }

    public void DesplayPlayerGold()
    {
        bounty_gold_text.text = "Contract Fulfilled: " + contract_gold + "g";
        capture_bonus_gold_text.text = "Capture Bonus: " + capture_bonus_earned + "g";
        time_bonus_gold_text.text = "Time Bonus: " + time_bonus_gold + "g";
        total_gold_text.text = "Total Gold: " + total_gold + "g";
    }

    public void ReturnToGameMenu()
    {
        SceneManager.LoadScene( "MainMenu" );
    }
}
