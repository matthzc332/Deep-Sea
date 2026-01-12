using UnityEngine;
using UnityEngine.UI;


public class WaveController : MonoBehaviour
{
    public float waveTimer;
    public float normalWaveTime = 90f;
    public string timerUI;

    public GameManager GameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Timer text with (minutes:secconds)
        int minutes = Mathf.FloorToInt(waveTimer / 60);
        int seconds = Mathf.FloorToInt(waveTimer % 60);
        timerUI = string.Format("{0:0}:{1:00}", minutes, seconds);

        //Update Timer

        if (waveTimer > 0)
        {
            waveTimer -= Time.deltaTime;
        }

        if (waveTimer <= 0)
        {
            EndWave();
        }
    }

    public void StartWave()
    {
        waveTimer = normalWaveTime;
    }

    public void EndWave()
    {
        GameManager.EndWave();
    }
}

