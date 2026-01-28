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
    public float vida = 1f;

    [Header("Objetivo")]
    public Transform barco;

    [Header("Daño explosión")]
    public int dañoAlBarco = 1;
    public float radioExplosion = 0.8f;

    [Header("Explosión")]
    public GameObject prefabExplosion;
    public float duracionExplosion = 2f;

    [Header("Debug")]
    public bool debug = true;

    [HideInInspector] public bool enAreaBarco = false;

    private Keyboard keyboard;
    private State_Machine stateMachine;
    private bool muerta = false;

    void Start()
    {
        stateMachine = GetComponent<State_Machine>();
        keyboard = Keyboard.current;

        GameObject shipObj = GameObject.FindGameObjectWithTag("Ship");
        if (shipObj != null)
        {
            barco = shipObj.transform;
            if (debug) Debug.Log("Barco asignado a gaviota");
        }
        else
        {
            if (debug) Debug.LogWarning("No se encontró objeto con tag Ship");
        }
    }

    void Update()
    {
        if (muerta) return;

        // TEST daño manual
        if (keyboard != null && keyboard.dKey.wasPressedThisFrame)
        {
            vida -= 1f;
            if (debug) Debug.Log("Vida gaviota: " + vida);
        }

        if (vida <= 0)
            Morir();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship"))
        {
            if (debug) Debug.Log("Gaviota toca barco → explota");
            Morir();
        }
    }

    void Morir()
    {
        if (muerta) return;

        muerta = true;

        if (debug) Debug.Log("Gaviota muere → estado Explotando");

        stateMachine.SetState<Gaviota_Explotando>();
    }
}
