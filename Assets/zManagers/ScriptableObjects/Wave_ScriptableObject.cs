using UnityEngine;

[CreateAssetMenu(fileName = "Wave_ScriptableObject", menuName = "ScriptableObjects/Wave_ScriptableObject", order = 1)]
public class Wave_ScriptableObject : ScriptableObject
{
    [field: SerializeField]

    //numero de enemigos a generar
    public GameObject[] EnemiesInWave {  get; private set; }
    [field: SerializeField]
    
    // tiempo entre olas
    public float TimeBeforeThisWave { get; private set; }

    [field: SerializeField]
    //numero de spawn
    public float NumberToSpawn { get; private set; }
}
