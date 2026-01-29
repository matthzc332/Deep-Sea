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

    [Header("Vida")]
    public float vida = 5f;

    public Transform barco;


    [Header("Opciones adicionales")]
    public bool debug = true;

    [Header("Prefab de explosión")]
    public GameObject prefabExplosion;
    public float duracionExplosion = 2f;

    [HideInInspector]
    public bool enAreaBarco = false;

    // Referencia al teclado para el nuevo Input System
    private Keyboard keyboard;

    void Start()
    {
        // Obtener referencia al teclado
        GameObject objetoBarco = GameObject.FindGameObjectWithTag("Ship");

    if (objetoBarco != null)
    {
        barco = objetoBarco.transform;

        // 2. Ahora puedes usar 'barco' (que es un Transform) para calcular la dirección
        // Asegúrate de que 'controlledObject' esté asignado en tu script
        Vector3 direccion = (barco.position - transform.position).normalized;
        
        if(debug); //Debug.Log("Barco encontrado en: " + barco.position);
    }
    else
    {
        //Debug.LogError("No se encontró ningún objeto con el Tag 'Ship'");
    }
        keyboard = Keyboard.current;
    }

    void Update()
    {
        // Test: restar vida con D - Usando nuevo Input System
        if (keyboard != null && keyboard.dKey.wasPressedThisFrame)
        {
            vida -= 1f;
            if (debug); //Debug.Log($"Gaviota recibe daño, vida actual: {vida}");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    //    Debug.Log($"=== COLISIÓN DETECTADA ===");
    //Debug.Log($"Objeto: {other.gameObject.name}");
    //Debug.Log($"Tag: {other.tag}");
    //Debug.Log($"Layer: {LayerMask.LayerToName(other.gameObject.layer)}");
    
    if (other.CompareTag("Ship"))
    {
    //    Debug.Log("✅ Gaviota entró en área del barco");
        enAreaBarco = true;
    }
    else
    {
    //    Debug.Log("❌ No es el barco");
    }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ship"))
        {
            enAreaBarco = false;
            if (debug); //Debug.Log("Gaviota salió del área del barco");
        }
    }
}