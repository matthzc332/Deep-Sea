using UnityEngine;
using System.Collections.Generic;

public class ShopGenerator : MonoBehaviour
{
    [Header("Configuración de Datos")]
    [SerializeField] private TiendaData tiendaData;
    
    [Header("Referencias de Prefabs")]
    [SerializeField] private GameObject cartaPrefab;
    [SerializeField] private Transform containerGrid;

    // NUEVO: Referencia al manager que realmente procesa las compras
    [Header("Manager Responsable")]
    [SerializeField] private ShopManager shopManager;

    void Start()
    {
        GenerarContenido();
    }

    public void GenerarContenido()
    {
        foreach (Transform child in containerGrid)
            Destroy(child.gameObject);

        if (tiendaData == null) {
            Debug.LogError($"No hay TiendaData asignado en {gameObject.name}");
            return;
        }

        // Si no asignaste un manager en el inspector, intentamos buscarlo en el mismo objeto
        if (shopManager == null) shopManager = GetComponent<ShopManager>();

        foreach (PlantillaObjeto data in tiendaData.listaProductos)
        {
            GameObject carta = Instantiate(cartaPrefab, containerGrid);
            Objeto objetoScript = carta.GetComponent<Objeto>();
            
            if (objetoScript != null)
            {
                // Ahora pasamos la data Y el manager responsable
                objetoScript.ConfigurarObjeto(data, shopManager); 
            }
        }
    }
}