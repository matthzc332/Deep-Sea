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

    public GameManager GameManagerScript;
    public WaveController WaveController;
    public RectTransform transformIsland;

    public TMP_Text waveTimerText;

    private List<Button> allButtons = new List<Button>();

    string escenaActual;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }

    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
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
            transformIsland.DOAnchorPosX(124f, 2f).SetEase(Ease.InOutCubic).SetUpdate(true).OnComplete(() =>
            {
                EnableAllButtons();
            });
        }
    }

    public void GoAstillero()
    {
        if (escenaActual == "ISLA")
        {
            DisableAllButtons();
            Debug.Log("GoTavern");
            transformIsland.DOAnchorPosX(-1492f, 2f).SetEase(Ease.InOutCubic).SetUpdate(true).OnComplete(() =>
            {
                EnableAllButtons();
            });
        }
    }

    public void GoBackToMenu()
    {
        if (escenaActual == "ISLA")
        {
            DisableAllButtons();
            transformIsland.DOAnchorPosX(-683f, 2f).SetEase(Ease.InOutCubic).SetUpdate(true).OnComplete(() =>
            {
                EnableAllButtons();
            });
        }
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
            if (btn != null) btn.interactable = false;
    }

    public void EnableAllButtons()
    {
        foreach (Button btn in allButtons)
            if (btn != null) btn.interactable = true;
    } 
}
