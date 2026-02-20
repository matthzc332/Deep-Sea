using UnityEngine;

public class BossRelocateState : State_Base
{
    private Animation_Controller anim;
    private float timer = 1f;

    protected override void Awake()
    {
        base.Awake();
        anim = controlledObject.GetComponent<Animation_Controller>();
    }

    public override void EnterState()
    {
        timer = 1f;

        // Usamos animacion Pursue (indice 1)
        if (anim != null)
            anim.Play(1, 1f);
    }

    public override void UpdateState()
    {
        if (anim != null)
            anim.Play(1, 1f);

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            state_machine.SetState<BossPursueState>();
        }
    }

    public override void ExitState(string nextState)
    {
    }
}