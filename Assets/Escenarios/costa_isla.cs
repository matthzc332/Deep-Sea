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
        // Crear instancia de MoveRect
        moveRect = new MoveRect();

        // Intentamos buscar referencias al inicio
        BuscarReferencias();
    }

    void OnEnable()
    {
        // Cuando se active el objeto, iniciar movimiento hacia el barco
        movimientoActivo = false;
    }

    void Update()
    {
        // 1. SI NO TENEMOS BARCO, LO BUSCAMOS
        if (shipTransform == null)
        {
            BuscarReferencias();
            // Si después de buscar sigue siendo null, no hacemos nada este frame
            if (shipTransform == null) return;
        }

        // 2. LÓGICA DE MOVIMIENTO
        if (movimientoActivo && moveRect != null)
        {
            Vector3 posicionDestino = new Vector3(
                shipTransform.position.x,
                2.5165f, // Usar 2.5165f en lugar de la Y del barco
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

    // Función dedicada a encontrar lo que falta
    void BuscarReferencias()
    {
        // Buscar el barco por su Script si no lo tenemos
        if (shipTransform == null)
        {
            // FindFirstObjectByType es mejor que buscar por nombre
            Ship barcoScript = FindFirstObjectByType<Ship>();
            if (barcoScript != null)
            {
                shipTransform = barcoScript.transform;
            }
        }

        // Buscar el GameManager si no se asigna por inspector
        if (gameManager == null)
        {
            gameManager = GameManager.instance;
            // Fallback por si instance no está listo aún
            if (gameManager == null)
                gameManager = FindFirstObjectByType<GameManager>();
        }
    }

    // Detectar colisión con el barco
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nota: Asegúrate que tu barco tenga el Tag "Ship" o "Player" (según hayas configurado)
        // O mejor aún, verifica si tiene el componente Ship:
        if (other.GetComponent<Ship>() != null || other.CompareTag("Player") || other.CompareTag("Ship"))
        {
            Debug.Log("¡Colisión detectada con el barco!");
            if (gameManager != null)
            {
                gameManager.softTransition();
            }
        }
    }

    public void ActivarMovimiento(bool activar)
    {
        movimientoActivo = activar;
    }
}