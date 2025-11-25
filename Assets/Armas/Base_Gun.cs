using UnityEngine;

public class Base_Gun : MonoBehaviour
{

    [Header("Tuning")]
    public int amount_ammunition = 5;
    public float time_recharge = 1;
    public float power_shoot = 1;
    public float damage_percentage = 100;
    protected GameObject ship;

    public Vector3 objective;


    void Start(){
        ship = FindParentWithTag(transform, "Ship");
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
