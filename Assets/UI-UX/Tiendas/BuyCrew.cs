using UnityEngine;

public class BuyCrew : MonoBehaviour
{
    public void IntentarIniciarCompra()
    {
        // Buscamos el componente Objeto en el padre (el prefab de inspección)
        Objeto objetoPadre = GetComponentInParent<Objeto>();

        if (objetoPadre == null) return;

        PlantillaObjeto datos = objetoPadre.GetDatos();
        ShopManager tienda = objetoPadre.GetManager();
        GameObject cartaOriginal = objetoPadre.GetCartaOriginal();

        if (datos == null || tienda == null) return;

        if (tienda.monedaJugador >= datos.precio)
        {
            ShipPlacementManager placement = Object.FindFirstObjectByType<ShipPlacementManager>();
            if (placement != null)
            {
                placement.AbrirSeleccionDePosicion(datos, tienda, cartaOriginal);
            }
        }
        else Debug.Log("No hay suficiente dinero.");
    }
}