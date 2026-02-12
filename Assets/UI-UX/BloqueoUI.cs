using UnityEngine;
using UnityEngine.EventSystems;

public class BloqueoUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Variable estática que consultaremos desde Shoot.cs
    public static bool TocandoBoton { get; private set; } = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        TocandoBoton = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TocandoBoton = false;
    }

    private void OnDisable()
    {
        // Si el botón se oculta (ej. cerrar tienda), liberamos el bloqueo
        TocandoBoton = false;
    }
}