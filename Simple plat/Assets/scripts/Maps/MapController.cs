using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public GameObject[] maps;

    public GameObject fullMapCam;

    private bool canFullmap;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject map in maps) //if player continued the game from the main menu, this method will check every maps and if there are maps that
                                         //already been unlocked, it will spawn and continued with the maps that've been unlocked
        {
            if (PlayerPrefs.GetInt("Map_" + map.name) == 1)
            {
                map.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UIController.instance.pauseScreen.activeInHierarchy)
        {
            canFullmap = false;
        }
        else
        {
            canFullmap = true;

            if ((UIController.instance.fullScreenMap.activeInHierarchy) && (!UIController.instance.pauseScreen.activeInHierarchy))
            {
                Time.timeScale = 0f;
            }

            if (Input.GetKeyDown(KeyCode.M) && canFullmap) //if we press M keyboard, int will show the full map
            {
                if (!UIController.instance.fullScreenMap.activeInHierarchy) //if the fullScreenMap is not active in hierarchy
                {
                    UIController.instance.fullScreenMap.SetActive(true); //set active the fullScreenMap
                    Time.timeScale = 0f; //stop player from moving while accesing the full map
                    fullMapCam.SetActive(true); //set active the cam for the fullscreen map;
                }
                else
                {
                    UIController.instance.fullScreenMap.SetActive(false); //set nonactive the fullScreenMap
                    Time.timeScale = 1f; //continue the player after fullmap is nonactived
                    fullMapCam.SetActive(false); //set nonactive the cam for the fullscreen map;
                }
            }
        }
    }

    public void ActivateMap(string mapToActivate)//method to activate minimap
    {
        foreach (GameObject map in maps) //for each maps that are made, if the name is same as mapToActivate, then set active the gameObject.
        {
            if (map.name == mapToActivate)
            {
                map.SetActive(true);
                PlayerPrefs.SetInt("Map_" + mapToActivate, 1); //save the current minimap unlocked progress
            }
        }
    }
}
