using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundController : MonoBehaviour
{
    public static float
        masterVolume = 100f,
        musicVolume = 100f,
        playerVolume = 100f,
        animalVolume = 100f,
        worldVolume = 100f;

    // Start is called before the first frame update
    void Start()
    {
        masterVolume = 100f;
        musicVolume = 100f;
        playerVolume = 100f;
        animalVolume = 100f;
        worldVolume = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
