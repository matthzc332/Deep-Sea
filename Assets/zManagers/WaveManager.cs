using System.Collections.Generic; // Necesario para usar Listas si fuera el caso
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Referencias Externas")]
    // Arrastra aquí el objeto "WaveSpawner" de tu jerarquía
    public Wave_Spawner waveSpawnerScript;

    [Header("Referencias Hijos (Controladores)")]
    // Arrastra aquí los hijos WaveController y BossWaveController
    public GameObject waveControllerObj;
    public GameObject bossWaveControllerObj;

    // NOTA: Borré la variable 'gameManager' porque al ser estático el dato, no necesitamos la referencia.

    private void Start()
    {
        // Llamamos a la validación directamente al iniciar
        ValidateAndSelectMode();
    }

    private void ValidateAndSelectMode()
    {
        int currentLevelIndex = GameManager.difficultyLevel;

        if (waveSpawnerScript == null)
        {
            Debug.LogError("Error: Falta asignar el WaveSpawner en el inspector del WaveManager.");
            return;
        }

        // CAMBIO 1: Usar .Length en lugar de .Count (y chequeamos null primero)
        if (waveSpawnerScript.gameLevels == null || waveSpawnerScript.gameLevels.Length == 0)
        {
            Debug.LogError("Error: La lista 'gameLevels' en el WaveSpawner está vacía o es nula.");
            return;
        }

        // CAMBIO 2: Usar .Length aquí también
        if (currentLevelIndex >= 0 && currentLevelIndex < waveSpawnerScript.gameLevels.Length)
        {
            Wave_ScriptableObject currentData = waveSpawnerScript.gameLevels[currentLevelIndex];

            if (currentData == null)
            {
                Debug.LogError($"Error: El elemento {currentLevelIndex} de la lista gameLevels está vacío (null).");
                return;
            }

            if (currentData.isBossWave)
            {
                Debug.Log($"Nivel {currentLevelIndex}: MODO JEFE ACTIVADO.");
                bossWaveControllerObj.SetActive(true);
                waveControllerObj.SetActive(false);
            }
            else
            {
                Debug.Log($"Nivel {currentLevelIndex}: MODO OLEADA NORMAL.");
                bossWaveControllerObj.SetActive(false);
                waveControllerObj.SetActive(true);
            }
        }
        else
        {
            // CAMBIO 3: .Length aquí también para el mensaje de error
            Debug.LogError($"El nivel actual ({currentLevelIndex}) está fuera del rango. Tamaño lista: {waveSpawnerScript.gameLevels.Length}");
        }
    }
}