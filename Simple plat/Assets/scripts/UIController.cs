using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

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

    public Slider healthSlider;

    public Image fadeScreen;

    public float fadeSpeed = 2f;
    private bool fadingToBlack, fadingFromBlack;

    public string mainMenuScene;

    public GameObject pauseScreen;

    public GameObject fullScreenMap;

    void Start()
    {
        //UpdateHealth(PlayerHealthController.instance.currentHealth, PlayerHealthController.instance.maxHealth);//at the start of the frame, will instantly set the value
    }

   

    // Update is called once per frame
    void Update()
    {
        //when fadingToBlack is true, adjust the alpha(the seen through parameter) to full 1, so the black screen starts to appear
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1f)
            {
                fadingToBlack = false;
            }

        } else if (fadingFromBlack)//when fadingFromBlack is true, adjust the alpha(the seen through parameter) to full 0, so the black screen start to fade to the original game screen
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
            {
                fadingFromBlack = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))//basiclly you choose which key on keyboard you wanted to set as an input to execute an
                                             //action, in this case its the escape key.
        {
            PauseUnpause();
        }
    }

    //function for updating the slider with current health
    public void UpdateHealth (int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void StartFadeToBlack()//function to start black screen when move to another scene
    {
        fadingToBlack = true;
        fadingFromBlack = false;
    }

    public void StartFadeFromBlack()//function recover from black screen to game screen
    {
        fadingFromBlack = true;
        fadingToBlack = false;
    }

    public void PauseUnpause()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);//set active the pause screen if it haven't been activated

            Time.timeScale = 0f;//stop the frame flow of time
        } else
        {
            pauseScreen.SetActive(false);//unactive the pause screen if its already been activated

            Time.timeScale = 1f;//continue the frame flow of time
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; //continue the frame so it doesnt freezes the screen if creating another new game in main menu

        Destroy(PlayerHealthController.instance.gameObject);//destroy the game object (player) so it doesnt spwan on the main menu
        PlayerHealthController.instance = null;//this is crucial in order to free up some spaces ini unity memory

        Destroy(RespawnController.instance.gameObject);//destroy the game object (Respawn poiny) so it doesnt spwan on the main menu
        RespawnController.instance = null;//this is crucial in order to free up some spaces ini unity memory

        Destroy(MapController.instance.gameObject);//destroy the game object (pink map tiles) so it doesnt spwan on the main menu
        MapController.instance = null;//this is crucial in order to free up some spaces ini unity memory

        instance = null;//destroy the game object so it doesnt spwan on the main menu
        Destroy(gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }
}
