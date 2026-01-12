using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    protected float HP;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected bool isAlive = true;

    protected bool collision_with_ship = false;



    public float getSpeed() {return speed;}
    public float getHP() {return HP;}
    public bool getIsAlive() {return isAlive;}
    public bool getCollisionWithShip() {return collision_with_ship;}



    // modifica dificultad a la oleada
    protected virtual void Awake()
    {
        // Si existe el GameManager, sumamos vida base + dificultad
        if (GameManager.instance != null)
        {
            // Ejemplo: +1 de vida por cada nivel de dificultad
            // O puedes hacer: HP += GameManager.instance.difficultyLevel * 10;
            HP += GameManager.instance.difficultyLevel;
        }
    }
    public virtual void takeDamage(int damage)
    {
        if (isAlive)
        {
            HP -= damage;
            if (HP <= 0)
            {
                isAlive = false;
                Destroy(gameObject);
            }
        }
    }

    // Método que se ejecuta cuando ocurre una colisión con trigger 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
            //Debug.Log("Detectó la colision");
        // Verificar si el objeto con el que colisionó tiene la etiqueta "Bullet"
        if (collision.CompareTag("Bullet"))
        {
            //Debug.Log("Colisionó con una bala");
            Bullet bullet = collision.GetComponent<Bullet>();
            takeDamage(bullet.getDamage());
            //Debug.Log("Vida Actual:"+ HP);
        }

        else if (collision.CompareTag("Ship"))
        {
            collision_with_ship = true;
        }
    }

}