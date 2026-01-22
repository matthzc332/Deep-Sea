using UnityEngine;

[CreateAssetMenu(fileName = "Wave_ScriptableObject", menuName = "ScriptableObjects/Wave_ScriptableObject", order = 1)]
public class Wave_ScriptableObject : ScriptableObject
{
    [Header("Configuración de la Oleada")]

    [Tooltip("Si marcas esto, solo se spawneará UNA vez el primer enemigo de la lista.")]
    public bool isBossWave = false; // <--- AGREGAMOS ESTO

    [Tooltip("Arrastra aquí los prefabs. Si es Boss Wave, pon solo al Jefe.")]
    [SerializeField] private GameObject[] enemiesInWave;

    [Tooltip("Tiempo de espera antes de empezar / Intervalo entre enemigos")]
    [SerializeField] private float timeBeforeThisWave;

    [Tooltip("Cantidad total (Ignorado si es Boss Wave)")]
    [SerializeField] private float numberToSpawn;

    // --- PROPIEDADES PÚBLICAS ---

    public GameObject[] EnemiesInWave => enemiesInWave;
    public float TimeBeforeThisWave => timeBeforeThisWave;
    public float NumberToSpawn => numberToSpawn;
}