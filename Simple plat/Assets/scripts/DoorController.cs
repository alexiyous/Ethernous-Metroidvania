using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public Animator anim;

    public float distanceToOpen;

    private PlayerController thePlayer;

    private bool playerExiting;

    public Transform exitPoint;
    public float movePlayerSpeed;

    public string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = PlayerHealthController.instance.GetComponent<PlayerController>();//get access to the playerController script
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, thePlayer.transform.position) < distanceToOpen)
        {
            anim.SetBool("doorOpen", true);
        } else
        {
            anim.SetBool("doorOpen", false);
        }

        if (playerExiting)
        {
            thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position, exitPoint.position, movePlayerSpeed * Time.deltaTime);
            //move the player position to the exit point
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!playerExiting)
            {
                thePlayer.canMove = false;//player can't move when the coroutine start

                StartCoroutine(UseDoorCo());
            }
        }
    }

    IEnumerator UseDoorCo()
    {
        playerExiting = true;//means player exiting through the portal

        thePlayer.anim.enabled = false;//freezes the sprite (smooth effect)

        UIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        RespawnController.instance.SetSpawn(exitPoint.position);//set the checkpoint to the exitPoint position 
        thePlayer.canMove = true;//player can move again
        thePlayer.anim.enabled = true;//unfreeze the sprite of player

        UIController.instance.StartFadeFromBlack();

        PlayerPrefs.SetString("ContinueLevel", levelToLoad);//Stores player preferences between game sessions with the name "ContinueLevel", in this case the scene that is going to be loaded
        PlayerPrefs.SetFloat("PosX", exitPoint.position.x);//Stores player preferences between game sessions with the name "PosX", in this case the Player's x position in recent game session
        PlayerPrefs.SetFloat("PosY", exitPoint.position.y);//Stores player preferences between game sessions with the name "PosY", in this case the Player's y position in recent game session
        PlayerPrefs.SetFloat("PosZ", exitPoint.position.z);//Stores player preferences between game sessions with the name "PosZ", in this case the Player's z position in recent game session

        SceneManager.LoadScene(levelToLoad);
    }
}
