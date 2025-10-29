// MoveRect.cs - Cambiar a clase normal
using UnityEngine;

public class MoveRect
{
    // Mueve un objeto en línea recta hacia posicionDestino a cierta velocidad
    public void MovimientoRecto(Transform objeto, Vector3 posicionDestino, float velocidad)
    {
        // comprobación robusta por si hay errores por punto flotante
        if (Vector3.Distance(objeto.position, posicionDestino) > 0.001f)
        {
            objeto.position = Vector3.MoveTowards(
                objeto.position,
                posicionDestino,
                velocidad * Time.deltaTime
            );
        }
    }
}