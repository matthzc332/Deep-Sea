using System.Collections.Generic;
using UnityEngine;

public class ArmaManager : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra aquí tus prefabs de armas. El orden importa (0, 1, 2...).")]
    public List<GameObject> listaArmas;

    void Start()
    {
        VerificarYCrearArma();
    }

    public void VerificarYCrearArma(int indice = 0)
    {
        // Verificar si hay arma
        if (transform.childCount > 0)
        {
            Debug.Log("ArmaManager: Ya hay un hijo (posiblemente un arma), no se creó nada nuevo.");
            return;
        }

        if (listaArmas == null || listaArmas.Count == 0)
        {
            Debug.LogError("ArmaManager: ¡La lista de armas está vacía o no asignada en el Inspector!");
            return;
        }

        // Verificamos la lista de armas  (desde el scripteableobject se debera enviar el indice y asegurarse que el indice coincida y exista en la lista de armas)
        if (indice < 0 || indice >= listaArmas.Count)
        {
            Debug.LogWarning($"ArmaManager: El índice {indice} está fuera de rango. Se usará el arma 0 por seguridad.");
            indice = 0;
        }
        // 4. Obtenemos el prefab de la lista usando el número
        GameObject prefabSeleccionado = listaArmas[indice];

        if (prefabSeleccionado != null)
        {
            // 5. Instanciamos el arma como hija de este objeto
            Instantiate(prefabSeleccionado, transform.position, transform.rotation, transform);

            Debug.Log($"ArmaManager: Se creó el arma número {indice} ({prefabSeleccionado.name}).");
        }
        else
        {
            Debug.LogError($"ArmaManager: El espacio {indice} de la lista está vacío (null). Revisa el Inspector.");
        }
    }
}