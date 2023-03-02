using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextGate : MonoBehaviour
{
    public GameObject gate, escalator, door;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Dark"))
        {
            escalator.SetActive(false);
            gate.SetActive(false);
            door.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
