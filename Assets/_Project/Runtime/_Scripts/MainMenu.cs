using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    

    public void StartLevel(int levelIndex)
    {
        Debug.Log("Start level: " + levelIndex);
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }



}
