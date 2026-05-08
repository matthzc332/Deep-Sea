using UnityEngine;

public class Player_Jump_State : PlayerStateBase
{
    public override void Enter()
    {
        player.animator.SetBool("isGrounded", false);
    }

    public override void LogicUpdate()
    {
        if (player.isGrounded)
            stateMachine.ChangeState(
                stateMachine.GetComponentInChildren<Player_Idle_State>()
            );
    }
}
