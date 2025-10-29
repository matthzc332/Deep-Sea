using UnityEngine;
using UnityEngine.EventSystems;


public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform baseTimon;   // La base del timón
    public RectTransform mango;        // El timón que vamos a girar

    public float velocidadMinima = 100f;  // velocidad base de regreso
    public float factorGrados = 2f;       // cuánto aumenta la velocidad según cuánto giraste

    private bool estoyTocando = false;
    [SerializeField] public float angulo = 0;           // ángulo actual
    private float gradosAcumulados = 0; // cuánto giré desde que agarre el timón
    private float anguloAnterior;       // para guardar el ángulo del frame anterior

    // Cuando empiezo a tocar
    public void OnPointerDown(PointerEventData e)
    {
        estoyTocando = true;
        anguloAnterior = CalcularAngulo(e); // guardo el ángulo inicial
    }

    // Mientras arrastro
    public void OnDrag(PointerEventData e)
    {
        if (!estoyTocando) return;

        // calculo ángulo actual
        float anguloActual = CalcularAngulo(e);

        // diferencia entre el último y este
        float diferencia = anguloActual - anguloAnterior;

        // esto es para que funcione cuando pasa de 180 a -180
        if (diferencia > 180) diferencia -= 360;
        if (diferencia < -180) diferencia += 360;

        // sumo al ángulo del timón
        angulo += diferencia;
        gradosAcumulados += diferencia;


        //Limito
        angulo = Mathf.Clamp(angulo, -170f, 170f);
        gradosAcumulados = Mathf.Clamp(gradosAcumulados, -170f, 170f);


        // guardo para el próximo frame
        anguloAnterior = anguloActual;

        // aplico la rotación
        mango.localRotation = Quaternion.Euler(0, 0, angulo);
    }

    // Cuando dejo de tocar
    public void OnPointerUp(PointerEventData e)
    {
        estoyTocando = false;
    }

    void Update()
    {
        // si no estoy tocando y hay grados acumulados, aplico retroceso
        if (!estoyTocando && Mathf.Abs(gradosAcumulados) > 0.01f)
        {
            float direccion = gradosAcumulados > 0 ? 1 : -1;

            // velocidad proporcional al giro acumulado
            float velocidad = velocidadMinima + Mathf.Abs(gradosAcumulados) * factorGrados;

            float paso = velocidad * Time.deltaTime;
            if (paso > Mathf.Abs(gradosAcumulados)) paso = Mathf.Abs(gradosAcumulados);

            angulo -= paso * direccion;
            gradosAcumulados -= paso * direccion;

            mango.localRotation = Quaternion.Euler(0, 0, angulo);
        }
    }

    // Función simple para calcular el ángulo desde el centro
    float CalcularAngulo(PointerEventData e)
    {
        Vector2 pos;
        // obtenemos la posición local del toque dentro del timón
        RectTransformUtility.ScreenPointToLocalPointInRectangle(baseTimon, e.position, e.pressEventCamera, out pos);
        // calculamos el ángulo usando Atan2
        return Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
    }
}