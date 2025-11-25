using UnityEngine;

public class Idle_pistolero : State_Base
{
    private Animator animator;
    
    public override void EnterState(){
        // Obtener el componente Animator del objeto controlado
        animator = controlledObject.GetComponent<Animator>();
        
        // Reproducir la animación "pistolero"
        if (animator != null)
        {
            animator.Play("pistolero");
        }
        else
        {
            Debug.LogWarning("Animator no encontrado en el objeto controlado: " + controlledObject.name);
        }
        
    }

    public override void UpdateState(){

        
        // Verificar si hay enemigos en el trigger
        ManagerMarineros marineroManager = controlledObject.GetComponent<ManagerMarineros>();
        if (marineroManager != null && marineroManager.enemiesInArea)
        {
            // Cambiar al estado de apuntar
            state_machine.SetState<Focus_pistolero>();
        }
    }

}