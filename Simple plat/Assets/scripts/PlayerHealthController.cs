using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);//it tells unity to not destroy the gameobject that this script attached to everytime
                                          //it loads a scene.
        }
        else
        {
            Destroy(gameObject);
        }
    }

 /*   [HideInInspector]*/
    public int currentHealth;
    public int maxHealth;

    public float invincibilityLength;
    private float invinceCounter;

    public float flashLength;
    private float flashCounter;

    public SpriteRenderer[] playerSprites;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (invinceCounter > 0)
        {
            invinceCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                {
                    sr.enabled = !sr.enabled;
                }
                flashCounter = flashLength;
            }

            if (invinceCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                {
                    sr.enabled = true;
                }
                flashCounter = 0f;
            }
        }
    }

    public void DamagePlayer(int damageAmount) //function for damaging the player (deplete the health bar)
    {
        if (invinceCounter <= 0)
        {

            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                /*gameObject.SetActive(false);*/

                RespawnController.instance.Respawn();

                AudioManager.instance.PlaySFX(8);//play player death SFX
            }
            else
            {
                invinceCounter = invincibilityLength;

                AudioManager.instance.PlaySFXAdjusted(11);//play player hurt SFX
            }

            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    //function to refill the health after respawn
    public void FillHealth()
    {
        currentHealth = maxHealth;

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    //function to heal player when pickup a health
    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}
