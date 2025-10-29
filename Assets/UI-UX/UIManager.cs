using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PauseMenu;
    public GameObject UpgradesMenu;
    public GameObject UIRoot;

    public GameManager GameManagerScript;
    public WaveController WaveController;

    public TMP_Text waveTimerText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        #if UNITY_EDITOR
            UnityEngine.Object debugCanvas = GameObject.Find("Debug Canvas");
            if (debugCanvas != null)
                GameObject.DestroyImmediate(debugCanvas);   
        #endif
    }

    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        waveTimerText.text = WaveController.timerUI;
    }

    //Start Game
    public void StartGame()
    {
        Time.timeScale = 1f;
        MainMenu.SetActive(false);
        waveTimerText.gameObject.SetActive(true);
    }

    //Pause and Resume Menu
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
        UIRoot.SetActive(true);
        waveTimerText.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }
    public void CloseSkillTreeButton()
    {
        UIRoot.SetActive(false);
        waveTimerText.gameObject.SetActive(true);
    }

    public void OpenMainMenu()
    {
        waveTimerText.gameObject.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
