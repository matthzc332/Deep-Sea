//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class BossController : Entity
//{
//    [Header("Referencias")]
//    public Transform playerShip;
//    private State_Machine stateMachine;



//    [Header("Configuraci�n de Combate")]
//    public float attackInterval = 5f;
//    private float nextAttackTimer;
//    private Transform barco;

//    // evitar saltos de frame
//    private bool allowFollow = false;


//    // ESTA ES LA CLAVE: Arrastra aqu� tus objetos de ataque (Jump, Bite, Bomb)
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
//            // Movimiento vertical suave (Persecuci�n)
//            // Solo ocurre si no hay un ataque bloqueando el movimiento
//            Vector3 targetPos = new Vector3(transform.position.x, playerShip.position.y, 0);
//        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
//        }
//        // L�gica de ataque modular
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
    // Quitamos public para que no intentes asignarlo en el inspector y falle
    private Transform targetShip;
    private State_Machine stateMachine;
    private Rigidbody2D rb;

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
        stateMachine = GetComponent<State_Machine>();
        nextAttackTimer = attackInterval;
// --- FIX DE ROTACIÓN ---
        // Si tu dibujo mira hacia arriba, esto lo acuesta hacia la izquierda (90) o derecha (-90).
        // Prueba con 90 o -90 según hacia dónde mire su cara.
        transform.rotation = Quaternion.Euler(0, 0, -90f);
       
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
            stateMachine.SetState(attackState);
            nextAttackTimer = attackInterval;
     
        }
    }

    // Pon esto en BossController.cs para probar
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug para ver si Unity detecta el choque f�sico
        Debug.Log("�Colisi�n detectada con: " + collision.gameObject.name + "!");

        if (collision.gameObject.CompareTag("Ship"))
        {
            Debug.Log("�Golpe� al Barco!");
            // Aqu� llamas al da�o, por ejemplo:
            // collision.gameObject.GetComponent<Ship>().TakeDamage(10);
        }
    }

    // O si usas "Is Trigger" marcado:
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("�Atraves� a: " + collision.gameObject.name + "!");

        if (collision.gameObject.CompareTag("Ship"))
        {
            // L�gica de da�o
        }
    }
}