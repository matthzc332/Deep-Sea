using UnityEngine;

public class Base_Gun : MonoBehaviour
{

    [Header("Tuning")]
    public int amount_ammunition = 50;
    public float time_recharge = 1;
    public float power_shoot = 1;
    public float damage_percentage = 100;
    protected GameObject ship;

    public Vector3 objective;
    
    public ShipData shipData;

    void Start()
    {
        ship = FindParentWithTag(transform, "Ship");

        // SINCRONIZACIÓN:
        if (shipData != null)
        {
            // Igualamos la munición del arma a la del ScriptableObject
            amount_ammunition = shipData.municion;
            Debug.Log($"Arma sincronizada: {amount_ammunition} balas.");
        }
        else
        {
            Debug.LogError("¡Base_Gun no tiene asignado el ShipData en el inspector!");
        }
    }


    // Método para buscar recursivamente en los padres
    private GameObject FindParentWithTag(Transform current, string tag)
{
    Transform parent = current.parent;
    
    while (parent != null)
    {
        if (parent.CompareTag(tag))
        {
            Debug.Log("El arma encontró el barco!");
            return parent.gameObject;
        }
        parent = parent.parent;
    }
    
    return null; // No se encontró ningún padre con la etiqueta
}

}
