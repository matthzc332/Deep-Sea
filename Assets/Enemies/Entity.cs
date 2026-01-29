using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    [SerializeField] public float HP;
    [SerializeField] protected float speed;
    [SerializeField] protected bool isAlive = true;

    protected bool collision_with_ship = false;

    public float getSpeed() { return speed; }
    public float getHP() { return HP; }
    public bool getIsAlive() { return isAlive; }
    public bool getCollisionWithShip() { return collision_with_ship; }

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (GameManager.instance != null)
        {
            HP += GameManager.difficultyLevel;
        }
    }

    public virtual void takeDamage(int damage)
    {
        if (isAlive)
        {
            HP -= damage;

            if (HP > 0)
            {
                StartCoroutine(DamageEffectRoutine());
            }
            else
            {
                isAlive = false;
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator DamageEffectRoutine()
    {
        if (spriteRenderer == null) yield break;
        spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f); 
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = originalColor;
    }

    // MÉTODO CORREGIDO: Sin lógica de balas para evitar fuego amigo
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // La lógica de daño por balas ahora vive solo en Bullet.cs
        
        if (collision.CompareTag("Ship"))
        {
            collision_with_ship = true;
        }
    }
}