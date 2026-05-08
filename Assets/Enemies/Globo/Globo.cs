using UnityEngine;

public class Globo : Entity
{
   

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