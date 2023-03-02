using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    public AudioManager theAM;

    private void Awake()
    {
        if (AudioManager.instance == null) //if there are no audioManager gameobject in this peculiar scene, instantiate
                                           //a new audiomanager and set it into the current scene
        {
            AudioManager newAM = Instantiate(theAM);
            AudioManager.instance = newAM;
            DontDestroyOnLoad(newAM.gameObject);
        }
    }
}
