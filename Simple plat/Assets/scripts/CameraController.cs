using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private PlayerController player;
    public BoxCollider2D boundsbox; //creating some sort of a box so the camera wont see beyond that box while
                                    //following the player

    private float halfHeight, halfWidth;

    // Start is called before the first frame update
    void Start()
    {

        //camera follows the player at the beginning of the game
        player = FindObjectOfType<PlayerController>();
        //variables to set into the clamping box x,y
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        AudioManager.instance.PlayLevelMusic();
        
    }

    // Update is called once per frame
    void Update()
    {
        //settings so the camera will always follows the player
        if (player != null)
        {
            transform.position = new Vector3(
                Mathf.Clamp (player.transform.position.x, boundsbox.bounds.min.x + halfWidth, boundsbox.bounds.max.x - halfWidth),
                Mathf.Clamp(player.transform.position.y, boundsbox.bounds.min.y + halfHeight, boundsbox.bounds.max.y - halfHeight), 
                transform.position.z);
        } else
        {
            player = FindObjectOfType<PlayerController>();
        }
    }
}
