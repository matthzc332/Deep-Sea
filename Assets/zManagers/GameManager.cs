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

    // Agregar referencia al objeto que quieres activar
    public GameObject costaIsla0;

    public GameState currentGameState = GameState.MainMenu;
    public GameState gameStateBeforePause;

    public int wood;
    public TMP_Text woodText;

    // Referencia para el fade (solo fondo negro)
    public Image fadeImage;
    public float fadeDuration = 2f;

    //Maneja al Boss
    public GameObject bossPrefab;
    public Transform bossSpawnPoint; 
    // hasta aqui

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

        // Crear fade image si no existe
        if (fadeImage == null)
        {
            CreateFadeImage();
        }
    }

    void Start()
    {
        // Asegurarse de que el fade est� transparente al inicio
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f; // Completamente transparente
            fadeImage.color = color;
            fadeImage.gameObject.SetActive(false);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

    }

    // Funci�n para crear el fade image si no existe
    private void CreateFadeImage()
    {
        GameObject fadeObject = new GameObject("FadeImage");
        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = Color.black; // Fondo negro

        fadeImage.raycastTarget = false;

        // Hacer que ocupe toda la pantalla
        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.SetParent(GetComponentInChildren<Canvas>().transform);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;

        // Establecer el orden en la jerarqu�a para que est� encima de todo
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

        // Opcional: Desactivar el objeto al empezar la wave
        if (costaIsla0 != null)
            costaIsla0.SetActive(false);

        // INICIA LA APARICIÓN DEL BOSS
        StartCoroutine(SpawnBossDelayed(5f));
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

        // Activar el objeto Costa_Isla_0
        if (costaIsla0 != null)
            costaIsla0.SetActive(true);
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

    // Nueva funci�n de transici�n suave - Versi�n simplificada
    public void softTransition()
    {
        StartCoroutine(SoftTransitionCoroutine());
    }

    // Corrutina que solo maneja el fade del fondo negro
    private IEnumerator SoftTransitionCoroutine()
    {
        // Activar la imagen de fade (fondo negro)
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            // Configurar color negro con alpha 0 (completamente transparente)
            Color startColor = Color.black;
            startColor.a = 0f;
            fadeImage.color = startColor;
        }

        Debug.Log("Iniciando fade a negro...");

        // Fade in: aumentar gradualmente el alpha de 0 a 1
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = alpha; // Solo modificamos el canal alpha
                fadeImage.color = color;
            }

            yield return null;
        }

        // Asegurar que est� completamente opaco (alpha = 1)
        if (fadeImage != null)
        {
            Color finalColor = fadeImage.color;
            finalColor.a = 1f; // Negro completamente opaco
            fadeImage.color = finalColor;
        }

        Debug.Log("Fade completado. Cambiando de escena...");

        // Cambiar de escena despu�s del fade
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (nextSceneIndex < totalScenes)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No hay m�s escenas. Volviendo al men� principal.");
            SceneManager.LoadScene(1);
        }
    }

    // Funci�n opcional para hacer fade out (volver a transparente)
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            // Comenzar con alpha 1 (completamente opaco)
            Color startColor = Color.black;
            startColor.a = 1f;
            fadeImage.color = startColor;
        }

        Debug.Log("Iniciando fade out...");

        // Fade out: disminuir gradualmente el alpha de 1 a 0
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);

            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = alpha; // Reducir el alpha gradualmente
                fadeImage.color = color;
            }

            yield return null;
        }

        // Asegurar que est� completamente transparente (alpha = 0)
        if (fadeImage != null)
        {
            Color finalColor = fadeImage.color;
            finalColor.a = 0f;
            fadeImage.color = finalColor;
            fadeImage.gameObject.SetActive(false);
        }

        Debug.Log("Fade out completado");
    }

    // Boss spawn
    private IEnumerator SpawnBossDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bossPrefab != null)
        {
            // Aparece en la posición del spawn point o en una coordenada fija
            Vector3 spawnPos = bossSpawnPoint != null ? bossSpawnPoint.position : new Vector3(-10f, 0f, 0f);
            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            Debug.Log("¡El Boss ha entrado a la batalla!");
        }
    }

}