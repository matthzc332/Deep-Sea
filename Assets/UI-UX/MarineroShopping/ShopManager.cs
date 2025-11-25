//using UnityEngine;
//using System.Collections.Generic;

//public class ShopManager : MonoBehaviour
//{
//    public static ShopManager Instance { get; private set; }

//    public Transform container;
//    public GameObject cartaPrefab; // el diseñador arrastra el prefab aquí cuando esté listo
//    public List<MarineroShopping> marinerosDisponibles = new List<MarineroShopping>();

//    public int monedaJugador = 500; // ejemplo inicial

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    void Start()
//    {
//        // Ejemplo: agregar marineros a la lista (puedes poblarla desde el inspector) no se si va
//        //aca o en el panel de unity
//        marinerosDisponibles.Clear();

//        marinerosDisponibles.Add(new MarineroShopping
//        {
//            nombre = "Pistolero",
//            ventajasGenerales = "Antiaéreo",
//            descripcion = "Daña unidades voladoras",
//            precio = 120
//        });

//        marinerosDisponibles.Add(new MarineroShopping
//        {
//            nombre = "Apostador",
//            ventajasGenerales = "Economía",
//            descripcion = "Gana monedas extra por ronda",
//            precio = 200
//        });

//        marinerosDisponibles.Add(new MarineroShopping
//        {
//            nombre = "Capitán",
//            ventajasGenerales = "Defensa",
//            descripcion = "Otorga escudo a aliados cercanos",
//            precio = 300
//        });

//        marinerosDisponibles.Add(new MarineroShopping
//        {
//            nombre = "Explorador",
//            ventajasGenerales = "Visión",
//            descripcion = "Revela zonas ocultas del mapa",
//            precio = 180
//        });

//        LlenarShop();


//    }


//    //

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
//            Debug.LogWarning($"No alcanza la moneda para {m.nombre}. Tenés: {monedaJugador}, cuesta: {m.precio}");

//        }
//    }

//    void ProcesarCompra(MarineroShopping m)
//    {
//        // lógica: añadir al inventario, desbloquear en juego, etc.
//        Debug.Log($"Comprado: {m.nombre}");
//    }

//    void MostrarErrorNoSuficienteMoneda()
//    {
//        Debug.Log("Moneda insuficiente");
//    }







//}

using UnityEngine;
using UnityEngine.UI; // Necesario para manipular la UI
using System.Collections.Generic;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Configuración Lista (Izquierda)")]
    public Transform container;
    public GameObject cartaPrefab;
    public List<MarineroShopping> marinerosDisponibles = new List<MarineroShopping>();

    [Header("Configuración Detalle (Derecha)")]
    public GameObject panelDetalle; // Para activar/desactivar si no hay selección
    public TMP_Text detalleNombre;
  //  public TMP_Text detalleDescripcion;
  //  public TMP_Text detallePrecio;
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
        //// Configurar botón de compra del panel derecho una única vez
        //if (detalleBotonComprar != null)
        //{
        //    detalleBotonComprar.onClick.AddListener(ComprarSeleccionado);
        //}

        //// (Tu código de poblar lista sigue aquí...)
        //CargarDatosEjemplo();
        //LlenarShop();

        //// Opcional: Seleccionar el primero por defecto o ocultar el panel
        //if (marinerosDisponibles.Count > 0)
        //    SeleccionarMarinero(marinerosDisponibles[0]);
        //else
        //    panelDetalle.SetActive(false);
        // Ya no creamos datos aquí. Los leeremos del Inspector.
        // Solo configuramos el botón y llenamos la tienda.

        if (detalleBotonComprar != null)
            detalleBotonComprar.onClick.AddListener(ComprarSeleccionado);

        LlenarShop(); // Usará la lista que llenes manualmente en Unity

        if (marinerosDisponibles.Count > 0)
            SeleccionarMarinero(marinerosDisponibles[0]);
        else if (panelDetalle != null)
            panelDetalle.SetActive(false);
    }

    void CargarDatosEjemplo()
    {
        marinerosDisponibles.Clear();
        marinerosDisponibles.Add(new MarineroShopping { nombre = "Pistolero", ventajasGenerales = "Antiaéreo", descripcion = "Daña unidades voladoras...", precio = 120 });
        marinerosDisponibles.Add(new MarineroShopping { nombre = "Apostador", ventajasGenerales = "Economía", descripcion = "Gana monedas extra...", precio = 200 });
        marinerosDisponibles.Add(new MarineroShopping { nombre = "Capitán", ventajasGenerales = "Defensa", descripcion = "Otorga escudo...", precio = 300 });
        marinerosDisponibles.Add(new MarineroShopping { nombre = "Explorador", ventajasGenerales = "Visión", descripcion = "Revela zonas ocultas...", precio = 180 });
    }

    public void LlenarShop()
    {
        foreach (Transform child in container) Destroy(child.gameObject);
        foreach (var m in marinerosDisponibles)
        {
            var go = Instantiate(cartaPrefab, container);
            var ui = go.GetComponent<CartaMarineroUI>();
            ui.ConfigurarCarta(m);
        }
    }

    // --- NUEVA LÓGICA DE SELECCIÓN ---

    public void SeleccionarMarinero(MarineroShopping m)
    {
        seleccionActual = m;
        panelDetalle.SetActive(true);

        // Actualizar UI del panel derecho
        detalleNombre.text = m.nombre;
       // detalleVentajas.text = m.ventajasGenerales;
      //  detalleDescripcion.text = m.descripcion;
      //  detallePrecio.text = "$ " + m.precio.ToString();
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
            Debug.Log($"Comprado: {m.nombre} - Moneda restante: {monedaJugador}");
        }
        else
        {
            MostrarErrorNoSuficienteMoneda();
        }
    }

    void ProcesarCompra(MarineroShopping m)
    {
        Debug.Log($"Procesando compra de: {m.nombre}");
        // Aquí tu lógica de desbloqueo
    }

    void MostrarErrorNoSuficienteMoneda()
    {
        Debug.Log("No tienes suficiente dinero.");
    }
}
