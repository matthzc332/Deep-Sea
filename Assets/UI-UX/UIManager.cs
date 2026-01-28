using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Datos del Barco")]
    public ShipData datosDelBarco; // Arrastra aquí tu ScriptableObject

    [Header("Menus")]
    public GameObject MainMenu;
    public GameObject PauseMenu;
    public GameObject UpgradesMenu;
    public GameObject UIRoot;
    public GameObject IslandMenu;

    [Header("Botón de pausa")]
    public Button PauseButton;

    public GameManager GameManagerScript;
    public WaveController WaveController;
    public RectTransform transformIsland;

    public TMP_Text waveTimerText;

    private List<Button> allButtons = new List<Button>();
    string escenaActual;

    private void Awake()
    {
        DOTween.Init();

        // --- LIMPIEZA DE SHIP DATA ---
        if (datosDelBarco != null)
        {
            datosDelBarco.posicion1 = null;
            datosDelBarco.posicion2 = null;
            Debug.Log("<color=green>UIManager:</color> Posiciones del ShipData reseteadas a NULL.");
        }
        else
        {
            Debug.LogWarning("UIManager: No se ha asignado el ScriptableObject ShipData.");
        }
        // -----------------------------

#if UNITY_EDITOR
        UnityEngine.Object debugCanvas = GameObject.Find("Debug Canvas");
        if (debugCanvas != null)
            GameObject.DestroyImmediate(debugCanvas);
#endif
        Button[] buttonsInScene = FindObjectsByType<Button>(FindObjectsSortMode.None);
        allButtons.AddRange(buttonsInScene);
    }

    void Start()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (waveTimerText != null && WaveController != null)
        {
            waveTimerText.text = WaveController.timerUI;
        }

        escenaActual = SceneManager.GetActiveScene().name;
        if (escenaActual == "ISLA")
        {
            // Nota: Es mejor guardar esta referencia una sola vez, no en Update
            if (transformIsland == null && IslandMenu != null)
                transformIsland = IslandMenu.GetComponent<RectTransform>();
        }
    }

    // ... resto de tus métodos (StartGame, PauseGame, etc.)
    public void StartGame()
    {
        Time.timeScale = 1f;
        MainMenu.SetActive(false);
        if (waveTimerText != null)
        {
            waveTimerText.gameObject.SetActive(true);
        }
    }

    public void PauseGame() => PauseMenu.SetActive(true);
    public void ResumeGame() => PauseMenu.SetActive(false);
    
    public void OpenMainMenu()
    {
        if (waveTimerText != null) waveTimerText.gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void CloseGame() => Application.Quit();
    public void StartNextWave() => SceneManager.LoadScene(0);
}