using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint;

    public float moveSpeed, waitAtPoints;
    private float waitCounter;

    public float jumpForce;

    public Rigidbody2D theRB;
    public Animator anim;

    public Transform groundPoint;
    private bool IsOnGround;
    public LayerMask WhatIsGround;

    /*public GameObject player;*/
    private Transform playerPosition;
    public float distance;
    public float chaseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = PlayerHealthController.instance.transform;

        waitCounter = waitAtPoints;

        foreach(Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //function to chase the player if within the distance
        if (Mathf.Abs(Vector3.Distance(transform.position, playerPosition.position)) < distance)
        {
            IsOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, WhatIsGround);

            if (transform.position.x < playerPosition.position.x)
            {
                theRB.velocity = new Vector2(chaseSpeed, theRB.velocity.y);//move to the right of player Position
                transform.localScale = new Vector3(-1F, 1F, 1F);
            } else
            {
                theRB.velocity = new Vector2(-chaseSpeed, theRB.velocity.y);//move to the left of player position
                transform.localScale = Vector3.one;
            }

            if ((transform.position.y < playerPosition.position.y - .5f && theRB.velocity.y < .1f) && IsOnGround)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }
            
        }
        else if (Mathf.Abs(Vector3.Distance(transform.position, playerPosition.position)) > distance)
        {
                //function so that the enemy walks to the destinated patrol point
                if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > .2F)//if the distance between the enemy with the patrol point is greater than 0.2f
                {
                    IsOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, WhatIsGround);

                    if (transform.position.x < patrolPoints[currentPoint].position.x)
                    {
                        theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);//move to the right of patrol point
                        transform.localScale = new Vector3(-1F, 1F, 1F);
                    }
                    else
                    {
                        theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);//move to the left of patrol point
                        transform.localScale = Vector3.one;
                    }

                    //the enemy jumps
                    if ((transform.position.y < patrolPoints[currentPoint].position.y - .5f && theRB.velocity.y < .1f) && IsOnGround)
                    {
                        theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                    }

                }
                else //everytime the enemy reaches the patrol point it stops for a few seconds
                {
                    theRB.velocity = new Vector2(0f, theRB.velocity.y);

                    waitCounter -= Time.deltaTime;
                    if (waitCounter <= 0)
                    {
                        waitCounter = waitAtPoints;

                        currentPoint++;
                        if (currentPoint >= patrolPoints.Length)
                        {
                            currentPoint = 0;
                        }
                    }

                }
             
        }
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }
}
