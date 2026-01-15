using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave_Spawner : MonoBehaviour
{
    // Elemento 0: Solo Globos
    // Elemento 1: Globo + Gaviota
    public Wave_ScriptableObject[] gameLevels;

    [SerializeField] private Transform[] spawnpoints;

    private float timeBtwnSpawns;

    // Variable para recordar qué configuración estamos usando
    private Wave_ScriptableObject currentConfig;

    private void Start()
    {
        if (gameLevels == null || gameLevels.Length == 0)
        {
            Debug.LogError(" ERROR: Asigna los Game Levels en el Inspector del Spawner.");
            return;
        }

        // Inicializar el primer spawn
        UpdateWaveConfig();
        timeBtwnSpawns = Time.time + currentConfig.TimeBeforeThisWave;
    }

    private void Update()
    {
        if (GameManager.instance == null) return;

        // Si no estamos en oleada, no hacemos nada
        if (GameManager.instance.currentGameState != GameManager.GameState.OnWave) return;

        // --- CLAVE DEL ÉXITO: ---
        // Antes de spawnear, nos aseguramos de tener la configuración de la dificultad actual
        UpdateWaveConfig();

        if (Time.time >= timeBtwnSpawns)
        {
            SpawnWave();
            // Reiniciar contador usando el tiempo del nivel actual
            timeBtwnSpawns = Time.time + currentConfig.TimeBeforeThisWave;
        }
    }

    // Esta función selecciona el archivo correcto según la dificultad del GameManager
    private void UpdateWaveConfig()
    {
        int currentDifficulty = GameManager.difficultyLevel;

        // Protección: Si la dificultad es mayor que los niveles que tenemos, usamos el último
        if (currentDifficulty >= gameLevels.Length)
        {
            currentDifficulty = gameLevels.Length - 1;
        }

        // Asignamos la configuración actual
        currentConfig = gameLevels[currentDifficulty];
    }

    private void SpawnWave()
    {
        if (spawnpoints == null || spawnpoints.Length == 0) return;

        // Fórmula: Enemigos base + Dificultad actual (para que sean más cada vez)
        //int extraEnemies = GameManager.instance.difficultyLevel;
        //float totalToSpawn = currentConfig.NumberToSpawn + extraEnemies;
        int extraEnemies = GameManager.difficultyLevel;
        int currentDifficulty = GameManager.difficultyLevel;

        //for (int i = 0; i < totalToSpawn; i++)
        {
            if (currentConfig.EnemiesInWave != null && currentConfig.EnemiesInWave.Length > 0)
            {
                // Elegir enemigo al azar de la lista actual (Nivel 0: Solo globo, Nivel 1: Globo o Gaviota)
                int enemyIndex = Random.Range(0, currentConfig.EnemiesInWave.Length);
                int spawnIndex = Random.Range(0, spawnpoints.Length);

                Instantiate(currentConfig.EnemiesInWave[enemyIndex], spawnpoints[spawnIndex].position, spawnpoints[spawnIndex].rotation);
            }
        }
    }
}