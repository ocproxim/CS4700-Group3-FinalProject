using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalVisual : MonoBehaviour
{
    public Slider
        healthBar,
        staminaBar,
        hungerBar,
        thirstBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = 100f;
        staminaBar.maxValue = 100f;
        hungerBar.maxValue = PlayerVitalsController.Instance.maxHunger;
        thirstBar.maxValue = PlayerVitalsController.Instance.maxThirst;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = PlayerVitalsController.Instance.health;
        staminaBar.value = PlayerVitalsController.Instance.stamina;
        hungerBar.value = PlayerVitalsController.Instance.hunger;
        thirstBar.value = PlayerVitalsController.Instance.thirst;
    }
}
