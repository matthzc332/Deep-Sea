using UnityEngine;

public class Cofre : Entity
{
    [SerializeField] private GameObject efecto;
    [SerializeField] private float cantidadPuntos = 100f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (puntaje.instancia != null)
            {
                puntaje.instancia.SumarPuntos(cantidadPuntos);
            }

            if (efecto != null)
            {
                Instantiate(efecto, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (speed <= 0)
            speed = 2.0f;

        transform.position = new Vector3(
            -15f,
            transform.position.y,
            transform.position.z
        );
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }
}