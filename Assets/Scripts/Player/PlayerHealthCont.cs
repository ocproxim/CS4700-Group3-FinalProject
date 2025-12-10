using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthCont : MonoBehaviour
{
    public float health,
    stamina,
    hunger,
    thirst;
    // Start is called before the first frame update
    void Start()
    {
        health = stamina = hunger = thirst = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
