using UnityEngine;
using DG.Tweening;

public class BossJumpState : State_Base
{
    public float jumpPower = 7f;
    public float jumpDuration = 1.5f;

    private Animation_Controller anim;
    private Sequence jumpSequence;

    protected override void Awake()
    {
        base.Awake();
        anim = controlledObject.GetComponent<Animation_Controller>();
    }

    public override void EnterState()
    {
        if (anim != null)
            anim.Play(3, 1f);

        if (jumpSequence != null)
            jumpSequence.Kill();

        jumpSequence = DOTween.Sequence();
        jumpSequence.Append(
            controlledObject.transform.DOJump(
                new Vector3(9f, -1f, 0),
                jumpPower,
                1,
                jumpDuration
            )
        );

        jumpSequence.OnComplete(() =>
        {
            state_machine.SetState<BossPursueState>();
        });
    }

    public override void UpdateState()
    {
        if (anim != null)
            anim.Play(3, 1f);
    }

    public override void ExitState(string next)
    {
        if (jumpSequence != null)
            jumpSequence.Kill();
    }
}