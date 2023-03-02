using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapActivator : MonoBehaviour
{
    public string mapToActivate;

    // Start is called before the first frame update
    void Start()
    {
        MapController.instance.ActivateMap(mapToActivate); //Activate map that wanted to be activate;
    }
}
