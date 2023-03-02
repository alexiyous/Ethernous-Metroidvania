using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUnlock : MonoBehaviour
{
    public bool unlockDoubleJump, unlockDash, unlockBecomeBall, unlockDropBomb;
    public GameObject pickupEffect;

    public string unlockMessege;
    public TMP_Text unlockText;

    public string abilityRef;

    private void Start()
    {
        if (PlayerPrefs.HasKey(abilityRef))
        {
            if (PlayerPrefs.GetInt(abilityRef) == 1)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //unlock the ability
        if (other.tag == "Player")
        {
            PlayerAbillityTracker player = other.GetComponentInParent<PlayerAbillityTracker>();

            if (unlockDoubleJump)
            {
                player.canDoubleJump = true;

                PlayerPrefs.SetInt("DoubleJumpUnlocked", 1);//store integer value as an exchange for boolean, in this case it stores the abillity that the player could double jump (1 == true);
            }

            if (unlockDash)
            {
                player.canDash = true;

                PlayerPrefs.SetInt("DashUnlocked", 1);//store integer value as an exchange for boolean, in this case it stores the abillity that the player could dash (1 == true);
            }

            if (unlockBecomeBall)
            {
                player.canBecomeBall = true;

                PlayerPrefs.SetInt("BallUnlocked", 1);//store integer value as an exchange for boolean, in this case it stores the abillity that the player could become ball (1 == true);
            }

            if (unlockDropBomb)
            {
                player.canDropBomb = true;

                PlayerPrefs.SetInt("BombUnlocked", 1);//store integer value as an exchange for boolean, in this case it stores the abillity that the player could drop bomb (1 == true);
            }

            Instantiate(pickupEffect, transform.position, transform.rotation);//pickup effect animation

            unlockText.transform.parent.SetParent(null);//makes the child parentless so that when
                                                        //Destroy(gameObject) is called, it wont destroy the Text too
            unlockText.transform.parent.position = transform.position;

            unlockText.text = unlockMessege;
            unlockText.gameObject.SetActive(true);

            Destroy(unlockText.transform.parent.gameObject, 3f);//destroy the text after certain amount of time

            Destroy(gameObject);

            PlayerPrefs.SetInt(abilityRef, 1);

            AudioManager.instance.PlaySFX(5);//play pickup SFX
        }
    }
}
