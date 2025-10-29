using UnityEngine;
using UnityEngine.UI; // o TMPro si usás TextMeshPro

public class CartaMarineroUI : MonoBehaviour
{
    public Text nombreText;
    public Text ventajasText;
    public Text descripcionText;
    public Text precioText;
    public Button comprarButton; // el diseñador puede asignar el botón del prefab
    private MarineroShopping marineroActual;

    public void ConfigurarCarta(MarineroShopping marinero)
    {
        marineroActual = marinero;
        nombreText.text = marinero.nombre;
        ventajasText.text = marinero.ventajasGenerales;
        descripcionText.text = marinero.descripcion;
        precioText.text = marinero.precio.ToString();
        comprarButton.onClick.RemoveAllListeners();
        comprarButton.onClick.AddListener(() => OnComprarClicked());
    }

    void OnComprarClicked()
    {
        ShopManager.Instance.IntentarComprar(marineroActual);
    }
}