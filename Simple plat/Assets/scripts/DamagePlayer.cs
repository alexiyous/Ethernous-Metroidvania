using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damageAmount;

    public bool destroyOnDamage;
    public GameObject destroyEffect;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")//if it hits and collide with the "player", it will deal damage with the damageAmount;
        {
            DealDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")//if it hits and collide with the "player", it will deal damage with the damageAmount;
        {
            DealDamage();
        }
    }

    //as the name implies, its for dealing damage that are connected to the EnemyHealthController script
    void DealDamage()
    {
        PlayerHealthController.instance.DamagePlayer(damageAmount);

        //function so that when the enemy hits the player, it will explode and deal damage (sort of like a taget missile enemy type)
        if (destroyOnDamage)
        {
            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
