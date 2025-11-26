using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;
using DG.Tweening;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
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

    public void SpawnSkillTreeButton()
    {
        if (waveTimerText != null)
        {
            waveTimerText.gameObject.SetActive(false);
        }
        Time.timeScale = 0f;
    }

    public void CloseSkillTreeButton()
    {
        if (waveTimerText != null)
        {
            waveTimerText.gameObject.SetActive(true);
        }
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

    //-------------------------------------------------------------
    //   ISLAND UI
    //-------------------------------------------------------------

    public void GoTavern()
    {
        if (escenaActual == "ISLA")
        {
            DisableAllButtons();
            Debug.Log("GoTavern");
            transformIsland
                .DOAnchorPosX(124f, 1f)
                .SetEase(Ease.InOutCubic)
                .SetUpdate(true)
                .OnComplete(() => { EnableAllButtons(); });
        }
    }

    public void GoAstillero()
    {
        if (escenaActual == "ISLA")
        {
            DisableAllButtons();
            Debug.Log("GoAstillero");
            transformIsland
                .DOAnchorPosX(-1492f, 1f)
                .SetEase(Ease.InOutCubic)
                .SetUpdate(true)
                .OnComplete(() => { EnableAllButtons(); });
        }
    }

    public void GoBackToMenu()
    {
        if (escenaActual == "ISLA")
        {
            DisableAllButtons();
            transformIsland
                .DOAnchorPosX(-683f, 1f)
                .SetEase(Ease.InOutCubic)
                .SetUpdate(true)
                .OnComplete(() => { EnableAllButtons(); });
        }
    }

    //-------------------------------------------------------------
    //   TAVERN / SHOPS MANAGER
    //-------------------------------------------------------------

    // ---- NPC SHOP ----
    public void OpenNPCShop()
    {
        Debug.Log("OpenNPCShop1");
        if (escenaActual != "ISLA" || npcShopRect == null)
            return;

        Debug.Log("OpenNPCShop2");

        shopIsOpen = true;      // marcamos que hay una tienda abierta
        DisableAllButtons();    // esto dejará solo pausa + cerrar activos

        // Cambiar botones
        if (OpenNPCShopButton != null) OpenNPCShopButton.SetActive(false);
        if (CloseNPCShopButton != null) CloseNPCShopButton.SetActive(true);

        // Bajar la tienda
        npcShopRect
            .DOAnchorPosY(shopOpenY, shopAnimTime)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // NO habilitamos todo, queremos seguir en modo "solo cerrar + pausa"
            });
    }

    public void CloseNPCShop()
    {
        if (escenaActual != "ISLA" || npcShopRect == null)
            return;

        DisableAllButtons(); // mientras anima, no tocamos otros botones

        // Cambiar botones
        if (CloseNPCShopButton != null) CloseNPCShopButton.SetActive(false);
        if (OpenNPCShopButton != null) OpenNPCShopButton.SetActive(true);

        // Volver a su posición original
        npcShopRect
            .DOAnchorPosY(npcShopOriginalY, shopAnimTime)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                EnableAllButtons(); // vuelve todo a la normalidad
            });
    }

    // ---- TREE SHOP ----
    public void OpenTreeShop()
    {
        Debug.Log("OpenTreeShop1");
        if (escenaActual != "ISLA" || treeShopRect == null)
            return;

        Debug.Log("OpenTreeShop2");

        shopIsOpen = true;
        DisableAllButtons();    // solo pausa + cerrar

        if (OpenTreehopButton != null) OpenTreehopButton.SetActive(false);
        if (CloseTreeShopButton != null) CloseTreeShopButton.SetActive(true);

        treeShopRect
            .DOAnchorPosY(shopOpenY, shopAnimTime)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // igual que en NPC, no habilitamos todos los botones todavía
            });
    }

    public void CloseTreeShop()
    {
        if (escenaActual != "ISLA" || treeShopRect == null)
            return;

        DisableAllButtons();

        if (CloseTreeShopButton != null) CloseTreeShopButton.SetActive(false);
        if (OpenTreehopButton != null) OpenTreehopButton.SetActive(true);

        treeShopRect
            .DOAnchorPosY(treeShopOriginalY, shopAnimTime)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                EnableAllButtons();
            });
    }

    public void StartNextWave()
    {
        //Funcion preparada para comenzar la siguiente oleada, deberia llamar una funcion del GameManager.
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
