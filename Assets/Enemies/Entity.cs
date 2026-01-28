using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField]
    public int HP;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected bool isAlive = true;


    protected bool collision_with_ship = false;

    public float getSpeed() { return speed; }
    public float getHP() { return HP; }
    public bool getIsAlive() { return isAlive; }
    public bool getCollisionWithShip() { return collision_with_ship; }

    protected virtual void Awake()
    {
        if (GameManager.instance != null)
        {
            HP += GameManager.difficultyLevel;
        }
    }

    public virtual void takeDamage(int damage)
    {
        if (!isAlive) return;

        HP -= damage;

        if (HP <= 0)
        {
            isAlive = false;

            // 🔥 EN VEZ DE DESTRUIR, PASAMOS AL ESTADO DE MUERTE
            State_Machine sm = GetComponent<State_Machine>();

            if (sm != null)
            {
                sm.SetState<Gaviota_Explotando>();
            }
            else
            {
                // Si no tiene máquina de estados, se destruye normal
                Destroy(gameObject);
            }
        }
    }


    // Método que se ejecuta cuando ocurre una colisión con trigger 2D
    protected virtual void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null) // Pequeña seguridad extra
            {
                takeDamage(bullet.getDamage());

            }


        }
        else if (collision.CompareTag("Ship"))
        {
            collision_with_ship = true;
        }
    }

}