using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    public void PlayLevel1()
    {
        SceneManager.LoadScene(4);
    }

    public void PlayMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene( "GuardTesting" );
    }

    public void PlayLevel3()
    {
        SceneManager.LoadScene( "" );
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}