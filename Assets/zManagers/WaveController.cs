using UnityEngine;
using UnityEngine.UI;

public class WaveController : MonoBehaviour
{
    public float waveTimer;
    public float normalWaveTime = 90f;
    public string timerUI;

    public GameManager gameManager; // Cambiado a minúscula por convención de C#

    [Header("Guardado de Datos")]
    public ShipData shipData;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.instance;
        }
    }

    void Update()
    {
        // --- CORRECCIÓN CLAVE ---
        // Solo ejecuta la lógica del timer si el juego está efectivamente en "OnWave"
        // Si está en "Countdown" o "Pause", este código se salta.
        if (gameManager == null || gameManager.currentGameState != GameManager.GameState.OnWave)
        {
            return;
        }

        if (waveTimer > 0)
        {
            waveTimer -= Time.deltaTime;
        }
        else
        {
            waveTimer = 0;
            EndWave();
        }

        // Formateo del texto para la UI
        int minutes = Mathf.FloorToInt(waveTimer / 60);
        int seconds = Mathf.FloorToInt(waveTimer % 60);
        timerUI = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void StartWave()
    {
        // Aquí solo seteamos el tiempo. 
        // El Update empezará a descontar cuando el GameManager cambie el estado a OnWave
        waveTimer = normalWaveTime;
    }

    public void EndWave()
    {
        if (shipData != null)
        {
            Ship playerShip = Object.FindFirstObjectByType<Ship>();
            if (playerShip != null)
            {
                shipData.puntosDeVida = (int)playerShip.HP;
            }

            Base_Gun playerGun = Object.FindFirstObjectByType<Base_Gun>();
            if (playerGun != null)
            {
                shipData.municion = playerGun.amount_ammunition;
            }

            Debug.Log("Datos guardados en ShipData al finalizar la oleada.");
        }

        if (gameManager == null) gameManager = GameManager.instance;

        if (gameManager != null)
        {
            gameManager.EndWave();
        }
    }
}