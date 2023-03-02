using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int totalHealth = 3;
    public GameObject deathEffect;

    public void DamageEnemy(int damageAmount) //function for damaging enemy
    {
        totalHealth -= damageAmount;

        if (totalHealth <= 0)
        {
            if(deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);

            AudioManager.instance.PlaySFX(4);//play enemy death SFX
        }
    }
}
