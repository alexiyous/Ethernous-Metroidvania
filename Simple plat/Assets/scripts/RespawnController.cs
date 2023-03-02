using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);//it tells unity to not destroy the gameobject that this script attached to everytime
                                          //it loads a scene.
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private Vector3 respawnPoint;
    public float waitToRespawn;

    private GameObject thePlayer;

    public GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = PlayerHealthController.instance.gameObject;//get the player health controller as a reference for this respawn script

        respawnPoint = thePlayer.transform.position;//save the player position as a respawn point
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpawn(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());//basicly StartCoroutine means that unity will execute a program
                                    //alternatively together or you could say parallel(new timeline branch)
    }

    IEnumerator RespawnCo()
    {
        thePlayer.SetActive(false);//the player is nonactive;
        if (deathEffect != null)
        {
            Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation);
        }

        yield return new WaitForSeconds(waitToRespawn);//wait for a couple of seconds

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//load the previously scene

        thePlayer.transform.position = respawnPoint;//set the player position according to the respawn point
        thePlayer.SetActive(true);//activate the player

        PlayerHealthController.instance.FillHealth();//fill the previously health
    }
}
