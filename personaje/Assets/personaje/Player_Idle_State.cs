using UnityEngine;

public class Player_Idle_State : PlayerStateBase
{
    public override void Enter()
    {
        player.animator.SetBool("isRunning", false);
    }

    public override void LogicUpdate()
    {
        if (player.moveInput != 0)
            stateMachine.ChangeState(
                stateMachine.GetComponentInChildren<Player_Run_State>()
            );

        if (!player.isGrounded)
            stateMachine.ChangeState(
                stateMachine.GetComponentInChildren<Player_Jump_State>()
            );
    }
}
