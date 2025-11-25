using UnityEngine;

public class BulletIdleState : State_Base
{
    private Bullet bullet;

    public override void EnterState()
    {
        state_machine = GetComponent<State_Machine>();
        bullet = GetComponentInParent<Bullet>();
    }

    public override void UpdateState()
    {
        if (bullet.initialSpeed > 0f && bullet.enemy != Vector3.zero)
        {
            ExitState("Move");
        }
    }
    public override void ExitState(string nextState)
    {
        if (nextState == "Move"){
            state_machine.SetState<BulletMoveState>();
        }

        
    }

}