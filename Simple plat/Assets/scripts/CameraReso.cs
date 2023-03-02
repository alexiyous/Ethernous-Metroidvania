using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReso : MonoBehaviour
{
    public bool maintainWidth = true;
    [Range(-1, 1)]
    public int adaptPosition;
    float defaultWeidth, defaultHeight;

    Vector3 CameraPos;
    void Awake()
    {
        Debug.Log(Camera.main.aspect);
        Debug.Log(Camera.main.orthographicSize);
        CameraPos = Camera.main.transform.position;
        defaultHeight = Camera.main.orthographicSize;
        defaultWeidth = Camera.main.orthographicSize * Camera.main.aspect;
    }
    void Update()
    {
        if (maintainWidth)
        {
            Camera.main.orthographicSize = defaultWeidth / Camera.main.aspect;
            Camera.main.transform.position = new Vector3(CameraPos.x, adaptPosition * (defaultHeight - Camera.main.orthographicSize), CameraPos.z);
        }
        else
        {
            Camera.main.transform.position = new Vector3(adaptPosition * (defaultWeidth - Camera.main.orthographicSize * Camera.main.aspect), CameraPos.y, CameraPos.z);
        }
    }
}

