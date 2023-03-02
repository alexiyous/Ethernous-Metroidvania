using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset.z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerHealthController.instance.transform.position + offset;
    }
}
