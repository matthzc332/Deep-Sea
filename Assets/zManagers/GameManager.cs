using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UIManager uiManager;
    public WaveController waveController;

    public GameObject costaIsla0;

    public GameState currentGameState = GameState.MainMenu;
    public GameState gameStateBeforePause;

    public int wood;
    public TMP_Text woodText;

    public Image fadeImage;
    public float fadeDuration = 2f;

    public enum GameState
    {
        MainMenu,
        Playing,
        OnWave,
        Pause
    }

    void Awake()
    {
        Time.timeScale = 0f;

        if (fadeImage == null)
            CreateFadeImage();
    }

    void Start()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(false);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        woodText.text = wood.ToString();
    }

    private void CreateFadeImage()
    {
        GameObject fadeObject = new GameObject("FadeImage");
        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = Color.black;

        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.SetParent(GetComponentInChildren<Canvas>().transform);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;

        fadeObject.transform.SetAsLastSibling();
        fadeObject.SetActive(false);
    }

    public void StartGame()
    {
        uiManager.StartGame();
        StartWave();
    }

    public void StartWave()
    {
        currentGameState = GameState.OnWave;
        waveController.StartWave();

        uiManager.CloseSkillTree(); // CORRECTO

        Time.timeScale = 1f;

        if (costaIsla0 != null)
            costaIsla0.SetActive(false);
    }

    public void EndWave()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigos)
            Destroy(enemigo);

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
            Destroy(bullet);

        if (costaIsla0 != null)
            costaIsla0.SetActive(true);
    }

    public void Pause()
    {
        gameStateBeforePause = currentGameState;
        uiManager.PauseGame();
        Time.timeScale = 0f;
        currentGameState = GameState.Pause;
    }

    public void ResumeGame()
    {
        uiManager.ResumeGame();
        Time.timeScale = 1f;
        currentGameState = gameStateBeforePause;
    }

    public void softTransition()
    {
        StartCoroutine(SoftTransitionCoroutine());
    }

    private IEnumerator SoftTransitionCoroutine()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            Color c = Color.black;
            c.a = 0f;
            fadeImage.color = c;
        }

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            if (fadeImage != null)
            {
                Color cc = fadeImage.color;
                cc.a = t / fadeDuration;
                fadeImage.color = cc;
            }

            yield return null;
        }

        if (fadeImage != null)
        {
            Color finalColor = fadeImage.color;
            finalColor.a = 1f;
            fadeImage.color = finalColor;
        }

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int next = currentScene + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene(0);
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            Color c = Color.black;
            c.a = 1f;
            fadeImage.color = c;
        }

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            if (fadeImage != null)
            {
                Color cc = fadeImage.color;
                cc.a = 1f - (t / fadeDuration);
                fadeImage.color = cc;
            }

            yield return null;
        }

        if (fadeImage != null)
        {
            Color finalColor = fadeImage.color;
            finalColor.a = 0f;
            fadeImage.color = finalColor;
            fadeImage.gameObject.SetActive(false);
        }
    }
}
