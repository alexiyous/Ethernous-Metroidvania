using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    public float rangeToStartChase;
    private bool isChasing;

    public float speedTest = 1;

    public float moveSpeed, turnSpeed;

    private Transform player;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //function for a flyer tipe enemy to chase player
        if (!isChasing)
        {
            if (Vector3.Distance(transform.position, player.position) < rangeToStartChase)
            {
                isChasing = true;

                anim.SetBool("isChasing", isChasing);
            }
        } else
        {
            if (player.gameObject.activeSelf)
            {
                Vector3 direction = transform.position - player.position;//get the distance between player and enemy
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//get the angle for the enemy to face when chasing player
                Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);//identify the target rotation 

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);//rotate the enemy from the current to the player position
                                                                                                                 //according to the configurate speed every frame
                /* transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);*///start to chase the enemy
                transform.position += -transform.right * moveSpeed * Time.deltaTime;
            }
        }
    }
}
