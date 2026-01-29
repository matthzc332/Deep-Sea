using UnityEngine;
using DG.Tweening;

public class BossJumpState : State_Base
{
    [Header("Configuración del Salto")]
    public float jumpPower = 7f;
    public float jumpDuration = 1.5f;
    public float targetDistanceX = 8f; // Cuánto se desplaza lateralmente

    [Header("Animación y Efectos")]
    public string jumpAnimName = "Salto"; 
    public GameObject jumpEffectPrefab; 
    
    private Animator animator;
    private Sequence jumpSequence;
    private Vector3 originalPos;

    protected override void Awake()
    {
        base.Awake();
        animator = controlledObject.GetComponentInChildren<Animator>();
    }

    public override void EnterState()
    {
        Debug.Log("Cachalote iniciando: SALTO");
        originalPos = controlledObject.transform.position;

        // 1. Ejecutar animación de Salto
        if (animator != null)
        {
            animator.Play(jumpAnimName);
        }

        // 2. Efecto visual de salpicadura al inicio
        if (jumpEffectPrefab != null)
        {
            Instantiate(jumpEffectPrefab, controlledObject.transform.position, Quaternion.identity);
        }

        // 3. Iniciar la secuencia de movimiento
        PerformJumpSequence();
    }

    // El UpdateState queda libre por si quieres añadir lógica de daño por contacto durante el salto
    public override void UpdateState() { }

    void PerformJumpSequence()
{
    // 1. Buscamos al jugador para decidir a qué extremo saltar
    GameObject player = GameObject.FindGameObjectWithTag("Ship");
    float targetX = 0f;

    if (player != null)
    {
        // Si el jefe está a la derecha del barco, salta hasta el extremo izquierdo (-10)
        // Si el jefe está a la izquierda del barco, salta hasta el extremo derecho (10)
        targetX = (controlledObject.transform.position.x > player.transform.position.x) ? -9f : 9f;
    }
    else
    {
        // Fallback: Si no hay barco, elige el extremo más lejano a su posición actual
        targetX = (controlledObject.transform.position.x > 0) ? -10f : 10f;
    }

    // 2. Calculamos la posición final exacta (X: 10 o -10, Y: -1)
    Vector3 targetPos = new Vector3(targetX, -1f, 0);

    jumpSequence = DOTween.Sequence();

    // PASO 1: El Salto Parabólico hacia el destino fijo
    jumpSequence.Append(controlledObject.transform.DOJump(targetPos, jumpPower, 1, jumpDuration).SetEase(Ease.Linear));

    // PASO 2: Efecto al caer
    jumpSequence.AppendCallback(() => {
        if (jumpEffectPrefab != null)
        {
            Instantiate(jumpEffectPrefab, controlledObject.transform.position, Quaternion.identity);
        }
    });

    // PASO 3: Fin y cambio de estado
    jumpSequence.OnComplete(() => {
        state_machine.SetState<BossPursueState>();
    });
}

    public override void ExitState(string nextState)
    {
        // Matamos la secuencia para evitar que el Cachalote siga moviéndose por código
        if (jumpSequence != null) jumpSequence.Kill();

        // Nos aseguramos de que termine en una posición coherente si se interrumpe
        // (Opcional: podrías forzar la posición original aquí también)
        Debug.Log("Estado Salto finalizado.");
    }
}