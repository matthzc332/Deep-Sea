using UnityEngine;

public class Player_Run_State : PlayerStateBase
{
    public override void Enter()
    {
        player.animator.SetBool("isRunning", true);
    }

    public override void LogicUpdate()
    {
        if (player.moveInput == 0)
            stateMachine.ChangeState(
                stateMachine.GetComponentInChildren<Player_Idle_State>()
            );

        if (!player.isGrounded)
            stateMachine.ChangeState(
                stateMachine.GetComponentInChildren<Player_Jump_State>()
            );
    }

    public override void PhysicsUpdate()
    {
        player.rb.linearVelocity = new Vector2(
            player.moveInput * player.speed,
            player.rb.linearVelocity.y
        );
    }
}
