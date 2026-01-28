using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Entity
{
    [Header("Referencias")]
    // Quitamos public para que no intentes asignarlo en el inspector y falle
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

    [Tooltip("Lista de ataques posibles para este jefe")]
    public List<State_Base> attackPool;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        stateMachine = GetComponent<State_Machine>();
        nextAttackTimer = attackInterval;

        // --- FIX DE ROTACIÓN (IMPORTANTE) ---
        // Esto asegura que, aunque choque, no rote físicamente.
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // --- FIX DE ROTACIÓN ---
        // Si tu dibujo mira hacia arriba, esto lo acuesta hacia la izquierda (90) o derecha (-90).
        // Prueba con 90 o -90 según hacia dónde mire su cara.
      //  transform.rotation = Quaternion.Euler(0, 0, -90f);

        if (rb == null) Debug.LogWarning("BossController: Falta Rigidbody2D.");
        if (stateMachine == null) Debug.LogError("BossController: Falta State_Machine.");

        if (rb == null) Debug.LogWarning("BossController: Falta Rigidbody2D.");
        // ... (resto del código igual)


        // 1. B�SQUEDA AUTOM�TICA DEL SHIP
        // Esto soluciona el problema del Inspector y el Type Mismatch
        GameObject shipObj = GameObject.FindGameObjectWithTag("Ship");
        if (shipObj != null)
        {
            targetShip = shipObj.transform;
        }
        else
        {
            Debug.LogError("BossController: NO SE ENCONTR� EL OBJETO CON TAG 'Ship' EN LA ESCENA.");
        }

        if (HP <= 0) HP = 50f;

        StartCoroutine(EnableFollowAfterDelay(0.5f));


        void LateUpdate()
    {
        // FUERZA BRUTA: Esto obliga al objeto a mirar siempre a -90 grados
        // Sin importar lo que diga la física o la animación.
        transform.rotation = Quaternion.Euler(0, 0, -90f);
    }
    }

    // M�todo p�blico para que los Estados (Bite, Jump) obtengan el objetivo
    public Transform GetTarget()
    {
        return targetShip;
    }

    IEnumerator EnableFollowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (stateMachine.GetCurrentState() == null)
        {
            animator.SetBool("Pursuit", true);
            stateMachine.SetState<BossPursueState>();
        }
        allowFollow = true;
    }

    void Update()
    {
        // Usamos targetShip en lugar de Ship
        if (!isAlive || targetShip == null) return;

        if (stateMachine.GetCurrentState() is BossPursueState)
        {
            nextAttackTimer -= Time.deltaTime;

            if (nextAttackTimer <= 0 && attackPool.Count > 0)
            {
                TriggerRandomAttack();
            }
        }


        void LateUpdate()
    {
        // FUERZA BRUTA: Esto obliga al objeto a mirar siempre a -90 grados
        // Sin importar lo que diga la física o la animación.
        transform.rotation = Quaternion.Euler(0, 0, -90f);
    }

    }

    void FixedUpdate()
    {
        if (!isAlive || targetShip == null || !allowFollow) return;

        if (stateMachine.GetCurrentState() is BossPursueState)
        {
            MoveBoss();
        }
    }

    void MoveBoss()
    {
        if (rb != null)
        {
            // Usamos targetShip
            float newY = Mathf.Lerp(transform.position.y, targetShip.position.y, verticalSpeed * Time.fixedDeltaTime);
            Vector2 target = new Vector2(transform.position.x, newY);
            rb.MovePosition(target);
        }
        else
        {
            Vector3 targetPos = new Vector3(transform.position.x, targetShip.position.y, 0);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * verticalSpeed);
        }
    }

    void TriggerRandomAttack()
    {
        int randomIndex = Random.Range(0, attackPool.Count);
        State_Base attackState = attackPool[randomIndex];

        if (attackState != null)
        {
            // Debug.Log($"Boss ataca con: {attackState.GetType().Name}");
            animator.SetBool("Pursuit", false);
            if (randomIndex == 0)
            {
                animator.SetTrigger("Shoot");
            }
            if (randomIndex == 1)
            {
                animator.SetTrigger("Bite");
            }
            if (randomIndex == 2)
            {
                animator.SetTrigger("Jump");
            }
            stateMachine.SetState(attackState);
            nextAttackTimer = attackInterval;

        }
    }


    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Debug para ver si Unity detecta el choque f�sico
    //     Debug.Log("�Colisi�n detectada con: " + collision.gameObject.name + "!");

    //     if (collision.gameObject.CompareTag("Ship"))
    //     {
    //         Debug.Log("�Golpe� al Barco!");
    //         // Aqu� llamas al da�o, por ejemplo:
    //         // collision.gameObject.GetComponent<Ship>().TakeDamage(10);
    //     }
    // }

    // Si usas colliders que son Triggers (Is Trigger activado)
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. IMPORTANTE: Llama a la base para que funcione el recibir daño de las balas (Entity)
        base.OnTriggerEnter2D(collision);

        Debug.Log("Atravesó a: " + collision.gameObject.name + "!");
        Debug.Log("tiene de vida: " + HP);

        // Debug para entender qué está pasando
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log($"BOSS recibió disparo. Vida restante: {HP}");
        }

        // 2. Lógica para HACER daño al jugador
        if (collision.gameObject.CompareTag("Ship"))
        {
            Ship playerShip = collision.gameObject.GetComponent<Ship>();
            if (playerShip != null)
            {
                // Aplica el daño que quieras, por ejemplo 10 o 20
                playerShip.takeDamage(3);
                Debug.Log($"BOSS golpeó al barco. Vida del barco: {playerShip.getHP()}");
            }
        }
    }
    // Agregamos esto para visualizar la muerte del jefe
    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
        if (HP <= 0)
        {
            Debug.Log("EL BOSS HA MUERTO. Iniciando fin de oleada...");
        }
    }
}
