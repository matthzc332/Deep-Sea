using UnityEngine;

public class MarinerosManager : MonoBehaviour
{
    [Header("Datos")]
    [Tooltip("Arrastra aquí el ScriptableObject (ShipObject)")]
    public PlayerScripteable datosBarco;

    [Header("Referencias de Posiciones")]
    [Tooltip("Arrastra aquí el objeto 'Posicion1' de la jerarquía")]
    public Transform posicion1;

    [Tooltip("Arrastra aquí el objeto 'Posicion2' de la jerarquía")]
    public Transform posicion2;

    [Tooltip("Arrastra aquí el objeto 'Posicion3' de la jerarquía")]
    public Transform posicion3;

    void Start()
    {
        ColocarMarineros();
    }

    public void ColocarMarineros()
    {
        // Validación de seguridad
        if (datosBarco == null)
        {
            Debug.LogError("MarinerosManager: ¡Falta asignar el 'ShipObject'!");
            return;
        }

        // Revisamos y creamos cada uno de los 3 espacios
        VerificarYCrear(datosBarco.Espacio1, posicion1, "Espacio 1");
        VerificarYCrear(datosBarco.Espacio2, posicion2, "Espacio 2");
        VerificarYCrear(datosBarco.Espacio3, posicion3, "Espacio 3");
    }

    // Función auxiliar para no repetir código
    void VerificarYCrear(GameObject prefabMarinero, Transform posicionDestino, string nombreEspacio)
    {
        // 1. Si no hay posición asignada en el inspector, salimos
        if (posicionDestino == null) return;

        // 2. Si ya hay algo instanciado en esa posición, no ponemos otro encima
        if (posicionDestino.childCount > 0) return;

        // 3. Si el ScriptableObject tiene un prefab asignado para este espacio...
        if (prefabMarinero != null)
        {
            // Instanciamos: (Prefab, Posición, Rotación, Padre)
            Instantiate(prefabMarinero, posicionDestino.position, posicionDestino.rotation, posicionDestino);
            // Debug.Log($"MarinerosManager: Marinero creado en {nombreEspacio}");
        }
        else
        {
            // Si es null, simplemente no hace nada (deja el espacio vacío)
            // Debug.Log($"MarinerosManager: {nombreEspacio} está vacío en el ScriptableObject.");
        }
    }
}