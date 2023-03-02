using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount;
    public GameObject pickupEffect;

    public string healthPickupRef;

    private void Start()
    {
        if (PlayerPrefs.HasKey(healthPickupRef))
        {
            if (PlayerPrefs.GetInt(healthPickupRef) == 1)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)//when player enters the collision area of health pickup, then it will heal for the amount of
                                                  //health costumized
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.HealPlayer(healthAmount);

            if(pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);

            PlayerPrefs.SetInt(healthPickupRef, 1);

            AudioManager.instance.PlaySFX(5);//play health pickup SFX
        }
        
    }
}
