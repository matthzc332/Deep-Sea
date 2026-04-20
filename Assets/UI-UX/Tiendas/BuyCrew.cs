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
using DG.Tweening;
using UnityEngine.UI;

public class BuyCrew : MonoBehaviour
{
    [Header("Aviso de UI")]
    public GameObject cartelLleno;

    private Button miBoton;
    private Image imagenBoton;
public ShipData shipData;
    void Awake()
    {
        miBoton = GetComponent<Button>();
        imagenBoton = GetComponent<Image>();
    }

    void OnEnable()
    {
        ActualizarEstadoBoton();
    }

    public void IntentarIniciarCompra()
    {
        ShipPlacementManager placement = Object.FindFirstObjectByType<ShipPlacementManager>();

        // 1. Verificamos si hay espacio
        if (placement != null && !placement.TieneEspacioDisponible())
        {
            // HACER QUE EL CONTADOR REACCIONE (Vibración)
            placement.FeedbackTextoLleno();

            // Feedback en el propio botón para que el jugador sienta el clic bloqueado
            transform.DOShakePosition(0.3f, 10f).SetUpdate(true);

            Debug.Log("Barco lleno: Reacción del contador activada.");
            return;
        }

        // 2. Si hay espacio, procedemos con la lógica de obtención de datos
        Objeto objetoPadre = GetComponentInParent<Objeto>();
        if (objetoPadre == null) return;

        PlantillaObjeto datos = objetoPadre.GetDatos();
        ShopManager tienda = objetoPadre.GetManager();
        GameObject cartaOriginal = objetoPadre.GetCartaOriginal();

        if (datos == null || tienda == null) return;

        // 3. Verificamos economía y procedemos a la selección de posición
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
            // Opcional: podrías hacer que el texto de dinero vibre aquí también
        }
    }

    public void ActualizarEstadoBoton()
    {
        ShipPlacementManager placement = Object.FindFirstObjectByType<ShipPlacementManager>();
        if (placement == null) return;

        bool hayEspacio = placement.TieneEspacioDisponible();

        if (imagenBoton != null)
        {
            // Si está lleno, se pone gris; si no, blanco normal
            // Usamos DOColor para que el cambio sea suave
            Color colorObjetivo = hayEspacio ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);
            imagenBoton.DOColor(colorObjetivo, 0.3f).SetUpdate(true);
        }

        // Nota: No desactivamos miBoton.interactable para que el clic 
        // siga funcionando y pueda activar la reacción del contador.
    }

    // Ejemplo de lógica a añadir tras confirmar la compra
    public void AplicarEfectoDeCompra(PlantillaObjeto datos)
    {
        if (datos.nombre == "Cargador") // O el ID que uses
        {
            // Accedes al ShipData y aumentas la munición
            shipData.municion += 20;
            Debug.Log("Munición aumentada!");
        }
    }
}