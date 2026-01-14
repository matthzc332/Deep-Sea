using UnityEngine;
using System.Collections.Generic;

public class ShipPlacementManager : MonoBehaviour
{
    [Header("Configuración de UI")]
    [SerializeField] private string tagFondoModal = "FondoModalTienda";
    [SerializeField] private string nombreContenedorBarco = "ContenedorBarco";

    [Header("Referencias de Escena")]
    public GameObject fondoModal; 
    public GameObject contenedorBarco;
    public List<PosicionMarinero> posicionesExistentes = new List<PosicionMarinero>();

    // Variables de control de la transacción actual
    private PlantillaObjeto objetoEnEspera;
    private ShopManager tiendaActiva;
    private GameObject cartaOrigenUI;

    void Awake()
    {
        ConfigurarReferencias();
    }

    private void ConfigurarReferencias()
    {
        if (fondoModal == null)
            fondoModal = GameObject.FindGameObjectWithTag(tagFondoModal);

        if (contenedorBarco == null)
        {
            GameObject go = GameObject.Find(nombreContenedorBarco);
            if (go != null) contenedorBarco = go;
        }

        if (posicionesExistentes.Count == 0)
        {
            posicionesExistentes.AddRange(Object.FindObjectsByType<PosicionMarinero>(FindObjectsSortMode.None));
        }

        CerrarPanel(); 
    }

    public void AbrirSeleccionDePosicion(PlantillaObjeto objeto, ShopManager shop, GameObject cartaUI)
    {
        objetoEnEspera = objeto;
        tiendaActiva = shop;
        cartaOrigenUI = cartaUI;

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

    // ESTE ES EL MÉTODO QUE DABA ERROR SI ESTABA FUERA DE LA CLASE
    public void IntentarColocarEnSlot(PosicionMarinero slot)
    {
        if (objetoEnEspera == null || tiendaActiva == null) return;

        if (slot.AsignarMarinero(objetoEnEspera))
        {
            // ÉXITO: La tienda cobra y destruye la carta
            tiendaActiva.ConfirmarVenta(objetoEnEspera, cartaOrigenUI);
            CerrarPanel();
        }
        else
        {
            Debug.Log("El slot seleccionado ya está ocupado.");
        }
    }

    public void CerrarPanel()
    {
        objetoEnEspera = null;
        cartaOrigenUI = null;

        if (fondoModal != null) fondoModal.SetActive(false);
        if (contenedorBarco != null) contenedorBarco.SetActive(false);

        foreach (var pos in posicionesExistentes)
        {
            if (pos != null) pos.ActivarParpadeo(false);
        }
    }
}