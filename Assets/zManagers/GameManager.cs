using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Referencias de Managers")]
    public UIManager uiManager;
    public WaveController waveController;

    [Header("Persistencia de Datos")]
    public ShipData datosDelBarco;
    public Ship Player;

    [Header("Estado del Juego")]
    public GameState currentGameState = GameState.MainMenu;
    private GameState gameStateBeforePause;
    public static int difficultyLevel = 0;

    [Header("Configuración de Escena")]
    public GameObject costaIsla0;
    public int wood;
    public TMP_Text woodText;

    [Header("Efectos de Transición")]
    public Image fadeImage;
    public float fadeDuration = 2f;

    public enum GameState { MainMenu, Playing, OnWave, Pause }

    void Awake()
    {
        // Singleton Robusto
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

    private void OnEnable() => SceneManager.sceneLoaded += AlCargarEscena;
    private void OnDisable() => SceneManager.sceneLoaded -= AlCargarEscena;

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        // Re-vinculación de referencias en la nueva escena
        uiManager = FindFirstObjectByType<UIManager>();
        Player = FindFirstObjectByType<Ship>();

        if (difficultyLevel == 0)
        {
            datosDelBarco.puntosDeVida = 6;
            datosDelBarco.municion = 25;
        }

        // Cargar datos del ScriptableObject al Barco Real
        if (Player != null && datosDelBarco != null)
        {
            Player.HP = datosDelBarco.puntosDeVida;

            Cannon2 scriptCanon = Player.GetComponentInChildren<Cannon2>();
            if (scriptCanon != null)
            {
                scriptCanon.amount_ammunition = datosDelBarco.municion;
            }
            Debug.Log("<color=green>Datos cargados:</color> Vida y Munición sincronizadas.");
        }

        FadeOut();
    }

    // --- SINCRONIZACIÓN DE DATOS ---
    public void SincronizarDatosDelBarco()
    {
        if (Player == null || datosDelBarco == null) return;

        // Guardamos HP del Barco
        datosDelBarco.puntosDeVida = (int)Player.HP;

        // Guardamos munición del cañón (hijo)
        Cannon2 scriptCanon = Player.GetComponentInChildren<Cannon2>();
        if (scriptCanon != null)
        {
            datosDelBarco.municion = scriptCanon.amount_ammunition;
            Debug.Log($"<color=cyan>[Sincro]</color> Guardado: HP {Player.HP}, Balas {scriptCanon.amount_ammunition}");
        }
    }

    // --- FLUJO DE JUEGO ---
    public void StartGame()
    {
        difficultyLevel = 0;
        if (uiManager != null) uiManager.StartGame();
        StartWave();
    }

    public void StartWave()
    {
        currentGameState = GameState.OnWave;
        if (waveController != null) waveController.StartWave();
        Time.timeScale = 1f;

        if (costaIsla0 != null) costaIsla0.SetActive(false);
    }

    public void EndWave()
    {
        // Guardar progreso antes de cualquier otra cosa
        SincronizarDatosDelBarco();

        currentGameState = GameState.Playing;
        
        // Limpieza de escena
        LimpiarObjetosPorTag("Enemy");
        LimpiarObjetosPorTag("Bullet");

        difficultyLevel++;

        // Activar la isla para la fase de descanso/tienda
        if (costaIsla0 != null)
        {
            costaIsla0.SetActive(true);
            costa_isla scriptIsla = costaIsla0.GetComponent<costa_isla>();
            if (scriptIsla != null) scriptIsla.ActivarMovimiento(true);
        }
    }

    private void LimpiarObjetosPorTag(string tag)
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objetos) Destroy(obj);
    }

    // --- PAUSA Y MENÚS ---
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

    // --- TRANSICIONES ---
    public void softTransition() => StartCoroutine(SoftTransitionCoroutine());

    private IEnumerator SoftTransitionCoroutine()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            yield return Fade(0, 1);
        }

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextScene);
        else
            SceneManager.LoadScene(1);
    }

    public void FadeOut() => StartCoroutine(FadeOutCoroutine());

    private IEnumerator FadeOutCoroutine()
    {
        if (fadeImage != null)
        {
            yield return Fade(1, 0);
            fadeImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = a;
                fadeImage.color = c;
            }
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
        if (canvas != null)
        {
            RectTransform rt = fadeImage.GetComponent<RectTransform>();
            rt.SetParent(canvas.transform);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            rt.localScale = Vector3.one;
            fadeObject.transform.SetAsLastSibling();
        }
        fadeObject.SetActive(false);
    }
}