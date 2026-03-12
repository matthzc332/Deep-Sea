using UnityEngine;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class ShipPlacementManager : MonoBehaviour
{
    [Header("Configuración de UI")]
    [SerializeField] private string tagFondoModal = "FondoModalTienda";
    [SerializeField] private string nombreContenedorBarco = "ContenedorBarco";

    [Header("Referencias de Escena")]
    public GameObject fondoModal;
    public GameObject contenedorBarco;
    public List<PosicionMarinero> posicionesExistentes = new List<PosicionMarinero>();

    [Header("UI de Espacio")]
    public TextMeshProUGUI textoEspacio; // Referencia al texto 0/2


    // Variables de control de la transacción actual
    private PlantillaObjeto objetoEnEspera;
    private ShopManager tiendaActiva;
    private GameObject cartaOrigenUI;


    void Start()
    {
        ActualizarTextoContador(); // Actualiza al empezar la escena (0/2)
    }
    // public void ActualizarTextoContador()
    // {
    //     if (textoEspacio != null)
    //     {
    //         int ocupados = GetCantidadOcupada();
    //         int total = posicionesExistentes.Count;
    //         textoEspacio.text = $"Espacio: {ocupados}/{total}";

    //         // Feedback visual opcional
    //         textoEspacio.color = (ocupados >= total) ? Color.red : Color.white;
    //     }
    // }



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

    // En ShipPlacementManager.cs

    public void ActualizarTextoContador()
    {
        if (textoEspacio == null) return;

        int ocupados = 0;
        foreach (var pos in posicionesExistentes)
        {
            // Forzamos la comprobación directa
            if (pos != null && !pos.EstaDisponible())
            {
                ocupados++;
            }
        }

        int total = posicionesExistentes.Count;
        textoEspacio.text = $"Espacio: {ocupados}/{total}";

        // Feedback visual: Rojo si está lleno
        textoEspacio.color = (ocupados >= total) ? Color.red : Color.white;
    }

    // public void IntentarColocarEnSlot(PosicionMarinero slot)
    // {
    //     if (objetoEnEspera == null || tiendaActiva == null) return;

    //     if (slot.AsignarMarinero(objetoEnEspera))
    //     {
    //         tiendaActiva.ConfirmarVenta(objetoEnEspera, cartaOrigenUI);

    //         // REFRESCAR CONTADOR AQUÍ
    //         ActualizarTextoContador(); 

    //         CerrarPanel();
    //     }
    // }
    public void IntentarColocarEnSlot(PosicionMarinero slot)
    {
        if (objetoEnEspera == null || tiendaActiva == null) return;

        if (slot.AsignarMarinero(objetoEnEspera))
        {
            tiendaActiva.ConfirmarVenta(objetoEnEspera, cartaOrigenUI);

            // 1. Actualizamos el texto 2/2
            ActualizarTextoContador();

            // 2. BUSCAMOS LOS BOTONES Y LOS ACTUALIZAMOS
            BuyCrew[] botonesCompra = Object.FindObjectsByType<BuyCrew>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var btn in botonesCompra)
            {
                btn.ActualizarEstadoBoton();
            }

            CerrarPanel();
        }
    }

    // // ESTE ES EL MÉTODO QUE DABA ERROR SI ESTABA FUERA DE LA CLASE
    // public void IntentarColocarEnSlot(PosicionMarinero slot)
    // {
    //     if (objetoEnEspera == null || tiendaActiva == null) return;

    //     if (slot.AsignarMarinero(objetoEnEspera))
    //     {
    //         // ÉXITO: La tienda cobra y destruye la carta
    //         tiendaActiva.ConfirmarVenta(objetoEnEspera, cartaOrigenUI);
    //         tiendaActiva.ActualizarUI();
    //         ActualizarTextoContador();
    //         CerrarPanel();
    //     }
    //     else
    //     {
    //         Debug.Log("El slot seleccionado ya está ocupado.");
    //     }
    // }

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

    // Contador de lugares en barco


    public bool TieneEspacioDisponible()
    {
        // Si la lista está vacía, intentamos buscar de nuevo
        if (posicionesExistentes.Count == 0)
        {
            posicionesExistentes.AddRange(Object.FindObjectsByType<PosicionMarinero>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        }

        foreach (var pos in posicionesExistentes)
        {
            if (pos != null && pos.EstaDisponible())
            {
                return true;
            }
        }
        return false; // Si no hay ninguna posición disponible, devolvemos false
    }

    public int GetCantidadOcupada()
    {
        int ocupados = 0;
        foreach (var pos in posicionesExistentes)
        {
            // Si no está disponible, es porque hay un marinero
            if (pos != null && !pos.EstaDisponible()) ocupados++;
        }
        return ocupados;
    }

    // Ejemplo de feedback para el texto de UI
    public void FeedbackTextoLleno()
    {
        if (textoEspacio == null) return;

        // Detenemos cualquier animación previa en el texto para que no se solapen
        textoEspacio.transform.DOKill();
        textoEspacio.DOKill();

        // Reset de escala y color por seguridad
        Vector3 escalaOriginal = textoEspacio.transform.localScale;
        textoEspacio.transform.localScale = escalaOriginal;
        //textoEspacio.transform.localScale = Vector3.one;

        // Animación: Sacudida de escala y destello rojo
        textoEspacio.transform.DOShakePosition(0.4f, 10f, 20, 90);
        textoEspacio.DOColor(Color.red, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            // Al terminar, vuelve al color rojo fijo si sigue lleno o blanco si no
            textoEspacio.color = (GetCantidadOcupada() >= posicionesExistentes.Count) ? Color.red : Color.white;
        }).SetUpdate(true);
    }
}