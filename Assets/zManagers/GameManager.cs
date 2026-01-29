using JetBrains.Annotations;
using NUnit.Framework;
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

    [Header("Configuración de Datos")]
    public ShipData playerShipData;
    public WeaponSkillStatus skillStatus;

    public enum GameState
    {
        MainMenu,
        Playing,
        OnWave,
        Pause
    }

    public static int difficultyLevel = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 

        Time.timeScale = 0f;

        if (fadeImage == null)
        {
            CreateFadeImage();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        uiManager = FindFirstObjectByType<UIManager>();

        // --- CORRECCIÓN DE RESETEO ---
        // Si la dificultad es 0, reseteamos los ScriptableObjects automáticamente
        if (difficultyLevel == 0)
        {
            ResetearDatosPersistentes();
        }

        FadeOut();
    }

    // Método privado para no ensuciar AlCargarEscena
    private void ResetearDatosPersistentes()
    {
        if (playerShipData != null)
        {
            playerShipData.puntosDeVida = 6;
            playerShipData.municion = 25;
            playerShipData.dinero = 60;
            
            Debug.Log("GameManager: Datos de ShipData reseteados (Dificultad 0).");
        }

        if (skillStatus != null)
        {
            skillStatus.ResetearProgreso();
            Debug.Log("GameManager: Lista de habilidades limpiada.");
        }
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
        // DontDestroyOnLoad ya está en Awake, se puede omitir aquí
    }

    // Este es el método que debes llamar desde tu botón de "Jugar" en el menú
    public void StartGame()
    {
        difficultyLevel = 0; // Al ponerlo en 0, AlCargarEscena se encargará del resto
        
        if(uiManager != null) uiManager.StartGame();
        StartWave();
    }

    public void StartWave()
    {
        currentGameState = GameState.OnWave;
        waveController.StartWave();
        Time.timeScale = 1f;

        if (costaIsla0 != null)
            costaIsla0.SetActive(false);
    }

    void Update()
    {
    }

    private void CreateFadeImage()
    {
        GameObject fadeObject = new GameObject("FadeImage");
        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = Color.black; 
        fadeImage.raycastTarget = false;

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

    public void EndWave()
    {
        currentGameState = GameState.Playing;
        Debug.Log("Oleada terminada. Limpiando escena...");

        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigos)
        {
            Destroy(enemigo);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        difficultyLevel++;
        Debug.Log("Dificultad aumentada a: " + difficultyLevel);

        if (costaIsla0 != null)
        {
            costaIsla0.SetActive(true); 
            costa_isla scriptIsla = costaIsla0.GetComponent<costa_isla>();
            if (scriptIsla != null)
            {
                scriptIsla.ActivarMovimiento(true); 
                Debug.Log("Iniciando movimiento de la isla hacia el barco.");
            }
        }
    }

    public void GoIsland()
    {
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
            Color startColor = Color.black;
            startColor.a = 0f;
            fadeImage.color = startColor;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = alpha; 
                fadeImage.color = color;
            }
            yield return null;
        }

        if (fadeImage != null)
        {
            Color finalColor = fadeImage.color;
            finalColor.a = 1f;
            fadeImage.color = finalColor;
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (nextSceneIndex < totalScenes)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
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
            Color startColor = Color.black;
            startColor.a = 1f;
            fadeImage.color = startColor;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);

            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = alpha; 
                fadeImage.color = color;
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