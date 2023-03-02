using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D theRB;

    public float moveSpeed;
    public float jumpForce;

    public Transform groundPoint;
    private bool IsOnGround;
    public LayerMask WhatIsGround;

    public Animator anim;

    public BulletController shotToFire;
    public Transform shotPoint;

    private bool canDoubleJump;

    public float dashSpeed, dashTime;
    private float dashCounter;

    public SpriteRenderer theSR, afterImage;
    public float afterImageLifetime, timeBetweenAfterImages;
    private float afterImageCounter;
    public Color afterImageColor;

    public float waitAfterDashing;
    private float dashRechargeCounter;

    public GameObject standing, ball;
    private float waitToBall;
    public float ballCounter;

    public Animator ballAnim;

    public Transform bombPoint;
    public GameObject bomb;

    private PlayerAbillityTracker abilities;

    public float waitAfterBombing;
    private float bombCounter;

    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        //to track the abilites that are unlocked
        abilities = GetComponent<PlayerAbillityTracker>();

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && Time.timeScale != 0f) //the character will only take inputs if the frame doesnt stop and canMove is true.
        {

            if (dashRechargeCounter > 0)
            {
                dashRechargeCounter -= Time.deltaTime;
            }
            else
            {

                //function for dash mechanism
                if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash)//if we press the right button mouse it could dash
                                                                                             //as long its on standing mode
                {
                    dashCounter = dashTime;

                    ShowAfterImage();

                    AudioManager.instance.PlaySFXAdjusted(7);//play player dash SFX
                }
            }

            if (dashCounter > 0)
            {
                dashCounter = dashCounter - Time.deltaTime;

                theRB.velocity = new Vector2(dashSpeed * transform.localScale.x, transform.localScale.y);

                afterImageCounter -= Time.deltaTime;//to show after image every second of the dash time
                if (afterImageCounter <= 0)
                {
                    ShowAfterImage();
                }
                dashRechargeCounter = waitAfterDashing;
            }
            else
            {

                //move sideways
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);

                //checking if on the ground
                IsOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, WhatIsGround);

                //handle the direction change
                if (theRB.velocity.x < 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else if (theRB.velocity.x > 0)
                {
                    transform.localScale = Vector3.one;
                }
            }

            //jumping
            if (Input.GetButtonDown("Jump") && (IsOnGround || (canDoubleJump && abilities.canDoubleJump)))
            //the function so that player could double jump
            //if either IsOnGround is true or canDoubleJump is true
            {
                if (IsOnGround) //if its on the ground it could jump once
                                //and set canDoubleJump (unlock double jump when on the ground) as true
                {
                    canDoubleJump = true;
                    AudioManager.instance.PlaySFXAdjusted(12);//play jump SFX
                }
                else //since now IsOnGround is False but the canDoubleJumps is true,
                     //it could jump once again
                {
                    canDoubleJump = false;
                    //setting the animation of double jump when mid-air
                    anim.SetTrigger("doubleJump");

                    AudioManager.instance.PlaySFXAdjusted(9);//play double jump SFX
                }

                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }

            //shoot using the left click mouse
            if (Input.GetButtonDown("Fire1"))//if we press the left button mouse it could shoot
                                             //as long its on standing mode
            {
                if (standing.activeSelf)
                {
                    //the bullet will shot through the shotPoint it also make the bullet could shot where ever the player's facing (left or right)
                    Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDIR = new Vector2(transform.localScale.x, 0f);

                    //play the shooting animation every time it shot a bullet
                    anim.SetTrigger("shotFired");

                    AudioManager.instance.PlaySFXAdjusted(14);//play player shot SFX
                }
            }

            if (ball.activeSelf && abilities.canDropBomb) //if we press the left button mouse while its on ballmode,
                                                          //it will shoot bomb
            {
                if (bombCounter <= 0)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Instantiate(bomb, bombPoint.position, bombPoint.rotation);
                        AudioManager.instance.PlaySFXAdjusted(13);//play palyer mine SFX
                        bombCounter = waitAfterBombing;
                    }
                }
                else if (bombCounter > 0)
                {
                    bombCounter -= Time.deltaTime;
                }

            }


            //ball mode
            if (!ball.activeSelf)
            {
                if (Input.GetAxisRaw("Vertical") < -.9F && abilities.canBecomeBall)//if we hold the down arrow of keyboard it will turn into ball
                {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0)
                    {
                        ball.SetActive(true);
                        standing.SetActive(false);

                        AudioManager.instance.PlaySFX(6);//play turning into ball SFX
                    }
                }
                else
                {
                    ballCounter = waitToBall;
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > .9F)
                {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0)
                    {
                        ball.SetActive(false);
                        standing.SetActive(true);

                        AudioManager.instance.PlaySFX(10);//play from ball to player SFX
                    }
                }
                else
                {
                    ballCounter = waitToBall;
                }
            }
        } else
        {
            theRB.velocity = Vector2.zero;//if canMove is not true, freeze the position of the player
        }

        //setting animation standing if standing
        if (standing.activeSelf)
        {
            anim.SetBool("IsOnGround", IsOnGround); //jumping
            anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x)); //moving animation
        }
        //setting animation ball if balll
        if (ball.activeSelf)
        {
            ballAnim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }
    }

    public void ShowAfterImage()//function to show after image
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifetime);

        afterImageCounter = timeBetweenAfterImages;
    }
}
