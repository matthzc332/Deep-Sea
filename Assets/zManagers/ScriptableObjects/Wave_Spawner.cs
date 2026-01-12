using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave_Spawner : MonoBehaviour
{
    public Wave_ScriptableObject[] waves;

    private Wave_ScriptableObject currentWave;

    [SerializeField]
    private Transform[] spawnpoints;

    private float timeBtwnSpawns;
    private int i = 0;

    private bool stopSpawning = false;

    private void Awake()
    {
        currentWave = waves[i];
        timeBtwnSpawns = currentWave.TimeBeforeThisWave;
    }
    private void Update()
    {
        if (stopSpawning)
        {
            return;
        }

        if (Time.time >= timeBtwnSpawns)
        {
            SpawnWave();
            IncWave();

            timeBtwnSpawns = Time.time + currentWave.TimeBeforeThisWave;
        }
    }

    // aumenta dificultad del spawn
    private void SpawnWave()
    {

        // Calculamos cuántos enemigos extra spawnear
        int extraEnemies = 0;
        if (GameManager.instance != null)
        {
            extraEnemies = GameManager.instance.difficultyLevel;
            // Ojo: si quieres que sea más agresivo, multiplica: difficultyLevel * 2
        }

        //use float en vez de int
        float totalToSpawn = currentWave.NumberToSpawn + extraEnemies;

        // Usamos el nuevo total en el loop
        for (int i = 0; i < totalToSpawn; i++)
        {
            int num = Random.Range(0, currentWave.EnemiesInWave.Length);
            int num2 = Random.Range(0, spawnpoints.Length);

            Instantiate(currentWave.EnemiesInWave[num], spawnpoints[num2].position,
                spawnpoints[num2].rotation);
        }
    }

    //codigo principal
    //private void SpawnWave()
    //{
    //    for (int i = 0; i < currentWave.NumberToSpawn; i++)
    //    {
    //        int num = Random.Range(0, currentWave.EnemiesInWave.Length);
    //        int num2 = Random.Range(0, spawnpoints.Length);

    //        Instantiate(currentWave.EnemiesInWave[num], spawnpoints[num2].position,
    //            spawnpoints[num2].rotation);
    //    }
    //}

    // incrementa olas
    private void IncWave()
    {
        if (i + 1 < waves.Length)
        {
            i++;
            currentWave = waves[i];
        }
        else
        {
            stopSpawning = true;
        }
    } 
}