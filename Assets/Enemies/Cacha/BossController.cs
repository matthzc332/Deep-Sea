using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Entity
{
    [Header("Referencias")]
    private Transform targetShip;
    private State_Machine stateMachine;
    private Rigidbody2D rb;

    [Header("Movimiento")]
    public float verticalSpeed = 2f;
    private bool allowFollow = false;

    [Header("Combate")]
    public float attackInterval = 4f;
    private float nextAttackTimer;

    public List<State_Base> attackPool;

    void Start()
    {
        transform.position = new Vector3(-10f, -0.9f, 0f);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        rb = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<State_Machine>();
        nextAttackTimer = attackInterval;

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0;
        }

        GameObject shipObj = GameObject.FindGameObjectWithTag("Ship");
        if (shipObj != null)
            targetShip = shipObj.transform;

        if (HP <= 0) HP = 50;

        StartCoroutine(EnableFollowAfterDelay(0.5f));
    }

    void Update()
    {
        if (!isAlive || targetShip == null) return;

        if (stateMachine.GetCurrentState() is BossPursueState)
        {
            nextAttackTimer -= Time.deltaTime;

            if (nextAttackTimer <= 0 && attackPool.Count > 0)
                TriggerRandomAttack();
        }
    }

    void FixedUpdate()
    {
        if (!isAlive || targetShip == null || !allowFollow) return;

        if (stateMachine.GetCurrentState() is BossPursueState)
            MoveBoss();
    }

    void MoveBoss()
    {
        float newY = Mathf.Lerp(
            transform.position.y,
            targetShip.position.y,
            verticalSpeed * Time.fixedDeltaTime
        );

        if (rb != null)
            rb.MovePosition(new Vector2(transform.position.x, newY));
        else
            transform.position = new Vector3(transform.position.x, newY, 0);
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
            stateMachine.SetState<BossPursueState>();

        allowFollow = true;
    }
}