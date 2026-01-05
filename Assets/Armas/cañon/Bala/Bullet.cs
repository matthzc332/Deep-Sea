using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage;
    int pierce;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(
        Vector3 target,
        float initialSpeed,
        int power,
        int pierce
    )
    {
        damage = power;
        this.pierce = pierce;

        Vector2 dir = (target - transform.position).normalized;
        rb.linearVelocity = dir * initialSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        Entity e = other.GetComponent<Entity>();
        if (e != null)
            e.takeDamage(damage);

        if (pierce > 0)
        {
            pierce--;
            return;
        }

        Destroy(gameObject);
    }
}

