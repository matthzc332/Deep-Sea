using UnityEngine;

public class ArmaManager : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra aquí el archivo de ScriptableObject (ShipObject)")]
    public PlayerScripteable datosBarco;
    public GameObject armaDefault;

    void Start()
    {
        VerificarYCrearArma();
    }

    public void VerificarYCrearArma()
    {
        // 1. Verificamos si ya hay un hijo (para evitar duplicar armas)
        if (transform.childCount > 0)
        {
            // Debug.Log("ArmaManager: Ya hay un hijo instanciado, no se creó nada nuevo.");
            return;
        }

        // 2. Verificamos que hayas asignado el ScriptableObject en el inspector
        if (datosBarco == null)
        {
            Debug.LogError("ArmaManager: ¡Falta asignar el 'ShipObject' en el Inspector!");
            return;
        }

        // 3. Verificamos si el ScriptableObject tiene un prefab de arma asignado
        if (datosBarco.Arma != null)
        {
            // 4. Instanciamos el arma como hijo de este objeto
            Instantiate(datosBarco.Arma, transform.position, transform.rotation, transform);

            Debug.Log($"ArmaManager: Se creó el arma '{datosBarco.Arma.name}' usando los datos del ScriptableObject.");
        }
        else
        {
            Instantiate(armaDefault, transform.position, transform.rotation, transform);
            Debug.LogWarning("ArmaManager: El ScriptableObject asignado tiene la casilla 'Arma' vacía.");
        }
    }
}