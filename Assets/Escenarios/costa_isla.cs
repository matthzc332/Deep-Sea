using UnityEngine;

public class costa_isla : MonoBehaviour
{
    [Header("Referencias")]
    public Transform shipTransform; // Referencia al barco
    public GameManager gameManager;
    
    [Header("Configuración de Movimiento")]
    public float velocidad = 2f;

    private MoveRect moveRect;
    private bool movimientoActivo = false;

    void Start()
    {
        // Crear instancia de MoveRect (ahora es una clase normal)
        moveRect = new MoveRect();
        
        // Buscar el barco por nombre si no se asigna por inspector
        if (shipTransform == null)
        {
            GameObject shipObject = GameObject.Find("ship");
            if (shipObject != null)
                shipTransform = shipObject.transform;
        }

        // Buscar el GameManager si no se asigna por inspector
        if (gameManager == null)
        {
            gameManager = GameManager.instance;
        }
    }

    void OnEnable()
    {
        // Cuando se active el objeto, iniciar movimiento hacia el barco
        movimientoActivo = true;
    }

    void Update()
    {
        // Mover la isla hacia el barco si está activo el movimiento y tenemos referencia al barco
        if (movimientoActivo && shipTransform != null && moveRect != null)
        {
            Vector3 posicionDestino = new Vector3(
                shipTransform.position.x,
                -0.37f, // Usar -0.37f en lugar de la Y del barco
                shipTransform.position.z
            );

            moveRect.MovimientoRecto(transform, posicionDestino, velocidad);
            
            // Opcional: Detener el movimiento cuando esté muy cerca del barco
            if (Vector3.Distance(transform.position, posicionDestino) < 0.1f)
            {
                movimientoActivo = false;
                Debug.Log("Isla llegó al barco");
            }
        }
    }

    // Detectar colisión con el barco
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship"))
        {
            Debug.Log("¡Colisión detectada con el barco!");
            if (gameManager != null)
            {
                gameManager.softTransition();
            }
        }
    }

    // Función para activar/desactivar el movimiento manualmente
    public void ActivarMovimiento(bool activar)
    {
        movimientoActivo = activar;
    }
}