using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float moveSpeed;

    public Rigidbody2D theRB;

    public int damageAmount;
    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = transform.position - PlayerHealthController.instance.transform.position;//get the distance between player and bullet
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//get the angle for the bullet to face when chasing player
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);//identify the target rotation 

        AudioManager.instance.PlaySFXAdjusted(2);//play boss shot SFX
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = -transform.right * moveSpeed;//move towards the last position of the player
    }

    //function for damaging the player
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer(damageAmount);
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        AudioManager.instance.PlaySFXAdjusted(3);//play bullet impact SFX
    }
}
