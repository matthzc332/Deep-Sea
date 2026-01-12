using UnityEngine;

public class BuyDefault : MonoBehaviour
{
    public void ComprarDirecto()
{
    Objeto objetoPadre = GetComponentInParent<Objeto>();
    if (objetoPadre == null) return;

    PlantillaObjeto datos = objetoPadre.GetDatos();
    ShopManager tienda = objetoPadre.GetManager();
    GameObject cartaOriginal = objetoPadre.GetCartaOriginal();

    if (datos == null || tienda == null) return;

    if (tienda.monedaJugador >= datos.precio)
    {
        // 1. Procesar la venta
        tienda.ConfirmarVenta(datos, cartaOriginal);

        // 2. Lógica de cierre de inspección
        if (!datos.esPermanente)
        {
            // Si es un marinero o consumible, cerramos la inspección
            Destroy(objetoPadre.gameObject);
        }
        else
        {
            // Si es una habilidad permanente, quizás solo queremos 
            // dar un feedback visual de "Comprado" sin cerrar la ventana.
            Debug.Log("Habilidad adquirida!");
        }
    }
}
}