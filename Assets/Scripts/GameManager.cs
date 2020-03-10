using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int daggerCount = 2;
    public int rockCount = 3;
    public TextMeshProUGUI daggerText;
    public TextMeshProUGUI rockText;
    public TextMeshProUGUI SpottedText;
    public Image goldKey;
    public Image silverKey;
    public Image ironKey;

    // Start is called before the first frame update
    void Start()
    {
        UseKey(goldKey);
        UseKey(silverKey);
        UseKey(ironKey);
        daggerText.text = "" + daggerCount;
        rockText.text = "" + rockCount;
        hideSpotted();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            showSpotted();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            UpdateRockCountDecrease();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            UpdateDaggerCountIncrease();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            UpdateRockCountIncrease();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            UseKey(goldKey);
            UseKey(silverKey);
            UseKey(ironKey);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            AquireKey(goldKey);
            AquireKey(silverKey);
            AquireKey(ironKey);
        }
    }

    //decrease the amount of daggers
    public void UpdateDaggerCountDecrease()
    {
        daggerCount--;
        daggerText.text = "" + daggerCount;
    }

    //decrease the amount of rocks
    public void UpdateRockCountDecrease()
    {
        rockCount--;
        rockText.text = "" + rockCount;
    }

    //increase the amount of daggers
    void UpdateDaggerCountIncrease()
    {
        daggerCount++;
        daggerText.text = "" + daggerCount;
    }

    //increase the amount of rocks
    void UpdateRockCountIncrease()
    {
        rockCount++;
        rockText.text = "" + rockCount;
    }

    //make key solid
    void AquireKey(Image key)
    {
        key.color = new Color(1, 1, 1, 1.0f);
    }

    public void AquireGoldKey()
    {
        goldKey.color = new Color(1, 1, 1, 1.0f);
    }

    void UseKey(Image key)
    {
        key.color = new Color(1, 1, 1, 0.2f);
    }

    public void showSpotted()
    {
        SpottedText.color = new Color(1, 0, 0, 1.0f);
    }

    public void hideSpotted()
    {
        SpottedText.color = new Color(1, 0, 0, 0.0f);
    }
}
