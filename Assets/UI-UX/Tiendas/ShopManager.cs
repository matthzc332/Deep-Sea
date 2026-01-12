using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    [Header("Sistemas Externos")]
    [SerializeField] private ShipPlacementManager placementManager;

    [Header("UI References")]
    public Transform containerTienda;
    public GameObject cartaPrefab;
    public GameObject inspeccionPrefab;
    public Transform inspeccionContainer;
    public TextMeshProUGUI textoMonedas;

    [Header("Configuración Tienda")]
    [SerializeField] private int numeroObjetosEnTienda = 6;
    [SerializeField] private bool usarTiendaAleatoria = true;

    [Header("Datos y Economía")]
    public List<PlantillaObjeto> todosLosObjetos = new List<PlantillaObjeto>();
    public int monedaJugador = 500;

    private List<PlantillaObjeto> objetosDisponibles = new List<PlantillaObjeto>();
    private PlantillaObjeto seleccionActual;
    private GameObject inspeccionActual;
    private GameObject objetoTiendaOrigen; 

    void Start()
    {
        InicializarTienda();
        ActualizarUI();
        
        if (objetosDisponibles.Count > 0)
            SeleccionarMarinero(objetosDisponibles[0]);
    }

    public void InicializarTienda()
    {
        objetosDisponibles.Clear();
        if (usarTiendaAleatoria)
            objetosDisponibles = todosLosObjetos.OrderBy(x => Random.value).Take(numeroObjetosEnTienda).ToList();
        else
            objetosDisponibles = new List<PlantillaObjeto>(todosLosObjetos);
        
        LlenarTienda();
    }

    void LlenarTienda()
    {
        foreach (Transform child in containerTienda) Destroy(child.gameObject);

        foreach (var objeto in objetosDisponibles)
        {
            GameObject carta = Instantiate(cartaPrefab, containerTienda);
            Objeto objetoScript = carta.GetComponent<Objeto>();
            if (objetoScript != null)
                objetoScript.ConfigurarObjeto(objeto, this); 
        }
    }

    public void SeleccionarMarinero(PlantillaObjeto objeto)
    {
        seleccionActual = objeto;
        CrearVistaInspeccion(objeto);
    }

    void CrearVistaInspeccion(PlantillaObjeto objeto)
    {
        if (inspeccionActual != null) Destroy(inspeccionActual);
        if (inspeccionPrefab == null || inspeccionContainer == null) return;

        inspeccionActual = Instantiate(inspeccionPrefab, inspeccionContainer);
        
        // Ajuste de posición UI
        RectTransform rt = inspeccionActual.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = new Vector2(-950f, 270f);
            rt.localPosition = new Vector3(-950f, 270f, 0f);
            rt.localScale = Vector3.one;
            Debug.Log($"Inspección posicionada en: {rt.anchoredPosition}");
        }
        else
        {
            inspeccionActual.transform.localPosition = new Vector3(-950f, 270f, 0f);
            inspeccionActual.transform.localScale = Vector3.one;
        }


        Objeto objetoScript = inspeccionActual.GetComponent<Objeto>();
        if (objetoScript != null)
        {
            objetoScript.ConfigurarObjeto(objeto, this);
            objetoScript.ConfigurarBotonCompra(ComprarObjetoActual);
        }
    }

    public void ComprarObjetoActual()
    {
        if (seleccionActual == null) return;

        // Si la referencia es nula, intentamos buscarla en la escena antes de fallar
        if (placementManager == null)
        {
            placementManager = Object.FindAnyObjectByType<ShipPlacementManager>();
        }

        if (monedaJugador >= seleccionActual.precio)
        {
            if (placementManager != null)
            {
                placementManager.AbrirSeleccionDePosicion(seleccionActual, this);
            }
            else
            {
                Debug.LogError("ERROR: Sigue sin encontrarse ShipPlacementManager en la escena.");
            }
        }
        if (monedaJugador >= seleccionActual.precio)
        {
            // 1. Restar dinero
            monedaJugador -= seleccionActual.precio;
            ActualizarUI();

            // 2. Identificar la carta visual para borrarla después
            objetoTiendaOrigen = EncontrarObjetoTiendaOrigen();

            // 3. PASAR EL MANDO AL MANAGER DE POSICIONAMIENTO
            if (placementManager != null)
            {
                placementManager.AbrirSeleccionDePosicion(seleccionActual, this);
            }
            else
            {
                Debug.LogError("No se encontró el ShipPlacementManager asignado.");
            }
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
        }
    }

    // Método que llama el ShipPlacementManager cuando el jugador hace clic en un slot válido
    public void FinalizarCompraExitosa(PlantillaObjeto objeto)
    {
        objetosDisponibles.Remove(objeto);
        if (objetoTiendaOrigen != null) Destroy(objetoTiendaOrigen);
        if (inspeccionActual != null) Destroy(inspeccionActual);
        
        Debug.Log($"Compra de {objeto.nombre} finalizada con éxito.");
    }

    private GameObject EncontrarObjetoTiendaOrigen()
    {
        return containerTienda.Cast<Transform>()
            .Select(t => t.GetComponent<Objeto>())
            .FirstOrDefault(o => o != null && o.GetDatosMarinero() == seleccionActual)?.gameObject;
    }

    void ActualizarUI() { if (textoMonedas != null) textoMonedas.text = $"Monedas: {monedaJugador}"; }
}