using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Threading;

public class PanelControl : MonoBehaviour
{
    // HUD object - disabled when any menu is opened
    public GameObject HUD;

    // Pause menu
    public GameObject pauseMenu;
    public static bool gamePaused;

    // Pause sub-menus
    public GameObject controlsMenu;
    public GameObject audioMenu;

    // Inventory menu
    public GameObject inventoryMenu;
    public static bool inventoryOpen;

    // Crafting menu
    public GameObject craftingMenu;
    public static bool craftingOpen;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        gamePaused = false;

        inventoryMenu.SetActive(false);
        inventoryOpen = false;

        craftingMenu.SetActive(false);
        craftingOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Pause input
        if (KeyBindController.Instance
            .IsActionPressed(
            KeyBindController.GameAction.pause))
        {
            if (gamePaused)
            {
                ResumeGame();            
            }
            else
            {
                if (inventoryOpen)
                {
                    CloseInventory();
                }
                else if (craftingOpen)
                {
                    CloseCrafting();
                }
                else
                {
                    PauseGame(); 
                }
            }
        }

        // Inventory input
        if (KeyBindController.Instance
            .IsActionPressed(
            KeyBindController.GameAction.inventory))
        {
            if (inventoryOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();              
            }
        }
    }

    // Pauses game, enters Pause menu
    public void PauseGame()
    {
        // disable HUD
        HUD.SetActive(false);

        // enable Pause menu
        pauseMenu.SetActive(true);
        gamePaused = true;

        // unlock cursor
        Cursor.lockState = CursorLockMode.None;

        // ZA WARUDO
        Time.timeScale = 0f;

        // log
        Debug.Log("Game Paused.");
    }

    // Exits Pause menu, resumes game
    public void ResumeGame()
    {
        // disable Pause menu
        pauseMenu.SetActive(false);
        gamePaused = false;

        // disable all Pause sub-menus
        controlsMenu.SetActive(false);
        audioMenu.SetActive(false);

        // enable HUD
        HUD.SetActive(true);

        // Set time in motion
        Time.timeScale = 1f;

        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // log
        Debug.Log("Game Resumed.");
    }

    // Opens Inventory menu
    public void OpenInventory()
    {
        // disable HUD
        HUD.SetActive(false);

        // enable Inventory menu
        inventoryMenu.SetActive(true);
        inventoryOpen = true;

        // unlock cursor
        Cursor.lockState = CursorLockMode.None;

        // log
        Debug.Log("Inventory Opened.");
    }

    // Closes Inventory menu
    public void CloseInventory()
    {
        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // disable Inventory menu
        inventoryMenu.SetActive(false);
        inventoryOpen = false;

        // enable HUD
        HUD.SetActive(true);

        // log
        Debug.Log("Inventory Closed.");
    }

    // Opens Crafting menu
    public void OpenCrafting()
    {
        // disable HUD
        HUD.SetActive(false);

        // enable Crafting menu
        craftingMenu.SetActive(true);
        craftingOpen = true;

        // unlock cursor
        Cursor.lockState = CursorLockMode.None;

        // log
        Debug.Log("Crafting Opened.");
    }

    // Closes Crafting menu
    public void CloseCrafting()
    {
        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // disable Crafting menu
        craftingMenu.SetActive(false);
        craftingOpen = false;

        // enable HUD
        HUD.SetActive(true);

        // log
        Debug.Log("Crafting Opened.");
    }
}