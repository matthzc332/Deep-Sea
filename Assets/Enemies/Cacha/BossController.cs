//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class BossController : Entity
//{
//    [Header("Referencias")]
//    public Transform playerShip;
//    private State_Machine stateMachine;



//    [Header("Configuración de Combate")]
//    public float attackInterval = 5f;
//    private float nextAttackTimer;
//    private Transform barco;

//    // evitar saltos de frame
//    private bool allowFollow = false;


//    // ESTA ES LA CLAVE: Arrastra aquí tus objetos de ataque (Jump, Bite, Bomb)
//    [Tooltip("Lista de ataques posibles para este jefe")]
//    public List<State_Base> attackPool;

//    void Start()
//    {
//        barco = GameObject.FindGameObjectWithTag("Ship")?.transform;
//        stateMachine = GetComponent<State_Machine>();
//        nextAttackTimer = attackInterval;

//        if (playerShip == null)
//            playerShip = GameObject.FindGameObjectWithTag("Ship")?.transform;

//        // Aseguramos vida inicial para que no desaparezca
//        if (HP <= 0) HP = 500f;

//        // seguir ruta del boss
//            Debug.Log($"Spawned Boss at {transform.position}, player at {(playerShip ? playerShip.position : Vector3.zero)}");
//        // ...



//        //delay al boss claro
//        StartCoroutine(EnableFollowAfterDelay(0.15f));
//    }
//    IEnumerator EnableFollowAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        allowFollow = true;
//    }



//    void Update()
//    {
//        if (!isAlive || playerShip == null) return;

//        if (allowFollow)
//        { 
//            // Movimiento vertical suave (Persecución)
//            // Solo ocurre si no hay un ataque bloqueando el movimiento
//            Vector3 targetPos = new Vector3(transform.position.x, playerShip.position.y, 0);
//        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
//        }
//        // Lógica de ataque modular
//        nextAttackTimer -= Time.deltaTime;
//        if (nextAttackTimer <= 0 && attackPool.Count > 0)
//        {
//            // Elige un ataque al azar de la lista del Inspector
//            int randomIndex = Random.Range(0, attackPool.Count);
//            stateMachine.SetState(attackPool[randomIndex]);

//            nextAttackTimer = attackInterval;
//        }

//        // posicion jugador
//        Debug.Log($"Boss pos {transform.position}, playerY {playerShip.position.y}");
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Entity
{
    [Header("Referencias")]
    public Transform playerShip;
    private State_Machine stateMachine;
    private Rigidbody2D rb;

    [Header("Movimiento")]
    public float verticalSpeed = 2f;
    private bool allowFollow = false;

    [Header("Combate")]
    public float attackInterval = 4f; // Tiempo entre ataques
    private float nextAttackTimer;

    // Arrastra aquí tus scripts de estado (Jump, Bite, Bomb) en el Inspector
    [Tooltip("Lista de ataques posibles para este jefe")]
    public List<State_Base> attackPool;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<State_Machine>();

        // Inicializar temporizador
        nextAttackTimer = attackInterval;

        if (rb == null) Debug.LogWarning("BossController: Falta Rigidbody2D.");
        if (stateMachine == null) Debug.LogError("BossController: Falta State_Machine.");

        // Buscar al jugador si no está asignado
        if (playerShip == null)
            playerShip = GameObject.FindGameObjectWithTag("Ship")?.transform;

        // Asegurar vida inicial
        if (HP <= 0) HP = 500f;

        // Pequeño delay antes de empezar a moverse
        StartCoroutine(EnableFollowAfterDelay(0.5f));
    }

    IEnumerator EnableFollowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Aseguramos que empiece en estado de persecución
        if (stateMachine.GetCurrentState() == null)
        {
            stateMachine.SetState<BossPursueState>();
        }
        allowFollow = true;
    }

    void Update()
    {
        if (!isAlive || playerShip == null) return;

        // Lógica de Temporizador para Atacar
        // Solo cuenta el tiempo si ya estamos persiguiendo (BossPursueState)
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
        if (!isAlive || playerShip == null || !allowFollow) return;

        // IMPORTANTE: Solo movemos al jefe con física si está en modo PERSECUCIÓN.
        // Si está saltando o mordiendo, dejamos que el Estado (DOTween) controle el movimiento.
        if (stateMachine.GetCurrentState() is BossPursueState)
        {
            MoveBoss();
        }
    }

    void MoveBoss()
    {
        if (rb != null)
        {
            // Movimiento físico suave hacia la Y del jugador
            float newY = Mathf.Lerp(transform.position.y, playerShip.position.y, verticalSpeed * Time.fixedDeltaTime);
            Vector2 target = new Vector2(transform.position.x, newY);
            rb.MovePosition(target);
        }
        else
        {
            // Fallback si no hay Rigidbody
            Vector3 targetPos = new Vector3(transform.position.x, playerShip.position.y, 0);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * verticalSpeed);
        }
    }

    void TriggerRandomAttack()
    {
        // Elige un ataque al azar de la lista
        int randomIndex = Random.Range(0, attackPool.Count);
        State_Base attackState = attackPool[randomIndex];

        if (attackState != null)
        {
            Debug.Log($"Boss ataca con: {attackState.GetType().Name}");
            stateMachine.SetState(attackState);

            // Reiniciar el temporizador
            nextAttackTimer = attackInterval;
        }
    }
}
