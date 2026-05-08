using UnityEngine;
using UnityEngine.InputSystem;

public class Gaviota : Entity
{
    [Header("Velocidades")]
    public float velocidadNormal = 7f;
    public float velocidadPicada = 13f;

    [Header("Límites de vuelo")]
    public float limiteXIzquierda = -10f;
    public float limiteXDerecha = 10f;

    public Transform barco;

    [Header("Opciones adicionales")]
    public bool debug = true;

    [Header("Prefab de explosión")]
    public GameObject prefabExplosion;
    public float duracionExplosion = 2f;

    [Header("Daño explosión")]
    public float radioExplosion = 0.8f;
    public int dañoAlBarco = 1;

    [HideInInspector]
    public bool enAreaBarco = false;

    private Keyboard keyboard;

    void Start()
    {
        GameObject objetoBarco = GameObject.FindGameObjectWithTag("Ship");

        if (objetoBarco != null)
        {
            barco = objetoBarco.transform;

            if (debug)
                Debug.Log("Barco encontrado en: " + barco.position);
        }
        else
        {
            if (debug)
                Debug.LogWarning("No se encontró ningún objeto con el Tag 'Ship'");
        }

        keyboard = Keyboard.current;
    }

    void Update()
    {
        // Test: restar vida con D usando el sistema base
        if (keyboard != null && keyboard.dKey.wasPressedThisFrame)
        {
            takeDamage(1);

            if (debug)
                Debug.Log($"Gaviota recibe daño, HP actual: {getHP()}");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship"))
        {
            enAreaBarco = true;

            // Si quieres que explote AL TOCARLO (en lugar de solo entrar en modo picada)
            // Puedes forzar el cambio de estado aquí mismo:
            var stateMachine = GetComponentInChildren<State_Machine>(); // O como lo tengas referenciado
            if (stateMachine != null)
            {
                stateMachine.SetState<Gaviota_Explotando>();
            }

            if (debug) Debug.Log("Gaviota colisionó con el barco - EXPLOSIÓN");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ship"))
        {
            enAreaBarco = false;

            if (debug)
                Debug.Log("Gaviota salió del área del barco");
        }
    }
}