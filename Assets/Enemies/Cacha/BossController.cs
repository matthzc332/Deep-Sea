using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
using UnityEngine;

public class BossController : Entity
{
    public Transform playerShip;
    public float verticalSpeed = 2f; // velocidad de persecución en unidades/seg
    private Rigidbody2D rb;
    private bool allowFollow = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("BossController: falta Rigidbody2D. Añadilo para movimiento físico.");
        }

        StartCoroutine(EnableFollowAfterDelay(0.1f));
    }

    IEnumerator EnableFollowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        allowFollow = true;
    }

    void FixedUpdate()
    {
        if (!isAlive || playerShip == null || !allowFollow) return;

        if (rb != null)
        {
            // Calculamos la nueva posición Y de forma suave sin anular la física completamente
            float newY = Mathf.Lerp(transform.position.y, playerShip.position.y, verticalSpeed * Time.fixedDeltaTime);
            Vector2 target = new Vector2(transform.position.x, newY);
            rb.MovePosition(target);
        }
        else
        {
            // fallback: si no hay Rigidbody2D
            Vector3 targetPos = new Vector3(transform.position.x, playerShip.position.y, 0);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * verticalSpeed);
        }
    }
}
