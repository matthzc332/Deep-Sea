using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform baseTimon;
    public RectTransform mango;

    public float velocidadMinima = 100f;
    public float factorGrados = 2f;

    public static bool estoyTocando = false; // Se mantiene igual para tu script de disparo
    private float tiempoBloqueo = 0f; // Nueva variable interna para el delay

    [SerializeField] public float angulo = 0;
    private float gradosAcumulados = 0;
    private float anguloAnterior;

    public void OnPointerDown(PointerEventData e)
    {
        estoyTocando = true;
        // En lugar de una corrutina que cambie el bool, 
        // seteamos un tiempo en el futuro hasta el cual no se puede disparar.
        tiempoBloqueo = Time.time + 0.1f;

        anguloAnterior = CalcularAngulo(e);
    }

    public void OnDrag(PointerEventData e)
    {
        float anguloActual = CalcularAngulo(e);
        float diferencia = anguloActual - anguloAnterior;

        if (diferencia > 180) diferencia -= 360;
        if (diferencia < -180) diferencia += 360;

        angulo += diferencia;
        gradosAcumulados += diferencia;

        angulo = Mathf.Clamp(angulo, -170f, 170f);
        gradosAcumulados = Mathf.Clamp(gradosAcumulados, -170f, 170f);

        anguloAnterior = anguloActual;
        mango.localRotation = Quaternion.Euler(0, 0, angulo);
    }

    public void OnPointerUp(PointerEventData e)
    {
        estoyTocando = false;
    }

    void Update()
    {
        // El timón solo vuelve si REALMENTE soltaste el dedo
        if (!estoyTocando && Mathf.Abs(gradosAcumulados) > 0.01f)
        {
            float direccion = gradosAcumulados > 0 ? 1 : -1;
            float velocidad = velocidadMinima + Mathf.Abs(gradosAcumulados) * factorGrados;

            float paso = velocidad * Time.deltaTime;
            if (paso > Mathf.Abs(gradosAcumulados)) paso = Mathf.Abs(gradosAcumulados);

            angulo -= paso * direccion;
            gradosAcumulados -= paso * direccion;

            mango.localRotation = Quaternion.Euler(0, 0, angulo);
        }
    }

    // MÉTODO PARA TU SCRIPT DE DISPARO (Shoot.cs)
    // En Shoot.cs, donde preguntás por Joystick.estoyTocando, 
    // ahora deberías preguntar por Joystick.PuedeDisparar()
    public static bool PuedeDisparar()
    {
        // Solo dispara si no estamos tocando el joystick 
        // O si ya pasó el tiempo de bloqueo de 0.1s
        return !estoyTocando || Time.time > FindObjectOfType<Joystick>().tiempoBloqueo;
    }

    float CalcularAngulo(PointerEventData e)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(baseTimon, e.position, e.pressEventCamera, out pos);
        return Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
    }
}