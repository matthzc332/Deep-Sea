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

public override void ActualizarUI()
{
    // 1. Verificamos si esta tienda vende objetos permanentes (habilidades)
    // para decidir qué texto mostrar en el contador de dinero
    bool esTiendaDeHabilidades = false;
    if (todosLosObjetosEnEstaTienda.Count > 0 && todosLosObjetosEnEstaTienda[0] != null)
    {
        esTiendaDeHabilidades = todosLosObjetosEnEstaTienda[0].esPermanente;
    }

    // 2. Actualizamos el texto del dinero según el tipo de tienda
    if (textoMonedas != null) 
    {
        string etiquetaMoneda = esTiendaDeHabilidades ? "Puntos XP" : "Monedas";
        textoMonedas.text = $"{etiquetaMoneda}: {monedaJugador}";
    }

    // 3. Manejo del texto de espacio/estado
    if (textoEspacio != null) 
    {
        if (esTiendaDeHabilidades)
        {
            textoEspacio.text = "HABILIDADES"; // O dejarlo vacío
            textoEspacio.color = Color.white;
        }
        else
        {
            // Si es tienda de marineros, mostramos el espacio del barco
            ShipPlacementManager placement = Object.FindFirstObjectByType<ShipPlacementManager>();
            if (placement != null)
            {
                int ocupados = placement.GetCantidadOcupada();
                int total = placement.posicionesExistentes.Count;
                textoEspacio.text = $"Espacio: {ocupados}/{total}";
                textoEspacio.color = (ocupados >= total) ? Color.red : Color.white;
            }
        }
    }
}}

//     public override void ActualizarUI()
// {
//     // 1. Actualiza solo el dinero usando la lógica de la clase padre
//     if (textoMonedas != null) textoMonedas.text = $"Monedas: {monedaJugador}";

//     // 2. Limpia o desactiva el texto de espacio para que no diga 0/2
//     if (textoEspacio != null) 
//     {
//         textoEspacio.text = "Ya Adquirido"; // O puedes poner algo como "Habilidades"
//     }
// }
// }