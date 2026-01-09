using UnityEngine;
using System.Collections.Generic;

public class ShipPlacementManager : MonoBehaviour
{
    [Header("Configuración de Búsqueda")]
    [SerializeField] private string tagFondoModal = "FondoModalTienda";
    [SerializeField] private string nombreContenedorBarco = "ContenedorBarco";

    [Header("Referencias (Se buscan solas)")]
    public GameObject fondoModal; 
    public GameObject contenedorBarco;
    public List<PosicionMarinero> posicionesExistentes = new List<PosicionMarinero>();

    private PlantillaObjeto objetoEnEspera;
    private ShopManager tiendaActiva;

    void Awake()
    {
        ConfigurarReferencias();
    }

    private void ConfigurarReferencias()
    {
        // 1. Buscar el Fondo Modal por Tag
        if (fondoModal == null)
            fondoModal = GameObject.FindGameObjectWithTag(tagFondoModal);

        // 2. Buscar el Contenedor del Barco por nombre (o tag)
        if (contenedorBarco == null)
        {
            GameObject go = GameObject.Find(nombreContenedorBarco);
            if (go != null) contenedorBarco = go;
        }

        // 3. Buscar todos los slots de marineros en la escena
        if (posicionesExistentes.Count == 0)
        {
            posicionesExistentes.AddRange(Object.FindObjectsByType<PosicionMarinero>(FindObjectsSortMode.None));
        }

        // Inicializar oculto
        CerrarPanel();
    }

    public void AbrirSeleccionDePosicion(PlantillaObjeto objeto, ShopManager shop)
    {
        objetoEnEspera = objeto;
        tiendaActiva = shop;

        if (fondoModal != null) fondoModal.SetActive(true);
        if (contenedorBarco != null) contenedorBarco.SetActive(true);

        foreach (var pos in posicionesExistentes)
        {
            if (pos != null)
            {
                pos.gameObject.SetActive(true);
                pos.ActivarParpadeo(pos.EstaDisponible());
            }
        }
    }

    public void IntentarColocarEnSlot(PosicionMarinero slot)
    {
        if (objetoEnEspera == null) return;

        if (slot.AsignarMarinero(objetoEnEspera))
        {
            tiendaActiva.FinalizarCompraExitosa(objetoEnEspera);
            CerrarPanel();
        }
    }

    public void CerrarPanel()
    {
        objetoEnEspera = null;
        if (fondoModal != null) fondoModal.SetActive(false);
        if (contenedorBarco != null) contenedorBarco.SetActive(false);
    }
}