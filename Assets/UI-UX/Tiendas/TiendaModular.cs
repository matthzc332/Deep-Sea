using UnityEngine;
using System.Collections.Generic;

public class TiendaModular : ShopManager
{
    [Header("Configuración de Inventario")]
    public List<PlantillaObjeto> todosLosObjetosEnEstaTienda = new List<PlantillaObjeto>();
    public int cantidadObjetosEnTienda = 9;
    
    [Header("Posicionamiento Inspección")]
    public Vector2 posicionInspeccion = new Vector2(-950f, 270f);

    private GameObject inspeccionActual;

    void Start()
    {
        InicializarTiendaModular();
        ActualizarUI();
    }

    public void InicializarTiendaModular()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);

        if (todosLosObjetosEnEstaTienda.Count == 0) return;

        for (int i = 0; i < cantidadObjetosEnTienda; i++)
        {
            int index = Random.Range(0, todosLosObjetosEnEstaTienda.Count);
            PlantillaObjeto data = todosLosObjetosEnEstaTienda[index];
            
            GameObject carta = Instantiate(cartaPrefab, transform);
            Objeto script = carta.GetComponent<Objeto>();
            if (script != null) script.ConfigurarObjeto(data, this);
        }
    }

    public void SeleccionarObjetoModular(PlantillaObjeto objeto, GameObject cartaUI)
    {
        if (inspeccionActual != null) Destroy(inspeccionActual);
        if (inspeccionPrefab == null || inspeccionContainer == null) return;

        inspeccionActual = Instantiate(inspeccionPrefab, inspeccionContainer);
        
        RectTransform rt = inspeccionActual.GetComponent<RectTransform>();
        if (rt != null) rt.anchoredPosition = posicionInspeccion;

        Objeto script = inspeccionActual.GetComponent<Objeto>();
        if (script != null)
        {
            // Pasamos la cartaUI para que el BuyButton sepa qué destruir si se compra
            script.ConfigurarObjeto(objeto, this, cartaUI);
        }
    }

    public override void ConfirmarVenta(PlantillaObjeto objeto, GameObject cartaVisual)
    {
        base.ConfirmarVenta(objeto, cartaVisual);
        if (inspeccionActual != null) Destroy(inspeccionActual);
    }
}