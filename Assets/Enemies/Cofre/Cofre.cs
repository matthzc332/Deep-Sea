using UnityEngine;

public class Cofre : Entity
{
    [SerializeField] private GameObject efecto;
    [SerializeField] private float cantidadPuntos = 100f; // Valor por defecto

    // Ya no necesitamos [SerializeField] private puntaje puntaje;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Accedemos directamente a través de la instancia estática
            if (puntaje.instancia != null)
            {
                puntaje.instancia.SumarPuntos(cantidadPuntos);
            }

            Instantiate(efecto, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (speed <= 0) speed = 2.0f;
        isAlive = true;

        // Ajusta esto según tu necesidad, -15f podría estar muy lejos
        transform.position = new Vector3(-15f, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (isAlive)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        if (transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }
}