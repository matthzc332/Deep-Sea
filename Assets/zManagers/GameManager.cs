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

    // Add this line here:
    [Header("Difficulty Settings")]
    public int difficultyLevel = 1;

    [Header("Boat Settings")]
    public GameObject boatPrefab;

    public PlayerScripteable datosBarco;

    public string gameSceneName = "prefabs";
    public Vector3 spawnPosition = Vector3.zero;

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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Time.timeScale = 0f;

        if (fadeImage == null) CreateFadeImage();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameSceneName)
        {
            CheckAndSpawnBoat();
        }
    }

    void CheckAndSpawnBoat()
    {
        // Buscamos cualquier objeto en la escena que tenga el script "Ship"
        Ship existingBoat = FindObjectOfType<Ship>();

        if (existingBoat == null)
        {
            Debug.Log("No se encontró ningún objeto con el script Ship. Instanciando uno nuevo...");
            if (boatPrefab != null)
            {
                // Instanciamos y guardamos la referencia en una variable
                GameObject nuevoBarco = Instantiate(boatPrefab, spawnPosition, Quaternion.identity);

                // Obtenemos el script del barco recién creado
                Ship scriptBarco = nuevoBarco.GetComponent<Ship>();

                // Si tiene el script y tenemos los datos, los inyectamos manualmente
                if (scriptBarco != null && datosBarco != null)
                {
                    // IMPORTANTE: Asegúrate de que en Ship.cs la variable 'datosBarco' 
                    // ahora sea de tipo 'PlayerScripteable' también.
                    scriptBarco.datosBarco = datosBarco;

                    scriptBarco.HP = datosBarco.Vida;    // Le ponemos la vida del SO
                    scriptBarco.vida = datosBarco.Vida;  // Actualizamos la variable visual

                    Debug.Log("GameManager: Barco creado e inicializado con " + datosBarco.Vida + " de vida.");
                }
            }
            else
            {
                Debug.LogError("¡El 'boatPrefab' no está asignado en el GameManager!");
            }
        }
        else
        {
            Debug.Log("Ya existe un barco con el script Ship. No se creará otro.");

            // Opcional: Si ya existe, nos aseguramos que tenga los datos correctos también
            if (datosBarco != null)
            {
                existingBoat.datosBarco = datosBarco;
            }
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

        if (SceneManager.GetActiveScene().name == gameSceneName)
        {
            CheckAndSpawnBoat();
        }
    }

    // ... (El resto del código sigue igual: Update, CreateFadeImage, StartGame, EndWave, etc.)
    void Update() { }

    private void CreateFadeImage()
    {
        GameObject fadeObject = new GameObject("FadeImage");
        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = Color.black;

        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();

        if (GetComponentInChildren<Canvas>() != null)
        {
            rectTransform.SetParent(GetComponentInChildren<Canvas>().transform);
        }
        else
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null) rectTransform.SetParent(canvas.transform);
        }

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
        Time.timeScale = 1f;

        if (costaIsla0 != null)
            costaIsla0.SetActive(false);
    }

    public void EndWave()
    {
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

        if (costaIsla0 != null)
            costaIsla0.SetActive(true);
    }

    public void GoIsland() { }

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