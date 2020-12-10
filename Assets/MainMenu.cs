﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }
    
    public void QuitGame()
    {
        Debug.Log("Siup");
        Application.Quit();
    }
}
