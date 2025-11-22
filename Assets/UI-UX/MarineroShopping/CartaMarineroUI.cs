//using UnityEngine;
//using UnityEngine.UI; // o TMPro si usás TextMeshPro

//public class CartaMarineroUI : MonoBehaviour
//{
//    public Text nombreText;
//    public Text ventajasText;
//    public Text descripcionText;
//    public Text precioText;
//    public Button comprarButton; // el diseñador puede asignar el botón del prefab
//    private MarineroShopping marineroActual;

//    public void ConfigurarCarta(MarineroShopping marinero)
//    {
//        marineroActual = marinero;
//        nombreText.text = marinero.nombre;
//        ventajasText.text = marinero.ventajasGenerales;
//        descripcionText.text = marinero.descripcion;
//        precioText.text = marinero.precio.ToString();
//        comprarButton.onClick.RemoveAllListeners();
//        comprarButton.onClick.AddListener(() => OnComprarClicked());
//    }

//    void OnComprarClicked()
//    {
//        ShopManager.Instance.IntentarComprar(marineroActual);
//    }
//}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartaMarineroUI : MonoBehaviour
{
    public TMP_Text nombreText;
    public Image imagenRetrato;
    // Puedes quitar descripción y ventajas de la carta pequeña si quieres que ocupe menos espacio
    // o dejarlas como resumen.

    public Button seleccionarButton; // Asigna el botón que cubre la carta (o un botón "Ver")
    private MarineroShopping marineroAsignado;

    public void ConfigurarCarta(MarineroShopping marinero)
    {
        marineroAsignado = marinero;

        // Configuramos visuales básicos de la lista
        if (nombreText != null) nombreText.text = marinero.nombre;
        if (imagenRetrato != null && marinero.retrato != null)
            imagenRetrato.sprite = marinero.retrato;
        // Al hacer click, notificamos al Manager que SELECCIONAMOS este marinero
        seleccionarButton.onClick.RemoveAllListeners();
        seleccionarButton.onClick.AddListener(() => {
            ShopManager.Instance.SeleccionarMarinero(marineroAsignado);
        });
    }
}