using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave_Spawner : MonoBehaviour
{
    public Wave_ScriptableObject[] gameLevels;
    [SerializeField] private Transform[] spawnpoints;

    private float timeBtwnSpawns;
    [SerializeField] private Wave_ScriptableObject currentConfig;

    // Bandera para saber si ya soltamos al jefe
    [SerializeField] private bool bossSpawned = false;

    private void Start()
    {
        if (gameLevels == null || gameLevels.Length == 0)
        {
            Debug.LogError("ERROR: Asigna los Game Levels en el Inspector del Spawner.");
            return;
        }
        UpdateWaveConfig();
    }

    private void Update()
    {
        // Debug Temporal
        if (GameManager.instance == null) { Debug.Log("No hay GameManager"); return; }
        if (GameManager.instance.currentGameState != GameManager.GameState.OnWave)
        {
            // Solo para ver si el problema es el estado, quitar después porque llenará la consola
            // Debug.Log("Esperando estado OnWave... Estado actual: " + GameManager.instance.currentGameState);
            return;
        }

        // ... resto de tu código

        if (GameManager.instance == null) return;
        if (GameManager.instance.currentGameState != GameManager.GameState.OnWave) return;

        UpdateWaveConfig(); // Asegurar que tenemos la config correcta

        // --- LÓGICA PARA EVITAR 20 CACHALOTES ---
        
        // 1. Si es OLEADA DE JEFE y YA SALIÓ, no hacemos nada más (return).
        if (currentConfig.isBossWave && bossSpawned) 
        {
            return; 
        }

        // 2. Comprobar tiempo de spawn
        if (Time.time >= timeBtwnSpawns)
        {
            SpawnWave();

            // Si acabamos de spawnear un Jefe, marcamos la bandera para no entrar más
            if (currentConfig.isBossWave)
            {
                bossSpawned = true;
                // Opcional: poner el tiempo en infinito por seguridad
                timeBtwnSpawns = Mathf.Infinity; 
            }
            else
            {
                // Si es oleada normal, reiniciamos el contador para el siguiente enemigo
                timeBtwnSpawns = Time.time + currentConfig.TimeBeforeThisWave;
            }
        }
    }

    private void UpdateWaveConfig()
    {
        int currentDifficulty = GameManager.difficultyLevel;
        if (currentDifficulty >= gameLevels.Length) currentDifficulty = gameLevels.Length - 1;

        // Detectar cambio de oleada para resetear la bandera del jefe
        if (currentConfig != gameLevels[currentDifficulty])
        {
            currentConfig = gameLevels[currentDifficulty];
            
            // Cada vez que cambia la dificultad/oleada, permitimos spawnear jefe de nuevo si toca
            bossSpawned = false; 
            
            // Ajustamos el primer tiempo de spawn
            timeBtwnSpawns = Time.time + currentConfig.TimeBeforeThisWave;
        }
        else
        {
            // Si es la misma config, solo aseguramos que la variable no sea nula al inicio
            if(currentConfig == null) currentConfig = gameLevels[currentDifficulty];
        }
    }

    private void SpawnWave()
    {
        if (spawnpoints == null || spawnpoints.Length == 0) return;

        if (currentConfig.EnemiesInWave != null && currentConfig.EnemiesInWave.Length > 0)
        {
            // Si es Boss, usualmente queremos el índice 0 (o un spawnpoint específico)
            int enemyIndex = Random.Range(0, currentConfig.EnemiesInWave.Length);
            
            // Elegir un spawnpoint al azar (o podrías forzar uno central para el jefe)
            int spawnIndex = Random.Range(0, spawnpoints.Length);

            Instantiate(currentConfig.EnemiesInWave[enemyIndex], spawnpoints[spawnIndex].position, spawnpoints[spawnIndex].rotation);
        }
    }
}