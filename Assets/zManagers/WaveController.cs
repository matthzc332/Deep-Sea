using UnityEngine;
using UnityEngine.UI;

public class WaveController : MonoBehaviour
{
    public float waveTimer;
    public float normalWaveTime = 90f;
    public string timerUI;

    public GameManager GameManager;

    [Header("Guardado de Datos")]
    public ShipData shipData; // Arrastra el ScriptableObject aquí

    void Start()
    {
        if (GameManager == null)
        {
            GameManager = GameManager.instance;
        }
    }

    void Update()
    {
        // ... (Tu lógica de timer se mantiene igual)
        int minutes = Mathf.FloorToInt(waveTimer / 60);
        int seconds = Mathf.FloorToInt(waveTimer % 60);
        timerUI = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (waveTimer > 0)
    {
        waveTimer -= Time.deltaTime;
    }
    // Solo entramos aquí si el tiempo se acabó Y el estado sigue siendo OnWave
    else if (waveTimer <= 0 && GameManager.instance.currentGameState == GameManager.GameState.OnWave)
    {
        waveTimer = 0;
        EndWave();
    }
}

    public void StartWave()
    {
        waveTimer = normalWaveTime;
    }

    public void EndWave()
    {
        // --- NUEVA LÓGICA DE GUARDADO ---
        if (shipData != null)
        {
            // 1. Buscar el barco en la escena para obtener su vida actual
            Ship playerShip = Object.FindFirstObjectByType<Ship>();
            if (playerShip != null)
            {
                shipData.puntosDeVida = (int)playerShip.HP;
            }

            // 2. Buscar el arma para obtener la munición actual
            Base_Gun playerGun = Object.FindFirstObjectByType<Base_Gun>();
            if (playerGun != null)
            {
                shipData.municion = playerGun.amount_ammunition;
            }

            Debug.Log("Datos guardados en ShipData al finalizar la oleada.");
        }
        // --------------------------------

        if (GameManager == null) GameManager = GameManager.instance;
        
        if (GameManager != null)
        {
            GameManager.EndWave();
        }
    }
}