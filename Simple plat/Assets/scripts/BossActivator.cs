using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject bossToActivate;

    public string bossRef;

    public GameObject escalator, gate;


    //when entering the Collider zone, it activate the boss that was deactivated
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerPrefs.HasKey(bossRef))//check if there is a value stored in bossRef
            {
                if (PlayerPrefs.GetInt(bossRef) != 1)//if the stored value is not equal to 1 it woulf activate the boss
                {
                    bossToActivate.SetActive(true);

                    gameObject.SetActive(false);//deactivated this collider zone
                }

                escalator.SetActive(false);
                gate.SetActive(false);
            }
            else //if there are no stored value it would activated the boss
            {
                bossToActivate.SetActive(true);

                gameObject.SetActive(false);//deactivated this collider zone
            }
        }
    }
}
