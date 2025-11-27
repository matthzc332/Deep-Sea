
////}

//using UnityEngine;
//using UnityEngine.UI; // Necesario para manipular la UI
//using System.Collections.Generic;
//using TMPro;

//public class ShopManager : MonoBehaviour
//{
//    public static ShopManager Instance { get; private set; }

//    [Header("Configuración Lista (Izquierda)")]
//    public Transform container;
//    public GameObject cartaPrefab;
//    public List<MarineroShopping> marinerosDisponibles = new List<MarineroShopping>();

//    [Header("Configuración Detalle (Derecha)")]
//    public GameObject panelDetalle; // Para activar/desactivar si no hay selección
//    public TMP_Text detalleNombre;
//  //  public TMP_Text detalleDescripcion;
//  //  public TMP_Text detallePrecio;
//    public Button detalleBotonComprar;

//    private MarineroShopping seleccionActual;
//    public int monedaJugador = 500;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    void Start()
//    {
//        //// Configurar botón de compra del panel derecho una única vez
//        //if (detalleBotonComprar != null)
//        //{
//        //    detalleBotonComprar.onClick.AddListener(ComprarSeleccionado);
//        //}

//        //// (Tu código de poblar lista sigue aquí...)
//        //CargarDatosEjemplo();
//        //LlenarShop();

//        //// Opcional: Seleccionar el primero por defecto o ocultar el panel
//        //if (marinerosDisponibles.Count > 0)
//        //    SeleccionarMarinero(marinerosDisponibles[0]);
//        //else
//        //    panelDetalle.SetActive(false);
//        // Ya no creamos datos aquí. Los leeremos del Inspector.
//        // Solo configuramos el botón y llenamos la tienda.

//        if (detalleBotonComprar != null)
//            detalleBotonComprar.onClick.AddListener(ComprarSeleccionado);

//        LlenarShop(); // Usará la lista que llenes manualmente en Unity

//        if (marinerosDisponibles.Count > 0)
//            SeleccionarMarinero(marinerosDisponibles[0]);
//        else if (panelDetalle != null)
//            panelDetalle.SetActive(false);
//    }

//    void CargarDatosEjemplo()
//    {
//        marinerosDisponibles.Clear();
//        marinerosDisponibles.Add(new MarineroShopping { nombre = "Pistolero", ventajasGenerales = "Antiaéreo", descripcion = "Daña unidades voladoras...", precio = 120 });
//        marinerosDisponibles.Add(new MarineroShopping { nombre = "Apostador", ventajasGenerales = "Economía", descripcion = "Gana monedas extra...", precio = 200 });
//        marinerosDisponibles.Add(new MarineroShopping { nombre = "Capitán", ventajasGenerales = "Defensa", descripcion = "Otorga escudo...", precio = 300 });
//        marinerosDisponibles.Add(new MarineroShopping { nombre = "Explorador", ventajasGenerales = "Visión", descripcion = "Revela zonas ocultas...", precio = 180 });
//    }

//    public void LlenarShop()
//    {
//        foreach (Transform child in container) Destroy(child.gameObject);
//        foreach (var m in marinerosDisponibles)
//        {
//            var go = Instantiate(cartaPrefab, container);
//            var ui = go.GetComponent<CartaMarineroUI>();
//            ui.ConfigurarCarta(m);
//        }
//    }

//    // --- NUEVA LÓGICA DE SELECCIÓN ---

//    public void SeleccionarMarinero(MarineroShopping m)
//    {
//        seleccionActual = m;
//        panelDetalle.SetActive(true);

//        // Actualizar UI del panel derecho
//        detalleNombre.text = m.nombre;
//       // detalleVentajas.text = m.ventajasGenerales;
//      //  detalleDescripcion.text = m.descripcion;
//      //  detallePrecio.text = "$ " + m.precio.ToString();
//    }

//    public void ComprarSeleccionado()
//    {
//        if (seleccionActual != null)
//        {
//            IntentarComprar(seleccionActual);
//        }
//    }

//    public void IntentarComprar(MarineroShopping m)
//    {
//        if (monedaJugador >= m.precio)
//        {
//            monedaJugador -= m.precio;
//            ProcesarCompra(m);
//            Debug.Log($"Comprado: {m.nombre} - Moneda restante: {monedaJugador}");
//        }
//        else
//        {
//            MostrarErrorNoSuficienteMoneda();
//        }
//    }

//    void ProcesarCompra(MarineroShopping m)
//    {
//        Debug.Log($"Procesando compra de: {m.nombre}");
//        // Aquí tu lógica de desbloqueo
//    }

