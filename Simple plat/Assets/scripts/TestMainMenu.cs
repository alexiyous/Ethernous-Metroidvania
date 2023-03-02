using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TestMainMenu : MonoBehaviour
{
    public Image fadeImage;
    public Image ContButtonImage;
    public TMP_Text ContButtonText;
    public Image playButtonImage;
    public Image exitButtonImage2;
    public TMP_Text playButtonText;
    public TMP_Text exitButtonText;
    public Button playButton;
    public Button exitButton;
    public float fadeDuration;
    public string sceneName;

    public GameObject continueButton;

    public PlayerAbillityTracker player;

    private void Start()
    {
        AudioManager.instance.PlayMainMenuMusic();
        if (PlayerPrefs.HasKey("ContinueLevel"))//if there are values stored, then we activated the continue button
        {
            if (!PlayerPrefs.HasKey("Dark"))
            {
                continueButton.SetActive(true);
            } else if (PlayerPrefs.HasKey("Dark"))
            {
                continueButton.SetActive(false);

                Destroy(PlayerHealthController.instance.gameObject);//destroy the game object (Health bar) so it doesnt spwan on the main menu
                PlayerHealthController.instance = null;//this is crucial in order to free up some spaces ini unity memory

                Destroy(RespawnController.instance.gameObject);//destroy the game object (Player) so it doesnt spwan on the main menu
                RespawnController.instance = null;//this is crucial in order to free up some spaces ini unity memory

                Destroy(MapController.instance.gameObject);//destroy the game object (pink map tiles) so it doesnt spwan on the main menu
                MapController.instance = null;//this is crucial in order to free up some spaces ini unity memory

                Destroy(UIController.instance.gameObject);//destroy the game object (UI Canvas) so it doesnt spwan on the main menu
                UIController.instance = null;//this is crucial in order to free up some spaces ini unity memory
            }
        }
        /*playButton.onClick.AddListener(StartFade);*/
    }

    public void StartFade()
    {
        PlayerPrefs.DeleteAll();

        StartCoroutine(FadeInOut(fadeImage, fadeDuration, () =>
        {
            SceneManager.LoadScene(sceneName);
        }));
    }

    private IEnumerator FadeInOut(Image img, float duration, System.Action onComplete)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            img.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            ContButtonImage.color = new Color(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t));
            playButtonImage.color = new Color(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t));
            /*exitButtonImage1.color = new Color(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t));*/
            exitButtonImage2.color = new Color(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t));
            ContButtonText.color = new Color(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t));
            playButtonText.color = new Color(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t));
            exitButtonText.color = new Color(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t));
            yield return null;
        }

        onComplete?.Invoke();
    }

    public void Continue()
    {
        player.gameObject.SetActive(true);
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));

        if (PlayerPrefs.HasKey("DoubleJumpUnlocked"))//check if there is a value stored in "DoubleJumpUnlocked"
        {
            if (PlayerPrefs.GetInt("DoubleJumpUnlocked") == 1)//check if the value in "DoubleJumpUnlocked" == 1
            {
                player.canDoubleJump = true;//unlock abilty canDoubleJump
            }
        }

        if (PlayerPrefs.HasKey("DashUnlocked"))//check if there is a value stored in "DashUnlocked"
        {
            if (PlayerPrefs.GetInt("DashUnlocked") == 1)//check if the value in "DashUnlocked" == 1
            {
                player.canDash = true;//unlock abilty canDash
            }
        }

        if (PlayerPrefs.HasKey("BallUnlocked"))//check if there is a value stored in "BallUnlocked"
        {
            if (PlayerPrefs.GetInt("BallUnlocked") == 1)//check if the value in "BallUnlocked" == 1
            {
                player.canBecomeBall = true;//unlock abilty canBecomeBall
            }
        }

        if (PlayerPrefs.HasKey("BombUnlocked"))//check if there is a value stored in "BombUnlocked"
        {
            if (PlayerPrefs.GetInt("BombUnlocked") == 1)//check if the value in "BombUnlocked" == 1
            {
                player.canDropBomb = true;//unlock abilty canDropBomb
            }
        }

        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Game Quit");
    }
}