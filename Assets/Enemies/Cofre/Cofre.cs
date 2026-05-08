using UnityEngine;

public class Cofre : Entity
{
    [SerializeField] private GameObject efecto;
    [SerializeField] private float cantidadPuntos = 100f;



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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // 1. Sumar puntos
            if (puntaje.instancia != null)
            {
                puntaje.instancia.SumarPuntos(cantidadPuntos);
            }

            // 2. Efecto visual de explosión/partículas
            if (efecto != null)
            {
                Instantiate(efecto, transform.position, Quaternion.identity);
            }

            // 3. ¡LLAMAR AL MÉTODO DE LA CLASE PADRE!
            // Esto activará el parpadeo rojo y pondrá isAlive = false cuando llegue a 0
            takeDamage(1);
        }
    }

}