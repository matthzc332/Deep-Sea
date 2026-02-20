using UnityEngine;

public class Globo : Entity
{
    [Header("Explosion")]
    public float duracionExplosion = 2f;

    void Start()
    {
        speed = 0.9f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            collision_with_ship = true;
        }
    }
}