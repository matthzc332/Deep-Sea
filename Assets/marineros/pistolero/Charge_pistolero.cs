using UnityEngine;

public class Charge_pistolero : State_Base
{
    private Animation_Controller anim;
    private float timer = 1f;

    public override void EnterState()
    {
        anim = controlledObject.GetComponent<Animation_Controller>();
        timer = 1f;

        // Animacion Charge (indice 2)
        if (anim != null)
            anim.Play(2, 1f);
    }

    public override void UpdateState()
    {
        if (anim != null)
            anim.Play(2, 1f);

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ExitState("idle");
        }
    }

    public override void ExitState(string nextState)
    {
        if (nextState == "idle")
        {
            state_machine.SetState<Idle_pistolero>();
        }
    }
}