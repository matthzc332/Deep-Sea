//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class Wave_Spawner : MonoBehaviour
//{
//    public Wave_ScriptableObject[] waves;

//    private Wave_ScriptableObject currentWave;

//    [SerializeField]
//    private Transform[] spawnpoints;

//    private float timeBtwnSpawns;
//    private int i = 0;

//    private bool stopSpawning = false;

//    private void Awake()
//    {
//        currentWave = waves[i];
//        timeBtwnSpawns = currentWave.TimeBeforeThisWave;
//    }
//    private void Update()
//    {
//        if (stopSpawning)
//        {
//            return;
//        }

//        if (Time.time >= timeBtwnSpawns)
//        {
//            SpawnWave();
//            IncWave();

//            timeBtwnSpawns = Time.time + currentWave.TimeBeforeThisWave;
//        }
//    }

//    // aumenta dificultad del spawn
//    private void SpawnWave()
//    {

//        // Calculamos cuántos enemigos extra spawnear
//        int extraEnemies = 0;
//        if (GameManager.instance != null)
//        {
//            extraEnemies = GameManager.instance.difficultyLevel;
//            // Ojo: si quieres que sea más agresivo, multiplica: difficultyLevel * 2
//        }

//        //use float en vez de int
//        float totalToSpawn = currentWave.NumberToSpawn + extraEnemies;

//        // Usamos el nuevo total en el loop
//        for (int i = 0; i < totalToSpawn; i++)
//        {
//            int num = Random.Range(0, currentWave.EnemiesInWave.Length);
//            int num2 = Random.Range(0, spawnpoints.Length);

//            Instantiate(currentWave.EnemiesInWave[num], spawnpoints[num2].position,
//                spawnpoints[num2].rotation);
//        }
//    }

//    // incrementa olas
//    private void IncWave()
//    {
//        if (i + 1 < waves.Length)
//        {
//            i++;
//            currentWave = waves[i];
//        }
//        else
//        {
//            stopSpawning = true;
//        }
//    } 
//}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave_Spawner : MonoBehaviour
{
    // Aquí arrastras tus ScriptableObjects en orden:
    // Elemento 0: Nivel_Facil (Solo gaviotas)
    // Elemento 1: Nivel_Medio (Gaviotas + Globos)
    // Elemento 2: Nivel_Dificil (Gaviotas + Globos + Otro), etc.
    public Wave_ScriptableObject[] gameLevels;

    private Wave_ScriptableObject currentWaveConfig;
    private float timeBtwnSpawns;

    [SerializeField]
    private Transform[] spawnpoints;

    private void Start()
    {
        // --- PROTECCIÓN DE SEGURIDAD ---
        // Si la lista está vacía en el inspector, esto evita el error IndexOutOfRange
        if (gameLevels == null || gameLevels.Length == 0)
        {
            Debug.LogError("ERROR CRÍTICO: ¡La lista 'Game Levels' en Wave_Spawner está vacía! Asigna los ScriptableObjects en el Inspector.");
            return;
        }
        // -------------------------------

        // 1. Determinar qué configuración usar según la dificultad del GameManager
        int difficultIndex = 0;

        if (GameManager.instance != null)
        {
            difficultIndex = GameManager.instance.difficultyLevel;
        }

        // Si la dificultad es mayor que la cantidad de niveles que diseñaste, usa el último disponible
        if (difficultIndex >= gameLevels.Length)
        {
            difficultIndex = gameLevels.Length - 1;
        }

        // Protección extra por si el índice es negativo
        if (difficultIndex < 0) difficultIndex = 0;

        currentWaveConfig = gameLevels[difficultIndex];

        // Configurar el primer spawn (asegurando que currentWaveConfig existe)
        if (currentWaveConfig != null)
        {
            timeBtwnSpawns = Time.time + currentWaveConfig.TimeBeforeThisWave;
        }
    }

    private void Update()
    {
        // SEGURIDAD: Si no hay configuración cargada, no hacemos nada para evitar errores
        if (currentWaveConfig == null) return;

        // Si estamos en pausa o no es momento de oleada, no spawnear
        if (GameManager.instance != null && GameManager.instance.currentGameState != GameManager.GameState.OnWave) return;

        if (Time.time >= timeBtwnSpawns)
        {
            SpawnWave();
            // Reiniciamos el contador para el siguiente grupo de enemigos
            timeBtwnSpawns = Time.time + currentWaveConfig.TimeBeforeThisWave;
        }
    }

    private void SpawnWave()
    {
        // Calculamos cuántos enemigos extra spawnear por dificultad
        int extraEnemies = 0;
        if (GameManager.instance != null)
        {
            extraEnemies = GameManager.instance.difficultyLevel;
        }

        float totalToSpawn = currentWaveConfig.NumberToSpawn + extraEnemies;

        for (int i = 0; i < totalToSpawn; i++)
        {
            // Elegir enemigo aleatorio de la configuración actual
            if (currentWaveConfig.EnemiesInWave != null && currentWaveConfig.EnemiesInWave.Length > 0)
            {
                int enemyIndex = Random.Range(0, currentWaveConfig.EnemiesInWave.Length);

                if (spawnpoints != null && spawnpoints.Length > 0)
                {
                    int spawnIndex = Random.Range(0, spawnpoints.Length);
                    Instantiate(currentWaveConfig.EnemiesInWave[enemyIndex], spawnpoints[spawnIndex].position, spawnpoints[spawnIndex].rotation);
                }
            }
        }
    }
}