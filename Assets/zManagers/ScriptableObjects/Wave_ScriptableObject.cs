//using UnityEngine;

//[CreateAssetMenu(fileName = "Wave_ScriptableObject", menuName = "ScriptableObjects/Wave_ScriptableObject", order = 1)]
//public class Wave_ScriptableObject : ScriptableObject
//{
//    [field: SerializeField]

//    //numero de enemigos a generar
//    public GameObject[] EnemiesInWave {  get; private set; }
//    [field: SerializeField]

//    // tiempo entre olas
//    public float TimeBeforeThisWave { get; private set; }

//    [field: SerializeField]
//    //numero de spawn
//    public float NumberToSpawn { get; private set; }
//}

using UnityEngine;

[CreateAssetMenu(fileName = "Wave_ScriptableObject", menuName = "ScriptableObjects/Wave_ScriptableObject", order = 1)]
public class Wave_ScriptableObject : ScriptableObject
{
    [Header("Configuración de la Oleada")]

    [Tooltip("Arrastra aquí los prefabs de los enemigos (Gaviota, Globo, etc)")]
    [SerializeField] private GameObject[] enemiesInWave;

    [Tooltip("Tiempo de espera antes de empezar a spawnear")]
    [SerializeField] private float timeBeforeThisWave;

    [Tooltip("Cantidad total de enemigos a spawnear en esta tanda")]
    [SerializeField] private float numberToSpawn;


    // --- PROPIEDADES PÚBLICAS (Para que el Spawner pueda leerlas) ---

    public GameObject[] EnemiesInWave
    {
        get { return enemiesInWave; }
    }

    public float TimeBeforeThisWave
    {
        get { return timeBeforeThisWave; }
    }

    public float NumberToSpawn
    {
        get { return numberToSpawn; }
    }
}