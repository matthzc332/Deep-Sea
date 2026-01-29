using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Entity
{
    [Header("Referencias")]
    private Transform targetShip;
    private State_Machine stateMachine;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movimiento")]
    public float verticalSpeed = 2f;
    private bool allowFollow = false;

    [Header("Combate")]
    public float attackInterval = 4f;
    private float nextAttackTimer;

    [Tooltip("Arrastra aquí los GameObjects hijos que tienen los scripts de ataque")]
    public List<State_Base> attackPool;

    void Start()
    {
        // 1. POSICIONAMIENTO INICIAL INMEDIATO
        transform.position = new Vector3(-10f, -0.9f, 0f);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // 2. OBTENER COMPONENTES
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<State_Machine>();
        nextAttackTimer = attackInterval;

        // 3. CONFIGURACIÓN FÍSICA Y ROTACIÓN
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0; // Aseguramos que no caiga si hay gravedad global
        }

        // 4. BÚSQUEDA DEL JUGADOR
        GameObject shipObj = GameObject.FindGameObjectWithTag("Ship");
        if (shipObj != null)
        {
            targetShip = shipObj.transform;
        }

        if (HP <= 0) HP = 50;

        StartCoroutine(EnableFollowAfterDelay(0.5f));
    }

    void Update()
    {
        if (!isAlive || targetShip == null) return;

        // Solo descontamos tiempo si estamos en persecución
        if (stateMachine.GetCurrentState() is BossPursueState)
        {
            nextAttackTimer -= Time.deltaTime;

            if (nextAttackTimer <= 0 && attackPool.Count > 0)
            {
                TriggerRandomAttack();
            }
        }
    }

    void FixedUpdate()
    {
        if (!isAlive || targetShip == null || !allowFollow) return;

        // El movimiento vertical solo ocurre durante la persecución
        if (stateMachine.GetCurrentState() is BossPursueState)
        {
            MoveBoss();
        }
    }


    void MoveBoss()
    {
        float newY = Mathf.Lerp(transform.position.y, targetShip.position.y, verticalSpeed * Time.fixedDeltaTime);
        
        if (rb != null)
        {
            rb.MovePosition(new Vector2(transform.position.x, newY));
        }
        else
        {
            transform.position = new Vector3(transform.position.x, newY, 0);
        }
    }

    void TriggerRandomAttack()
    {
        int randomIndex = Random.Range(0, attackPool.Count);
        State_Base attackState = attackPool[randomIndex];

        if (attackState != null)
        {
            nextAttackTimer = attackInterval;
            stateMachine.SetState(attackState);
        }
    }

    public Transform GetTarget() => targetShip;

    IEnumerator EnableFollowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (stateMachine.GetCurrentState() == null)
        {
            stateMachine.SetState<BossPursueState>();
        }
        allowFollow = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Ship"))
        {
            Ship playerShip = collision.gameObject.GetComponent<Ship>();
            if (playerShip != null)
            {
                playerShip.takeDamage(2);
                Debug.Log("¡BOSS golpeó al barco!");
            }
        }
    }

    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
        if (HP <= 0)
        {
            Debug.Log("EL BOSS HA MUERTO.");
            // Aquí podrías desactivar el script o disparar animación de muerte
        }
    }
}