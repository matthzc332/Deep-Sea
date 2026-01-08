using UnityEngine;
using System.Collections.Generic;

public class ShopGenerator : MonoBehaviour
{
    [Header("Configuración de Datos")]
    [SerializeField] private TiendaData tiendaData;
    
    [Header("Referencias de Prefabs")]
    [SerializeField] private GameObject cartaPrefab;
    [SerializeField] private Transform containerGrid;

    void Start()
    {
        GenerarContenido();
    }

    public void GenerarContenido()
    {
        // Limpiar el contenedor antes de generar
        foreach (Transform child in containerGrid)
            Destroy(child.gameObject);

        if (tiendaData == null) {
            Debug.LogError($"No hay TiendaData asignado en {gameObject.name}");
            return;
        }

        foreach (PlantillaObjeto data in tiendaData.listaProductos)
        {
            GameObject carta = Instantiate(cartaPrefab, containerGrid);
            Objeto objetoScript = carta.GetComponent<Objeto>();
            
            if (objetoScript != null)
            {
                // Pasamos la data a la carta. 
                // Nota: Objeto.cs debe ser capaz de avisar al PurchaseManager cuando se hace click.
                objetoScript.ConfigurarObjeto(data); 
            }
        }
    }
}