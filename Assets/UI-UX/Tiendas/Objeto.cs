using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Objeto : MonoBehaviour
{
    public enum TipoVista { Carta, Inspeccion }
    [SerializeField] private TipoVista tipoVista = TipoVista.Carta;
    
    [Header("UI Común")]
    public TextMeshProUGUI nombreTxt;
    public TextMeshProUGUI precioTxt;
    public Image iconoMarinero;

    [Header("UI Solo Inspección")]
    public TextMeshProUGUI descTxt;
    public TextMeshProUGUI strongTxt;
    public TextMeshProUGUI weakTxt;

    private ShopManager manager;
    private PlantillaObjeto datos;
    private GameObject cartaOriginalReferencia;

    public void ConfigurarObjeto(PlantillaObjeto data, ShopManager m, GameObject cartaOriginal = null)
    {
        datos = data;
        manager = m;
        cartaOriginalReferencia = cartaOriginal;

        if (data == null) return;

        if (nombreTxt) nombreTxt.text = data.nombre;
        if (precioTxt) precioTxt.text = "$" + data.precio;
        if (iconoMarinero && data.idleAnimationSprites.Length > 0) 
            iconoMarinero.sprite = data.idleAnimationSprites[0];

        if (tipoVista == TipoVista.Inspeccion)
        {
            ActualizarTextoOpcional(descTxt, data.descripcion);
            ActualizarTextoOpcional(strongTxt, data.strongWith, "Fuerte contra: ");
            ActualizarTextoOpcional(weakTxt, data.weakWith, "Débil contra: ");
        }

        if (tipoVista == TipoVista.Carta)
        {
            Button btn = GetComponent<Button>();
            if (btn)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => {
                    if (manager is TiendaModular tm) tm.SeleccionarObjetoModular(datos, this.gameObject);
                });
            }
        }
    }

    private void ActualizarTextoOpcional(TextMeshProUGUI campo, string contenido, string prefijo = "")
    {
        if (campo == null) return;
        if (!string.IsNullOrEmpty(contenido))
        {
            campo.gameObject.SetActive(true);
            campo.text = prefijo + contenido;
        }
        else campo.gameObject.SetActive(false);
    }

    // Getters para el BuyButton
    public PlantillaObjeto GetDatos() => datos;
    public ShopManager GetManager() => manager;
    public GameObject GetCartaOriginal() => cartaOriginalReferencia;
}