using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float timeToExplode;
    public GameObject explosion;

    public float blastRange;
    public LayerMask whatIsDestructible;
    public LayerMask whatIsEnemy;

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

              DamageEnemy();

              //function to destroy destructible objects
              Collider2D[] objectToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDestructible);

              if (objectToRemove.Length > 0)
              {
                  foreach(Collider2D col in objectToRemove)
                  {
                      Destroy(col.gameObject);
                  }
              }
            AudioManager.instance.PlaySFXAdjusted(4);//play enemy explode SFX
        }
    }

    //function to damage enemy using bomb
    void DamageEnemy()
    {
        Collider2D[] enemyToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsEnemy);

        if (enemyToRemove.Length > 0)
        {
            foreach (Collider2D enmy in enemyToRemove)
            {
                enmy.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
            }
            /*PlayerHealthController.instance.DamagePlayer(bombToPlayerDamage);*/
        }
    }
}
