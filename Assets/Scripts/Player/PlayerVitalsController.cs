using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVitalsController : MonoBehaviour
{
    public static PlayerVitalsController Instance;

    public float
        health = 100f,
        maxHealth = 100f,

        stamina = 100f,
        maxStamina = 100f,

        hunger = 100f,
        maxHunger = 100f,

        thirst = 100f,
        maxThirst = 100f,

        decayFactor = 0.25f;

    // Initialize Instance
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadVitals();
    }

    // Update is called once per frame
    void Update()
    {
        changeHunger(-1 * Time.deltaTime * decayFactor);
        changeThirst(-1 * Time.deltaTime * decayFactor);

        // Health and Stamina decay if hunger thirst are 0
        if(hunger == 0 && thirst == 0)
        {
            changeHealth(-1 * Time.deltaTime * decayFactor);

            maxStamina = 50;
            changeStamina(-1 * Time.deltaTime * decayFactor * 2);
        }

        SaveVitals();
    }


    // Update health by an added amount
    public void changeHealth(float amt)
    {
        health += amt;

        if(health < 0)
        {
            health = 0;

            Debug.Log("Perished");
            SceneManager.LoadScene("MainMenu");
        }

        health = Mathf.Min(health, maxHealth);
    }

    // Update stamina by an added amount
    public void changeStamina(float amt)
    {
        stamina += amt;

        stamina = Mathf.Max(stamina, 0);

        stamina = Mathf.Min(stamina, maxStamina);
    }

    // Update hunger by an added amount
    public void changeHunger(float amt)
    {
        hunger += amt;

        hunger = Mathf.Max(hunger, 0);

        hunger = Mathf.Min(hunger, maxHunger);
    }

    // Update thirst by an added amount
    public void changeThirst(float amt)
    {
        thirst += amt;

        thirst = Mathf.Max(thirst, 0);

        thirst = Mathf.Min(thirst, maxThirst);
    }

    // Save vitals to player prefs
    public void SaveVitals()
    {
        PlayerPrefs.SetFloat("Player_Health", health);
        PlayerPrefs.SetFloat("Player_MaxHealth", maxHealth);

        PlayerPrefs.SetFloat("Player_Stamina", stamina);
        PlayerPrefs.SetFloat("Player_MaxStamina", maxStamina);

        PlayerPrefs.SetFloat("Player_Hunger", hunger);
        PlayerPrefs.SetFloat("Player_MaxHunger", maxHunger);

        PlayerPrefs.SetFloat("Player_Thirst", thirst);
        PlayerPrefs.SetFloat("Player_MaxThirst", maxThirst);

        PlayerPrefs.Save();
    }

    // Load vitals from player prefs
    private void LoadVitals()
    {
        health = PlayerPrefs.GetFloat("Player_Health", 100);
        maxHealth = PlayerPrefs.GetFloat("Player_MaxHealth", 100);

        stamina = PlayerPrefs.GetFloat("Player_Stamina", 100);
        maxStamina = PlayerPrefs.GetFloat("Player_MaxStamina", 100);

        hunger = PlayerPrefs.GetFloat("Player_Hunger", 100);
        maxHunger = PlayerPrefs.GetFloat("Player_MaxHunger", 100);

        thirst = PlayerPrefs.GetFloat("Player_Thirst", 100);
        maxThirst = PlayerPrefs.GetFloat("Player_MaxThirst", 100);

        Debug.Log("Player vitals loaded.");
    }
}
