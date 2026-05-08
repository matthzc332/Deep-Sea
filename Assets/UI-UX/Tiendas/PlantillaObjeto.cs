using UnityEngine;

[CreateAssetMenu(fileName = "Objeto data", menuName = "Tienda/Objeto data")]
public class PlantillaObjeto : ScriptableObject
{
    public bool esPermanente;
    public string nombreId;
    public string nombre;
    public string strongWith;
    public string weakWith;
    [TextArea(3, 10)] public string descripcion;
    public int precio;
    public Sprite[] idleAnimationSprites; 
    public float animationSpeed = 2f; // Valor por defecto mayor a 0
    public GameObject prefabDelObjeto; // Este es el campo para el Prefab
}