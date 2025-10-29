using UnityEngine;

public class Charge_pistolero : State_Base
{
    private Animator animator;
    private float timer = 1f;
    private GameObject brazoPistola;
    private ManagerMarineros marineroManager;

    public override void EnterState(){
        // Obtener componentes y resetear timers
        animator = controlledObject.GetComponent<Animator>();
        marineroManager = controlledObject.GetComponent<ManagerMarineros>();
        timer = 1f;
        // Reproducir la animación "cargando"
        if (animator != null)
        {
            animator.Play("recargando");
        }
        else
        {
            Debug.LogWarning("Animator no encontrado en el objeto controlado: " + controlledObject.name);
        }
        
    }

    public override void UpdateState(){
        timer = timer-Time.deltaTime;

        if (timer <= 0){
            ExitState("idle");
        }
    }

    public override void ExitState (string nextState){
        if (nextState == "idle"){
            state_machine.SetState<Idle_pistolero>();
        }
    }
}
