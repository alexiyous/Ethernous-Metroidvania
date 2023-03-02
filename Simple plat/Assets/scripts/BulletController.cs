using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D theRB;

    public Vector2 moveDIR;

    public GameObject impactEffect;
    public SpriteRenderer impactTest;
    private float impactLifetime = .1f;

    public int damageAmount = 1;
    // Update is called once per frame
    void Update()
    {
        theRB.velocity = bulletSpeed * moveDIR; 
    }

    // a function so that the bullet will gone everytime it collides with a collision (ex:wall,gorund,etc)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")//everytime the bullet hits the enemy, it calls the damaging function so it deplete the health
        {
            other.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }

        if (other.tag == "Boss")
        {
            BossHealthController.instance.TakeDamage(damageAmount);
        }
        //this part of the program is supposed to make a particle effect of an impact if there is
        //a game object as the refference for impactEffect everytime the bullet
        //collides with a collision
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
            //quaternion identity is a way to tell that there are no rotation for the parameters
        }

        if (impactTest != null)
        {
            ShowImpactTest();
        }

        AudioManager.instance.PlaySFXAdjusted(3);//play bullet impact SFX

        Destroy(gameObject);
    }

    // a function so that the bullet will gone when it's off screen or off the camera range of view
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void ShowImpactTest()
    {
        SpriteRenderer test = Instantiate(impactTest, transform.position, Quaternion.identity);

        Destroy(test.gameObject, impactLifetime);
    }
}