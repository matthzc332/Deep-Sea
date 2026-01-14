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

    [Header("Referencias Generales")]
    public UIManager uiManager;
    public WaveController waveController;
    public GameObject costaIsla0; // La isla que se mueve al final
    public TMP_Text woodText; // Referencia al texto de madera
    public int wood;

    [Header("Configuración del Barco (Player)")]
    public GameObject boatPrefab;
    public PlayerScripteable datosBarco; // Referencia al Scriptable Object
    public Vector3 spawnPosition = Vector3.zero;
    public string gameSceneName = "prefabs"; // Nombre de la escena de juego

    [Header("Configuración del Boss")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    [Header("Configuración Visual (Fade)")]
    public Image fadeImage;
    public float fadeDuration = 2f;

    [Header("Estado del Juego")]
    public GameState currentGameState = GameState.MainMenu;
    public GameState gameStateBeforePause;
    public int difficultyLevel = 0;

    public enum GameState
    {
        MainMenu,
        Playing,
        OnWave,
        Pause
    }

    void Awake()
    {
        // SINGLETON ROBUSTO
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

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

    // Esta función se ejecuta cada vez que cambia la escena
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Buscar referencias necesarias en la nueva escena
        uiManager = FindFirstObjectByType<UIManager>();

        // Si el texto de madera no está asignado, intentar buscarlo
        if (woodText == null)
        {
            GameObject woodObj = GameObject.Find("WoodText"); // Asegúrate del nombre en Unity
            if (woodObj != null) woodText = woodObj.GetComponent<TMP_Text>();
        }

        // 2. Hacer Fade In (aclarar pantalla)
        FadeOut();

        // 3. Si estamos en la escena del juego, Spawneamos el barco
        if (scene.name == gameSceneName)
        {
            CheckAndSpawnBoat();
        }
    }

    // --- LÓGICA DEL BARCO (Del Script A) ---
    void CheckAndSpawnBoat()
    {
        Ship existingBoat = FindObjectOfType<Ship>();

        if (existingBoat == null)
        {
            Debug.Log("GameManager: No se encontró barco. Instanciando uno nuevo...");
            if (boatPrefab != null)
            {
                GameObject nuevoBarco = Instantiate(boatPrefab, spawnPosition, Quaternion.identity);
                Ship scriptBarco = nuevoBarco.GetComponent<Ship>();

                // Inyectamos el ScriptableObject y la vida
                if (scriptBarco != null && datosBarco != null)
                {
                    scriptBarco.datosBarco = datosBarco;
                    scriptBarco.HP = datosBarco.Vida;
                    scriptBarco.vida = datosBarco.Vida;

                    Debug.Log("GameManager: Barco creado con " + datosBarco.Vida + " de vida del ScriptableObject.");
                }
            }
            else
            {
                Debug.LogError("GameManager: ¡Falta asignar 'boatPrefab'!");
            }
        }
        else
        {
            Debug.Log("GameManager: Ya existe un barco. Actualizando datos...");
            if (datosBarco != null)
            {
                existingBoat.datosBarco = datosBarco;
            }
        }
    }

    void Start()
    {
        // Inicializar Fade
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(false);
        }

        // Comprobación inicial por si arrancamos directo en la escena de juego
        if (SceneManager.GetActiveScene().name == gameSceneName)
        {
            CheckAndSpawnBoat();
        }
    }

    void Update() { }

    // --- LÓGICA DE JUEGO (Del Script B) ---

    public void StartGame()
    {
        if (uiManager != null) uiManager.StartGame();
        StartWave();
    }

    public void StartWave()
    {
        currentGameState = GameState.OnWave;
        if (waveController != null) waveController.StartWave();

        Time.timeScale = 1f;

        // Ocultar la isla durante la oleada
        if (costaIsla0 != null)
            costaIsla0.SetActive(false);

        // Iniciar Boss (ajustar el tiempo o condición si es necesario)
        StartCoroutine(SpawnBossDelayed(5f));
    }

    public void EndWave()
    {
        currentGameState = GameState.Playing;
        Debug.Log("Oleada terminada. Limpiando escena...");

        // Limpiar Enemigos y Balas
        foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy")) Destroy(obj);
        foreach (var obj in GameObject.FindGameObjectsWithTag("Bullet")) Destroy(obj);

        // Aumentar Dificultad
        difficultyLevel++;
        Debug.Log("Dificultad aumentada a: " + difficultyLevel);

        // Gestionar Isla (Final del nivel)
        if (costaIsla0 != null)
        {
            costaIsla0.SetActive(true);

            costa_isla scriptIsla = costaIsla0.GetComponent<costa_isla>();
            if (scriptIsla != null)
            {
                scriptIsla.ActivarMovimiento(true);
                Debug.Log("Isla moviéndose hacia el barco.");
            }
        }
    }

    private IEnumerator SpawnBossDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bossPrefab != null)
        {
            Vector3 spawnPos = bossSpawnPoint != null ? bossSpawnPoint.position : new Vector3(-10f, 0f, 0f);
            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            Debug.Log("¡El Boss ha entrado a la batalla!");
        }
    }

    // --- LÓGICA DE UI Y FADE ---

    private void CreateFadeImage()
    {
        GameObject fadeObject = new GameObject("FadeImage");
        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = false;

        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();

        if (GetComponentInChildren<Canvas>() != null)
            rectTransform.SetParent(GetComponentInChildren<Canvas>().transform);
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

    public void softTransition()
    {
        StartCoroutine(SoftTransitionCoroutine());
    }

    private IEnumerator SoftTransitionCoroutine()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0);
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }
            yield return null;
        }

        if (fadeImage != null) fadeImage.color = Color.black;

        // Cambio de escena
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else
            SceneManager.LoadScene(1); // Volver al menú o primera escena jugable
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
            fadeImage.color = Color.black;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }
            yield return null;
        }

        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
            fadeImage.gameObject.SetActive(false);
        }
    }

    // --- PAUSA Y RESUME ---

    public void Pause()
    {
        gameStateBeforePause = currentGameState;
        if (uiManager != null) uiManager.PauseGame();
        Time.timeScale = 0f;
        currentGameState = GameState.Pause;
    }

    public void ResumeGame()
    {
        if (uiManager != null) uiManager.ResumeGame();
        Time.timeScale = 1f;
        currentGameState = gameStateBeforePause;
    }

    public void GoIsland() { }
}