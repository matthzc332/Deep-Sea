using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Referencias UI Base")]
    public GameObject cartaPrefab;
    public GameObject inspeccionPrefab;
    public Transform inspeccionContainer;
    public TextMeshProUGUI textoMonedas;

    [Header("Economía")]
    public int monedaJugador = 500;

    void Awake() => ActualizarUI();

    public virtual void ActualizarUI() 
    { 
        if (textoMonedas != null) textoMonedas.text = $"Monedas: {monedaJugador}"; 
    }

    public virtual void ConfirmarVenta(PlantillaObjeto objeto, GameObject cartaVisual)
{
    monedaJugador -= objeto.precio;
    ActualizarUI();

    // SOLO destruimos la carta si NO es permanente
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