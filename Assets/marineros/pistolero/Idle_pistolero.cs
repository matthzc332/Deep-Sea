using UnityEngine;

public class Idle_pistolero : State_Base
{
    private Animation_Controller anim;

    public override void EnterState()
    {
        anim = controlledObject.GetComponent<Animation_Controller>();

        if (anim != null)
        {
            // Animacion Idle (indice 0)
            anim.Play(0, 1f);
        }
    }

    public override void UpdateState()
    {
        if (anim != null)
        {
            // Mantener animacion Idle
            anim.Play(0, 1f);
        }

        ManagerMarineros marineroManager =
            controlledObject.GetComponent<ManagerMarineros>();

        if (marineroManager != null && marineroManager.enemiesInArea)
        {
            state_machine.SetState<Focus_pistolero>();
        }
    }
}