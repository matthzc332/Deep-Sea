using UnityEngine;

public class BulletIdleState : State_Base
{
    private Bullet bullet;

    public override void EnterState()
    {
        // Inicializamos las referencias
        state_machine = GetComponent<State_Machine>();
        bullet = GetComponentInParent<Bullet>();
    }

    public override void UpdateState()
    {
        // Solo debe existir un UpdateState.
        // Verificamos si la bala ya tiene velocidad y un objetivo asignado.
        if (bullet != null && bullet.initialSpeed > 0f && bullet.enemy != null)
        {
            ExitState("Move");
        }
    }

    public override void ExitState(string nextState)
    {
        if (nextState == "Move")
        {
            state_machine.SetState<BulletMoveState>();
        }
    }
}