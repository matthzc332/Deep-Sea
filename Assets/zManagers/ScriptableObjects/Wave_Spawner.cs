using UnityEngine;

public class Wave_Spawner : MonoBehaviour
{
    public Wave_ScriptableObject[] gameLevels;
    [SerializeField] private Transform[] spawnpoints;

    [Header("Ajustes de Flujo")]
    [SerializeField] private int maxEnemiesAlive = 5;
    [Tooltip("Duración de las oleadas normales en segundos")]
    [SerializeField] private float normalWaveDuration = 30f; 

    private float timeBtwnSpawns;
    private float waveEndTime; // Momento exacto en que debe terminar
    [SerializeField] private Wave_ScriptableObject currentConfig;

    private bool bossSpawned = false;
    private bool waveFinished = false;
    
    [Header("Contadores de Estado")]
    [SerializeField] private int currentEnemyCount = 0; 
    [SerializeField] private int enemiesSpawnedInTotal = 0; 

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
        if (GameManager.instance == null || GameManager.instance.currentGameState != GameManager.GameState.OnWave) return;
        if (waveFinished) return;

        UpdateWaveConfig();

        // 1. Lógica de Finalización
        if (currentConfig.isBossWave)
        {
            // Termina si el jefe fue spawneado y ya no existe en la escena
            bool bossAlive = GameObject.FindGameObjectWithTag("Boss") != null;
            if (bossSpawned && !bossAlive) EndWave();
        }
        else
        {
            // Termina cuando el tiempo se agota
            if (Time.time >= waveEndTime) EndWave();
        }

        // 2. Lógica de Spawneo (Solo spawnea si hay espacio)
        if (currentEnemyCount < maxEnemiesAlive)
        {
            // En oleadas normales, ignoramos el "NumberToSpawn" y spawneamos por tiempo infinito
            // En oleadas de jefe, podrías querer limitar los minions (opcional)
            if (Time.time >= timeBtwnSpawns)
            {
                SpawnEnemy();
                timeBtwnSpawns = Time.time + currentConfig.TimeBeforeThisWave;
            }
        }
    }

    private void EndWave()
    {
        if (waveFinished) return;
        waveFinished = true;
        Debug.Log("Oleada Completada. Iniciando transición...");
        // Llama aquí a tu rutina de fin de oleada del GameManager
    }

    private void UpdateWaveConfig()
    {
        int currentDifficulty = GameManager.difficultyLevel;
        if (currentDifficulty >= gameLevels.Length) currentDifficulty = gameLevels.Length - 1;

        if (currentConfig != gameLevels[currentDifficulty])
        {
            currentConfig = gameLevels[currentDifficulty];
            bossSpawned = false;
            enemiesSpawnedInTotal = 0; 
            waveFinished = false;
            
            // Calculamos cuánto durará esta oleada si es normal
            waveEndTime = Time.time + normalWaveDuration;
            timeBtwnSpawns = Time.time + currentConfig.TimeBeforeThisWave;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnpoints == null || spawnpoints.Length == 0 || currentConfig.EnemiesInWave.Length == 0) return;
        
        int enemyIndex = 0;
        if (currentConfig.isBossWave)
        {
            if (!bossSpawned) { enemyIndex = 0; bossSpawned = true; }
            else { enemyIndex = Random.Range(1, currentConfig.EnemiesInWave.Length); }
        }
        else { enemyIndex = Random.Range(0, currentConfig.EnemiesInWave.Length); }

        int spawnIndex = Random.Range(0, spawnpoints.Length);
        GameObject enemy = Instantiate(currentConfig.EnemiesInWave[enemyIndex], spawnpoints[spawnIndex].position, spawnpoints[spawnIndex].rotation);
        
        currentEnemyCount++;
        enemiesSpawnedInTotal++; 
        
        EnemyDeathNotifier notifier = enemy.GetComponent<EnemyDeathNotifier>() ?? enemy.AddComponent<EnemyDeathNotifier>();
        notifier.OnDeath += () => { currentEnemyCount--; };
    }
}