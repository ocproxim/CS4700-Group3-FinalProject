using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        SavePrefs();
    }

    public void SavePrefs()
    {
        // Player Location


        // Player Orientation


        // Player Vitals


        // Player Inventory


        // Time


        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {

    }
}