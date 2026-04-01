using UnityEngine;
using System.Collections; // Necesario para IEnumerator

public class Cofre : Entity
{
    [SerializeField] private GameObject efecto;
    [SerializeField] private float cantidadPuntos = 100f;
    [SerializeField] private int indiceAnimApertura = 1; // Índice de la animación "Open" en el Controller

    private Animation_Controller animController;
    private bool estaAbierto = false;

    void Start()
    {
        animController = GetComponent<Animation_Controller>();

        if (speed <= 0)
            speed = 2.0f;

        // Ajusta este valor (-20f o similar) para que no aparezca de golpe en el barco
        transform.position = new Vector3(-20f, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (estaAbierto) return;

        if (other.CompareTag("Bullet"))
        {
            StartCoroutine(SecuenciaMuerte());
        }
    }

    private IEnumerator SecuenciaMuerte()
    {
        estaAbierto = true;
        
        // 1. Detener el movimiento (opcional, para que se abra en el lugar)
        speed = 0;

        // 2. Sumar puntos
        if (puntaje.instancia != null)
        {
            puntaje.instancia.SumarPuntos(cantidadPuntos);
        }

        // 3. Reproducir animación de apertura
        if (animController != null)
        {
            animController.Play(indiceAnimApertura);
            // Espera hasta que Animation_Controller diga que terminó
            yield return new WaitUntil(() => animController.IsFinished());
        }

        // 4. Mostrar la moneda (efecto)
        if (efecto != null)
        {
            Instantiate(efecto, transform.position, Quaternion.identity);
        }

        // 5. Desaparecer
        // El EnemyDeathNotifier (SpawnLimit) avisará al sistema de oleadas aquí
        Destroy(gameObject);
    }

    void Update()
    {
        // Solo se mueve si no ha sido impactado
        if (!estaAbierto)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (transform.position.x > 15f)
            {
                Destroy(gameObject);
            }
        }
    }
}