//    void MostrarErrorNoSuficienteMoneda()
//    {
//        Debug.Log("No tienes suficiente dinero.");
//    }
//}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Configuración Lista (Izquierda)")]
    public Transform container;
    public GameObject cartaPrefab;
    // Esta lista la llenas desde el Inspector de Unity
    public List<MarineroShopping> marinerosDisponibles = new List<MarineroShopping>();

    [Header("Configuración Detalle (Derecha)")]
    public GameObject panelDetalle;

    // VARIABLES DESCOMENTADAS Y AGREGADAS
    public TMP_Text detalleNombre;
    public TMP_Text detalleVentajas;    // <--- Agregado
    public TMP_Text detalleDescripcion; // <--- Descomentado
    public TMP_Text detallePrecio;      // <--- Descomentado
    public Image detalleRetrato;        // <--- Agregado (opcional, para ver la foto en grande)

    public Button detalleBotonComprar;

    private MarineroShopping seleccionActual;
    public int monedaJugador = 500;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (detalleBotonComprar != null)
            detalleBotonComprar.onClick.AddListener(ComprarSeleccionado);

        LlenarShop();

        // Si hay marineros, seleccionamos el primero por defecto
        if (marinerosDisponibles.Count > 0)
        {
            SeleccionarMarinero(marinerosDisponibles[0]);
        }
        else if (panelDetalle != null)
        {
            panelDetalle.SetActive(false);
        }

        // Actualizar estado del botón inicial (si alcanza la plata o no)
        ActualizarBotonCompra();
    }

    public void LlenarShop()
    {
        // Limpiar contenedor por si acaso
        foreach (Transform child in container) Destroy(child.gameObject);

        foreach (var m in marinerosDisponibles)
        {
            var go = Instantiate(cartaPrefab, container);
            var ui = go.GetComponent<CartaMarineroUI>();
            ui.ConfigurarCarta(m);
        }
    }

    // --- LÓGICA DE SELECCIÓN ---

    public void SeleccionarMarinero(MarineroShopping m)
    {
        seleccionActual = m;
        if (panelDetalle != null) panelDetalle.SetActive(true);

        // Actualizar UI del panel derecho (Validamos que no sean null para evitar errores)
        if (detalleNombre != null) StartCoroutine(EscribirTexto(detalleNombre, m.nombre)); // O simplemente .text = m.nombre
        else if (detalleNombre != null) detalleNombre.text = m.nombre;

        if (detalleVentajas != null) detalleVentajas.text = m.ventajasGenerales;
        if (detalleDescripcion != null) detalleDescripcion.text = m.descripcion;
        if (detallePrecio != null) detallePrecio.text = "$ " + m.precio.ToString();

        // Si quieres mostrar la imagen en grande a la derecha también:
        if (detalleRetrato != null && m.retrato != null) detalleRetrato.sprite = m.retrato;

        ActualizarBotonCompra();
    }

    // Opcional: Actualizar visualmente si el botón es interactuable o no según el dinero
    void ActualizarBotonCompra()
    {
        if (detalleBotonComprar != null && seleccionActual != null)
        {
            // Si tienes dinero, el botón es interactuable, si no, se desactiva (o cambia de color)
            detalleBotonComprar.interactable = (monedaJugador >= seleccionActual.precio);
        }
    }

    public void ComprarSeleccionado()
    {
        if (seleccionActual != null)
        {
            IntentarComprar(seleccionActual);
        }
    }

    public void IntentarComprar(MarineroShopping m)
    {
        if (monedaJugador >= m.precio)
        {
            monedaJugador -= m.precio;
            ProcesarCompra(m);
            ActualizarBotonCompra(); // Actualizamos botón tras gastar dinero
            Debug.Log($"Comprado: {m.nombre} - Moneda restante: {monedaJugador}");
        }
        else
        {
            MostrarErrorNoSuficienteMoneda();
        }
    }

    void ProcesarCompra(MarineroShopping m)
    {
        // Lógica futura: Agregar a la lista de "Marineros Desbloqueados" del GameManager
        Debug.Log($"Procesando lógica de desbloqueo para: {m.nombre}");
    }

    void MostrarErrorNoSuficienteMoneda()
    {
        Debug.Log("No tienes suficiente dinero.");
        // Aquí podrías poner una animación de "Dinero en rojo" o un sonido de error
    }

    // Corrutina simple por si quieres efecto de escritura (opcional)
    System.Collections.IEnumerator EscribirTexto(TMP_Text label, string texto)
    {
        label.text = texto;
        yield return null;
    }
}