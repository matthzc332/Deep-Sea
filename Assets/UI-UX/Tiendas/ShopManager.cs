// using UnityEngine;
// using TMPro;

// public class ShopManager : MonoBehaviour
// {
//     [Header("Referencias UI Base")]
//     public GameObject cartaPrefab;
//     public GameObject inspeccionPrefab;
//     public Transform inspeccionContainer;
//     public TextMeshProUGUI textoMonedas;

//     [Header("Persistencia de Datos")]
//     public ShipData shipData; // Arrastra el ScriptableObject aquí (Opcional)

//     [Header("Economía (Manual si no hay ShipData)")]
//     public int monedaJugador = 500;

//     [Header("UI de Espacio")]
//     public TextMeshProUGUI textoEspacio;

//     void Awake()
//     {
//         // Al cargar, si existe ShipData, extraemos su dinero
//         if (shipData != null)
//         {
//             monedaJugador = shipData.dinero;
//             Debug.Log("Dinero cargado desde ShipData: " + monedaJugador);
//         }
//         else
//         {
//             Debug.Log("No hay ShipData. Usando dinero manual: " + monedaJugador);
//         }

//         ActualizarUI();
//     }

//     public virtual void ActualizarUI()
//     {
//         if (textoMonedas != null) textoMonedas.text = $"Monedas: {monedaJugador}";
//     }

//     public virtual void ConfirmarVenta(PlantillaObjeto objeto, GameObject cartaVisual)
//     {
//         monedaJugador -= objeto.precio;

//         // Si existe ShipData, sincronizamos la resta para que persista
//         if (shipData != null)
//         {
//             shipData.dinero = monedaJugador;
//         }

//         ActualizarUI();

//         if (objeto != null && !objeto.esPermanente)
//         {
//             if (cartaVisual != null) Destroy(cartaVisual);
//         }
//         else
//         {
//             Debug.Log("Objeto permanente comprado: La carta permanece en la tienda.");
//         }
//     }
// }

using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Referencias UI Base")]
    public GameObject cartaPrefab;
    public GameObject inspeccionPrefab;
    public Transform inspeccionContainer;
    public TextMeshProUGUI textoMonedas;

    [Header("Persistencia de Datos")]
    public ShipData shipData;

    [Header("Economía (Manual si no hay ShipData)")]
    public int monedaJugador = 500;

    [Header("UI de Espacio")]
    public TextMeshProUGUI textoEspacio; // Referencia al texto 0/2

    void Awake()
    {
        if (shipData != null)
        {
            monedaJugador = shipData.dinero;
        }
        ActualizarUI();
    }

    // ELIMINA EL "override". Usa "virtual" para que sea un solo método consolidado.
    public virtual void ActualizarUI()
    {
        // 1. Actualiza el dinero
        if (textoMonedas != null) textoMonedas.text = $"Monedas: {monedaJugador}";

        // 2. Actualiza el contador 2/2
        // Buscamos al manager que tiene la lista de posiciones
        ShipPlacementManager placement = Object.FindFirstObjectByType<ShipPlacementManager>();

        if (placement != null && textoEspacio != null)
        {
            int ocupados = placement.GetCantidadOcupada(); // Usa el método que cuenta !EstaDisponible()
            int total = placement.posicionesExistentes.Count; // Element 0 y Element 1 de tu lista

            textoEspacio.text = $"Espacio: {ocupados}/{total}";

            // Feedback: Rojo si está lleno
            textoEspacio.color = (ocupados >= total) ? Color.red : Color.white;
        }
    }

    public virtual void ConfirmarVenta(PlantillaObjeto objeto, GameObject cartaVisual)
    {
        monedaJugador -= objeto.precio;
        if (shipData != null) shipData.dinero = monedaJugador;

        ActualizarUI();

        if (objeto != null && !objeto.esPermanente)
        {
            if (cartaVisual != null) Destroy(cartaVisual);
        }
    }
}