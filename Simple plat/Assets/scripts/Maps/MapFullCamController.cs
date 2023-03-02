using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFullCamController : MonoBehaviour
{
    public MapCameraController mapCam;

    public float zoomSpeed;
    private float startSize;
    public float maxZoom, minZoom;
    public float moveModifier;

    private Camera cam;

    private bool canMoveMap;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        startSize = cam.orthographicSize; // get the size of camera as the start size
    }

    // Update is called once per frame
    void Update()
    {
        if (UIController.instance.pauseScreen.activeInHierarchy)
        {
            canMoveMap = false;
        }
        else
        {
            canMoveMap = true;
        }

        if (canMoveMap)
        {
            //move left-rigth and up-down when accesing fullmap also the speed of view move depends on the size of camera;
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized * cam.orthographicSize * Time.unscaledDeltaTime * moveModifier;

            if (Input.GetKey(KeyCode.E)) //zoom in;
            {
                cam.orthographicSize -= zoomSpeed * Time.unscaledDeltaTime; //the time still could continued ignoring the timescale that is paused (Time.unscaledDeltaTime
            }

            if (Input.GetKey(KeyCode.Q)) //zoom out;
            {
                cam.orthographicSize += zoomSpeed * Time.unscaledDeltaTime; //the time still could continued ignoring the timescale that is paused (Time.unscaledDeltaTime
            }

            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom); //limit the zoom in and zoom out with certain bound.
        }
    }

    private void OnEnable() //if an object is activated for the first time, it will read as onEnable
    {
        transform.position = mapCam.transform.position; //set the fullmap camera position, as the transform position of MapCameraController
    }
}
