using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PauseMenu;
    public GameObject UpgradesMenu;
    public GameObject UIRoot;
    public GameObject IslandMenu;

    [Header("Shops")]
    public GameObject NPCShop;
    public GameObject OpenNPCShopButton;
    public GameObject CloseNPCShopButton;

    public GameObject TreeShop;
    public GameObject OpenTreehopButton;
    public GameObject CloseTreeShopButton;

    [Header("Botón de pausa")]
    public Button PauseButton;

    public GameManager GameManagerScript;
    public WaveController WaveController;
    public RectTransform transformIsland;

    public TMP_Text waveTimerText;

    private List<Button> allButtons = new List<Button>();

    string escenaActual;

    private RectTransform npcShopRect;
    private RectTransform treeShopRect;

    private float npcShopOriginalY;
    private float treeShopOriginalY;

    private const float shopOpenY = -200f;
    private const float shopAnimTime = 0.25f;

    private bool shopIsOpen = false;

    private void Awake()
    {
        DOTween.Init();
#if UNITY_EDITOR
        UnityEngine.Object debugCanvas = GameObject.Find("Debug Canvas");
        if (debugCanvas != null)
            GameObject.DestroyImmediate(debugCanvas);
#endif
        Button[] buttonsInScene = FindObjectsByType<Button>(FindObjectsSortMode.None);
        allButtons.AddRange(buttonsInScene);

        // Cachear RectTransforms de las tiendas
        if (NPCShop != null)
        {
            npcShopRect = NPCShop.GetComponent<RectTransform>();
            if (npcShopRect != null)
                npcShopOriginalY = npcShopRect.anchoredPosition.y; // debería ser 200
        }

        if (TreeShop != null)
        {
            treeShopRect = TreeShop.GetComponent<RectTransform>();
            if (treeShopRect != null)
                treeShopOriginalY = treeShopRect.anchoredPosition.y; // también 200 o lo que sea
        }
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
            transformIsland = IslandMenu.GetComponent<RectTransform>();
        }
    }

    //Start Game
    public void StartGame()
    {
        Time.timeScale = 1f;
        MainMenu.SetActive(false);
        if (waveTimerText != null)
        {
            waveTimerText.gameObject.SetActive(true);
        }
    }

    //-------------------------------------------------------------
    //   MENUS
    //-------------------------------------------------------------
    public void PauseGame()
    {
        PauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
    }
    public void OpenMainMenu()
    {
        if (waveTimerText != null)
        {
            waveTimerText.gameObject.SetActive(false);
        }
        MainMenu.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void StartNextWave()
    {
        SceneManager.LoadScene(0);
    }

    //-------------------------------------------------------------
    //   BUTTONS MANAGER
    //-------------------------------------------------------------

    public void DisableAllButtons()
    {
        foreach (Button btn in allButtons)
        {
            if (btn == null) continue;

            if (shopIsOpen)
            {
                // Si hay tienda abierta: solo permitir
                // - Botón cerrar NPCShop
                // - Botón cerrar TreeShop
                // - Botón de pausa

                bool esBotonCerrarNPC = (CloseNPCShopButton != null && btn.gameObject == CloseNPCShopButton);
                bool esBotonCerrarTree = (CloseTreeShopButton != null && btn.gameObject == CloseTreeShopButton);
                bool esBotonPausa = (PauseButton != null && btn == PauseButton);

                if (esBotonCerrarNPC || esBotonCerrarTree || esBotonPausa)
                    btn.interactable = true;
                else
                    btn.interactable = false;
            }
            else
            {
                // Si no hay tienda abierta, se comporta como antes: todo desactivado
                btn.interactable = false;
            }
        }
    }

    public void EnableAllButtons()
    {
        shopIsOpen = false; // ya no hay tienda abierta

        foreach (Button btn in allButtons)
        {
            if (btn != null)
                btn.interactable = true;
        }
    }
}