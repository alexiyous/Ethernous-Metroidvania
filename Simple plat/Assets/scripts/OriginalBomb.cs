using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalBomb : MonoBehaviour
{
    public float timeToExplode = .5F;
    public GameObject explosion;

    public float blastRange;
    public LayerMask whatIsDestructible;
    public LayerMask whatIsDamageable;

    public int bombToPlayerDamage = 1;
    public int damageAmount = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //bomb function, it will calls the explosion after certain amount of time(timeToExplode)
        timeToExplode -= Time.deltaTime;
        if (timeToExplode <= 0)
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            Destroy(gameObject);

            //function to destroy destructible objects
            Collider2D[] objectToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDestructible);

            if (objectToRemove.Length > 0)
            {
                foreach (Collider2D col in objectToRemove)
                {
                    Destroy(col.gameObject);
                }
            }

            Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDamageable);
            if (objectsToDamage.Length > 0)
            {
                foreach (Collider2D col in objectsToDamage)
                {
                    EnemyHealthController enemyHealth = col.GetComponent<EnemyHealthController>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.DamageEnemy(damageAmount);
                    }
                }
               
            }
        }
    }
}
