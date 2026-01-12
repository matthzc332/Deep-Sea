using UnityEngine;
using DG.Tweening;

public class BossJumpState : State_Base
{
    public float jumpPower = 5f;
    public float jumpDuration = 2f;
    public float targetRightX = 6f;

    public override void EnterState()
    {
        Vector3 originalPos = controlledObject.transform.position;
        Vector3 targetPos = new Vector3(targetRightX, originalPos.y, 0);

        // Salto hacia la derecha y regreso automático
        controlledObject.transform.DOJump(targetPos, jumpPower, 1, jumpDuration)
            .OnComplete(() => {
                controlledObject.transform.DOMove(originalPos, 1f)
                    .OnComplete(() => state_machine.SetState<BossPursueState>());
            });
    }
}