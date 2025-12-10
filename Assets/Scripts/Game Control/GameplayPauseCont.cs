using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameplayPauseCont : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject HUD;

    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        // ZA WARUDO
        Time.timeScale = 0f;

        // disable HUD
        HUD.SetActive(false);

        // unlock cursor
        Cursor.lockState = CursorLockMode.None;

        // enable pause menu
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        //disable pause menu
        pauseMenu.SetActive(false);
        isPaused = false;

        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // enable HUD
        HUD.SetActive(true);

        // Set time in motion
        Time.timeScale = 1f;
    }
}
