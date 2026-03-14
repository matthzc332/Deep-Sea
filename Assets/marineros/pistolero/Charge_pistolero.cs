using UnityEngine;

public class Charge_pistolero : State_Base
{
    private Animation_Controller anim;
    private ManagerMarineros marineroManager;
    private float timer = 1f;

    public override void EnterState()
    {
        anim = controlledObject.GetComponent<Animation_Controller>();
        marineroManager = controlledObject.GetComponent<ManagerMarineros>();
        timer = 1f;

        // Animacion Charge (indice 2)
        if (anim != null)
            anim.Play(2);
    }

    public override void UpdateState()
    {

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (marineroManager != null && marineroManager.enemiesInArea)
            {
                ExitState("focus");
            }
            else
            {
                ExitState("idle");
            }
        }
    }

    public override void ExitState(string nextState)
    {
        if (nextState == "idle")
        {
            state_machine.SetState<Idle_pistolero>();
        }
        // AGREGAR ESTA CONDICIÓN:
        else if (nextState == "focus")
        {
            state_machine.SetState<Focus_pistolero>();
        }
    }
}