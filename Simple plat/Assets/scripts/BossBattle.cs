using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    private CameraController theCam;
    public Transform camPosition;
    public float camSpeed;

    public int threshold1, threshold2;
    public float activeTime, fadeoutTime, inactiveTime;
    private float activeCounter, fadeCounter, inactiveCounter;

    public Transform[] spawnPoints;
    private Transform targetPoint;
    public float moveSpeed;

    public Animator anim;

    public Transform theBoss;

    public float timeBetweenShots1, timeBetweenShots2;
    private float shotCounter;
    public GameObject bullet;
    public Transform shotPoint;

    public GameObject WinObjects;

    private bool battleEnded;

    public string bossRef;

    public GameObject escalator;
    public GameObject gate;
    // Start is called before the first frame update
    void Start()
    {
        theCam = FindObjectOfType<CameraController>();//the camera will search its current position
        theCam.enabled = false;//then disabled the script component so it doesnt follow the player anymore

        activeCounter = activeTime;

        shotCounter = timeBetweenShots1;

        AudioManager.instance.PlayBossMusic();
    }

    // Update is called once per frame
    void Update()
    {
        //move towards to its fixed position for boss battle
        theCam.transform.position = Vector3.MoveTowards(theCam.transform.position, camPosition.position, camSpeed * Time.deltaTime);
        //if the battle is not ended, then it will run all the mechanics of the boss
        if (!battleEnded) {
            if (BossHealthController.instance.currentHealth > threshold1)//phase 1 of the Boss
            {
                if (activeCounter > 0)
                {
                    activeCounter -= Time.deltaTime;
                    if (activeCounter <= 0)//if activeCounter reaches 0, it will make the boss fade or vanish for a certain amount of time
                    {
                        fadeCounter = fadeoutTime;
                        anim.SetTrigger("Vanish");
                    }
                    //bullet function within phase 1
                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShots1;

                        Instantiate(bullet, shotPoint.position, Quaternion.identity);
                    }
                }
                else if (fadeCounter > 0)
                {
                    fadeCounter -= Time.deltaTime;
                    if (fadeCounter <= 0)//if fadeCounter reaches 0, it will deactivates the boss so it will disapppear
                    {
                        theBoss.gameObject.SetActive(false);
                        inactiveCounter = inactiveTime;
                    }
                }
                else if (inactiveCounter > 0)
                {
                    inactiveCounter -= Time.deltaTime;
                    if (inactiveCounter <= 0)//after it was deactivated for a certain amount of time, it will activates the boss again.
                    {
                        theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;//the spawn positions are generated randomly from 0 - 11 (it will never spawn at the last position, after the first spawn) 
                        theBoss.gameObject.SetActive(true);

                        activeCounter = activeTime;

                        shotCounter = timeBetweenShots1;
                    }
                }
            }
            else
            {
                if (targetPoint == null)//1
                {
                    targetPoint = theBoss;
                    fadeCounter = fadeoutTime;
                    anim.SetTrigger("Vanish");
                }
                else
                {
                    if (Vector3.Distance(theBoss.position, targetPoint.position) > .02f)//4
                    {
                        theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);

                        if (Vector3.Distance(theBoss.position, targetPoint.position) <= .02f)//if the distance between the boss and target point are close enough, it will make the boss vanish
                        {
                            fadeCounter = fadeoutTime;
                            anim.SetTrigger("Vanish");
                        }

                        //bullet function within phase 2
                        shotCounter -= Time.deltaTime;
                        if (shotCounter <= 0)
                        {
                            if (BossHealthController.instance.currentHealth > threshold2)
                            {
                                shotCounter = timeBetweenShots1;
                            }
                            else if (BossHealthController.instance.currentHealth <= threshold2)//if the boss health below the threshold, it will shoot more faster
                            {
                                shotCounter = timeBetweenShots2;
                            }

                            Instantiate(bullet, shotPoint.position, Quaternion.identity);
                        }
                    }
                    else if (fadeCounter > 0)//2
                    {
                        fadeCounter -= Time.deltaTime;
                        if (fadeCounter <= 0)//if fadeCounter reaches 0, it will deactivates the boss so it will disapppear
                        {
                            theBoss.gameObject.SetActive(false);
                            inactiveCounter = inactiveTime;
                        }
                    }
                    else if (inactiveCounter > 0)//3
                    {
                        inactiveCounter -= Time.deltaTime;
                        if (inactiveCounter <= 0)//after it was deactivated for a certain amount of time, it will activates the boss again.
                        {
                            theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;//the spawn positions are generated randomly from 0 - 11 (it will never spawn at the last position, after the first spawn) 

                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                            int whileBreaker = 0;

                            while (targetPoint.position == theBoss.position && whileBreaker < 100)
                            {
                                targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                                whileBreaker++;
                            }

                            theBoss.gameObject.SetActive(true);

                            if (BossHealthController.instance.currentHealth > threshold2)
                            {
                                shotCounter = timeBetweenShots1;
                            }
                            else if (BossHealthController.instance.currentHealth <= threshold2)//if the boss health below the threshold, it will shoot more faster
                            {
                                shotCounter = timeBetweenShots2;
                            }
                        }
                    }
                }
            }
        } else
        {
            fadeCounter -= Time.deltaTime;
            if (fadeCounter <= 0)
            {
                if (WinObjects != null)//make the platform and pickup appear
                {
                    WinObjects.SetActive(true);
                    WinObjects.transform.SetParent(null);
                }

                theCam.enabled = true;//the cam follows the player again

                gameObject.SetActive(false);//the boss dissapeared

                AudioManager.instance.PlayLevelMusic();

                PlayerPrefs.SetInt(bossRef, 1);//stored the condition for the boss later for bossActivator
            }
        }
    }

    public void EndBattle()//function means the boss health is zero and the battle ends
    {
        battleEnded = true;

        escalator.SetActive(false);
        gate.SetActive(false);

        fadeCounter = fadeoutTime;
        anim.SetTrigger("Vanish");//the boss vanish
        theBoss.GetComponent<Collider2D>().enabled = false;//no collider anymore

        BossBullet[] bullets = FindObjectsOfType<BossBullet>(); //find all bullets
        if (bullets.Length > 0)//destory all bullets
        {
            foreach (BossBullet bb in bullets)
            {
                Destroy(bb.gameObject);
            }
        }
    }
}
