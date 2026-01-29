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
    public ShipData shipData; // Arrastra el ScriptableObject aquí (Opcional)

    [Header("Economía (Manual si no hay ShipData)")]
    public int monedaJugador = 500;

    void Awake() 
    {
        // Al cargar, si existe ShipData, extraemos su dinero
        if (shipData != null)
        {
            monedaJugador = shipData.dinero;
            Debug.Log("Dinero cargado desde ShipData: " + monedaJugador);
        }
        else
        {
            Debug.Log("No hay ShipData. Usando dinero manual: " + monedaJugador);
        }

        ActualizarUI();
    }

    public virtual void ActualizarUI() 
    { 
        if (textoMonedas != null) textoMonedas.text = $"Monedas: {monedaJugador}"; 
    }

    public virtual void ConfirmarVenta(PlantillaObjeto objeto, GameObject cartaVisual)
    {
        monedaJugador -= objeto.precio;

        // Si existe ShipData, sincronizamos la resta para que persista
        if (shipData != null)
        {
            shipData.dinero = monedaJugador;
        }

        ActualizarUI();

        if (objeto != null && !objeto.esPermanente)
        {
            if (cartaVisual != null) Destroy(cartaVisual);
        }
        else
        {
            Debug.Log("Objeto permanente comprado: La carta permanece en la tienda.");
        }
    }
}