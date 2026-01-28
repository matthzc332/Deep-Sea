using UnityEngine;
using System.Collections; // Necesario para usar Corrutinas

public class Entity : MonoBehaviour
{
    [SerializeField]
    public float HP;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected bool isAlive = true;

    protected bool collision_with_ship = false;



    public float getSpeed() {return speed;}
    public float getHP() {return HP;}
    public bool getIsAlive() {return isAlive;}
    public bool getCollisionWithShip() {return collision_with_ship;}


    private SpriteRenderer spriteRenderer;
    private Color originalColor;


    // modifica dificultad a la oleada
    protected virtual void Awake()
    {
        // Guardamos la referencia y el color inicial al empezar
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

            // Iniciamos el efecto visual si no ha muerto
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

        // Cambiar a rojo con menos opacidad (Alpha)
        // Color(R, G, B, A) -> Valores de 0 a 1
        spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f); 

        // Esperar 0.3 segundos
        yield return new WaitForSeconds(0.3f);

        // Volver al color original
        spriteRenderer.color = originalColor;
    }

    // Método que se ejecuta cuando ocurre una colisión con trigger 2D
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
            //Debug.Log("Detectó la colision");
        // Verificar si el objeto con el que colisionó tiene la etiqueta "Bullet"
        if (collision.CompareTag("Bullet"))
        {
            //Debug.Log("Colisionó con una bala");
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null) // Pequeña seguridad extra
            {
            takeDamage(bullet.getDamage());
            //Debug.Log("Vida Actual:"+ HP);
            }
        }

        else if (collision.CompareTag("Ship"))
        {
            collision_with_ship = true;
        }
    }

}