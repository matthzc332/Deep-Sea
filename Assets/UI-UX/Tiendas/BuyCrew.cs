// using UnityEngine;

// public class BuyCrew : MonoBehaviour
// {
//     public void IntentarIniciarCompra()
//     {
//         // Buscamos el componente Objeto en el padre (el prefab de inspección)
//         Objeto objetoPadre = GetComponentInParent<Objeto>();

//         if (objetoPadre == null) return;

//         PlantillaObjeto datos = objetoPadre.GetDatos();
//         ShopManager tienda = objetoPadre.GetManager();
//         GameObject cartaOriginal = objetoPadre.GetCartaOriginal();

//         if (datos == null || tienda == null) return;

//         if (tienda.monedaJugador >= datos.precio)
//         {
//             ShipPlacementManager placement = Object.FindFirstObjectByType<ShipPlacementManager>();
//             if (placement != null)
//             {
//                 placement.AbrirSeleccionDePosicion(datos, tienda, cartaOriginal);
//             }
//         }
//         else Debug.Log("No hay suficiente dinero.");
//     }
// }

using UnityEngine;

public class BuyCrew : MonoBehaviour
{
    [Header("Aviso de UI")]
    public GameObject cartelLleno; // Referencia al panel de aviso

    // public void IntentarIniciarCompra()
    // {
    //     // 1. Buscamos al manager en la jerarquía
    //     ShipPlacementManager placement = Object.FindFirstObjectByType<ShipPlacementManager>();

    //     // 2. BLOQUEO: Si el manager dice que no hay espacio, activamos aviso y salimos
    //     if (placement != null && !placement.TieneEspacioDisponible())
    //     {
    //         if (cartelLleno != null) cartelLleno.SetActive(true);
    //         Debug.Log("Compra bloqueada: Barco lleno.");
    //         return;
    //     }
    public void IntentarIniciarCompra()
    {
        // 1. Buscamos al manager en la jerarquía
        ShipPlacementManager placement = Object.FindFirstObjectByType<ShipPlacementManager>();

        // 2. BLOQUEO: Si el manager dice que no hay espacio, activamos aviso y salimos
        if (placement != null && !placement.TieneEspacioDisponible())
        {
            if (cartelLleno != null) cartelLleno.SetActive(true);
            Debug.Log("Compra bloqueada: Barco lleno.");
            return;
        }

        // 3. Obtenemos el componente Objeto del padre (el prefab de inspección)
        Objeto objetoPadre = GetComponentInParent<Objeto>();
        if (objetoPadre == null) return;

        // 4. Extraemos los datos necesarios del objeto
        PlantillaObjeto datos = objetoPadre.GetDatos();
        ShopManager tienda = objetoPadre.GetManager();
        GameObject cartaOriginal = objetoPadre.GetCartaOriginal();

        if (datos == null || tienda == null) return;

        // 5. Verificamos economía y procedemos a la selección de posición
        if (tienda.monedaJugador >= datos.precio)
        {
            if (placement != null)
            {
                placement.AbrirSeleccionDePosicion(datos, tienda, cartaOriginal);
            }
        }
        else 
        {
            Debug.Log("No hay suficiente dinero.");
        }
    }
}