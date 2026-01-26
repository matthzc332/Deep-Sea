using UnityEngine;
using DG.Tweening;

public class BossJumpState : State_Base
{
    [Header("Configuración del Salto")]
    public float jumpPower = 5f;
    public float jumpDuration = 2f;
    public float targetRightX = 6f; // Distancia hacia la derecha

    [Header("Animación y Efectos")]
    public string jumpAnimationTrigger = "Jump"; // El nombre del Trigger en tu Animator
    public GameObject jumpEffectPrefab; // Arrastra aquí tu prefab (ej. salpicadura)
    
    private Animator animator;

    public override void EnterState()
    {
        // 1. Obtener componentes
        animator = controlledObject.GetComponent<Animator>();
        if (animator == null) animator = controlledObject.GetComponentInChildren<Animator>();

        // 2. Activar Animación
        if (animator != null)
        {
            animator.SetTrigger(jumpAnimationTrigger);
        }

        // 3. Instanciar el Prefab del efecto (al inicio del salto)
        if (jumpEffectPrefab != null)
        {
            Instantiate(jumpEffectPrefab, controlledObject.transform.position, Quaternion.identity);
        }

        // 4. Lógica de Movimiento (DOTween)
        PerformJump();
    }

    void PerformJump()
    {
        Vector3 originalPos = controlledObject.transform.position;
        // Calcula el punto objetivo sumando a la posición actual (o fijo según tu lógica)
        Vector3 targetPos = new Vector3(originalPos.x + targetRightX, originalPos.y, 0);

        // Salto hacia la derecha
        controlledObject.transform.DOJump(targetPos, jumpPower, 1, jumpDuration)
            .OnComplete(() => {
                // AL ATERRIZAR:
                
                // Opcional: Instanciar efecto de caída si quisieras
                // if (jumpEffectPrefab) Instantiate(jumpEffectPrefab, transform.position, Quaternion.identity);

                ReturnToPosition(originalPos);
            });
    }

    void ReturnToPosition(Vector3 returnPos)
    {
        // Regreso suave a la posición original (o volver a perseguir directamente)
        controlledObject.transform.DOMove(returnPos, 1f)
            .OnComplete(() => {
                // Avisar al animator que terminó (opcional, si tienes estado Idle)
                if (animator != null) animator.SetTrigger("Idle"); 
                
                // Volver al estado de persecución
                state_machine.SetState<BossPursueState>();
            });
    }
}