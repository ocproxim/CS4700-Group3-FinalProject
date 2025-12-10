using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadMainMenuScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameplayScene()
    {
        // Set time in motion
        Time.timeScale = 1f;
        
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}