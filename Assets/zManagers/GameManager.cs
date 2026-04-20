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

    [Header("UI de Conteo")]
    public TMP_Text countdownText; // Arrastra aquí el texto para el 3,2,1

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
        Pause,
        Countdown // Nuevo estado para evitar disparar cosas durante el conteo
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
        Time.timeScale = 1f;

        if (fadeImage == null) CreateFadeImage();
    }

    private void OnEnable() => SceneManager.sceneLoaded += AlCargarEscena;
    private void OnDisable() => SceneManager.sceneLoaded -= AlCargarEscena;

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        uiManager = FindFirstObjectByType<UIManager>();

        if (difficultyLevel == 0)
        {
            ResetearDatosPersistentes();
        }

        FadeOut();
    }

    private void ResetearDatosPersistentes()
    {
        if (playerShipData != null)
        {
            playerShipData.puntosDeVida = 6;
            playerShipData.municion = 30;
            playerShipData.dinero = 60;
            playerShipData.balasGastadas = 0;
            playerShipData.score = 0;
            Debug.Log("GameManager: ShipData reseteado.");
        }

        if (skillStatus != null)
        {
            skillStatus.ResetearProgreso();
            Debug.Log("GameManager: Habilidades reseteadas.");
        }
    }

    void Start()
    {
        // Inicializamos el fade
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
            
        }

        // Si estamos en la escena de juego al empezar, lanzamos el conteo
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            StartCoroutine(CountdownCoroutine());
        }
    }

    // --- LÓGICA DE CONTEO Y OLEADA ---

    public void StartGame()
    {
        difficultyLevel = 0;
        if (uiManager != null) uiManager.StartGame();
        StartWave(); // Esto ahora activará el conteo
    }

    public void StartWave()
    {
        StopAllCoroutines(); // Evita bugs si se llama dos veces
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        currentGameState = GameState.Countdown;
        Time.timeScale = 1f;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);

            int count = 3;
            while (count > 0)
            {
                countdownText.text = count.ToString();
                // Efecto visual simple: un pequeño "punch" de escala
                countdownText.transform.localScale = Vector3.one * 1.5f;
                count--;
                yield return new WaitForSeconds(1f);
            }

            countdownText.text = "0";
            yield return new WaitForSeconds(0.5f);
            countdownText.gameObject.SetActive(false);
            fadeImage.gameObject.SetActive(false);
        }

        RealStartWaveLogic();
    }

    private void RealStartWaveLogic()
    {
        currentGameState = GameState.OnWave;

        if (waveController != null)
            waveController.StartWave(); // Aquí inicia el timer de la oleada y spawn

        if (uiManager != null)
        {
            uiManager.MainMenu.SetActive(false);
            if (uiManager.waveTimerText != null)
                uiManager.waveTimerText.gameObject.SetActive(true);
        }

        if (costaIsla0 != null)
            costaIsla0.SetActive(false);
    }

    // --- TRANSICIONES Y OTROS MÉTODOS ---

    public void EndWave()
    {
        currentGameState = GameState.Playing;

        // 1. Buscamos todas las entidades
        Entity[] todasLasEntidades = FindObjectsByType<Entity>(FindObjectsSortMode.None);

        foreach (Entity e in todasLasEntidades)
        {
            // CORRECCIÓN: Solo activamos la retirada si el objeto tiene el tag "Enemy"
            // Esto evita que el Barco (Player) sea afectado si comparte el script Entity
            if (e.CompareTag("Enemy"))
            {
                e.StartRetreat(2);
            }
        }

        // 2. Limpieza de proyectiles
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets) Destroy(bullet);

        difficultyLevel++;

        // 3. Activación de la costa/isla
        if (costaIsla0 != null)
        {
            costaIsla0.SetActive(true);
            costa_isla scriptIsla = costaIsla0.GetComponent<costa_isla>();
            if (scriptIsla != null)
            {
                scriptIsla.ActivarMovimiento(true);
                Debug.Log("Barco a salvo. Iniciando aproximación a la isla.");
            }
        }
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

    // Fades y Escenas
    public void softTransition() => StartCoroutine(SoftTransitionCoroutine());

    private IEnumerator SoftTransitionCoroutine()
    {
        yield return StartCoroutine(FadeRoutine(0f, 1f));

        int nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        if (nextScene == 0) nextScene = 1; // Evitar volver al Main Menu si no quieres

        SceneManager.LoadScene(nextScene);
    }

    public void FadeOut() => StartCoroutine(FadeOutCoroutine());

    private IEnumerator FadeOutCoroutine()
    {
        yield return StartCoroutine(FadeRoutine(1f, 0f));
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha)
    {
        if (fadeImage == null) yield break;
        fadeImage.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            Color c = fadeImage.color;
            c.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }

    private void CreateFadeImage()
    {
        GameObject fadeObject = new GameObject("FadeImage");
        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = false;

        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas == null) return;

        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.SetParent(canvas.transform);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        fadeObject.transform.SetAsLastSibling();
        fadeObject.SetActive(false);
    }

    public void CalcularPuntajeFinOleada()
    {
        Ship player = FindFirstObjectByType<Ship>();
        if (player == null || player.shipData == null) return;

        ShipData data = player.shipData;
        long puntajeFinalOleada = (data.score * data.puntosDeVida) + data.dinero + data.balasGastadas;

        Debug.Log($"TOTAL FINAL OLEADA: {puntajeFinalOleada}");
    }
}