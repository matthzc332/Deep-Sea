using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public int damage;
    public int pierce;
    public float initialSpeed;
    public bool isFromEnemy;
    
    [Header("State Data")]
    public Transform enemy;    // Objetivo como Transform (usado por estados)
    public bool hasHit;        // Bandera para avisar al StateMachine de la colisión

    [HideInInspector] public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 1. Método para inicializar con una posición (Vector3)
    // Se usa en Shoot_Cannon
    public void Initialize(Vector3 targetPos, float speed, int power, int pierceCount)
    {
        this.damage = power;
        
        // CAMBIO CRÍTICO: Usa += para no borrar lo que puso Pierce1
        this.pierce += pierceCount; 
        
        this.initialSpeed = speed;
        this.hasHit = false;

        if (rb == null) rb = GetComponent<Rigidbody2D>();

        Vector2 dir = (targetPos - transform.position).normalized;
        rb.linearVelocity = dir * speed;
        
        OrientToVelocity();
    }

    // Método nuevo para que los scripts de habilidades añadan perforación
    public void AddPierce(int amount)
    {
        this.pierce += amount;
    }

    // 2. Método para inicializar/lanzar hacia un objeto (Transform)
    // Se usa en BulletMoveState
    public void LaunchTowards(Transform target, float speed)
    {
        if (target == null) return;
        
        this.enemy = target;
        this.initialSpeed = speed;

        Vector2 dir = (target.position - transform.position).normalized;
        rb.linearVelocity = dir * speed;
        
        OrientToVelocity();
    }

    // 3. Método para rotar la bala hacia donde se mueve
    public void OrientToVelocity()
    {
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    // 4. Getter para el daño (usado por Entity.cs)
    public int getDamage()
    {
        return damage;
    }

    // 5. Detección de colisiones
    private void OnTriggerEnter2D(Collider2D other)
{
    // SEGURIDAD: Si ya impactó, no procesar nada más
    if (hasHit) return;

    if (isFromEnemy)
    {
        // Si la bala es de un enemigo, IGNORA por completo a otros enemigos o jefes
        if (other.CompareTag("Enemy") || other.CompareTag("Boss")) 
        {
            return; // Aquí salimos y hasHit sigue siendo FALSE
        }
    }
    else
    {
        // Si la bala es del jugador, IGNORA a la nave del jugador
        if (other.CompareTag("Ship")) 
        {
            return;
        }
    }

    // SI LLEGÓ AQUÍ, ES UN IMPACTO REAL
    Entity e = other.GetComponent<Entity>();
    if (e != null)
    {
        e.takeDamage(damage);

        if (pierce > 0)
        {
            pierce--;
        }
        else
        {
            hasHit = true; // Solo aquí se activa la destrucción
        }
    }
}
}