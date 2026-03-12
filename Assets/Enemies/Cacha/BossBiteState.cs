using UnityEngine;
using DG.Tweening;

public class BossBiteState : State_Base
{
    public float sinkDepth = 10f;
    public float targetY = 0.9f;
    public float sinkDuration = 1f;
    public float biteDuration = 0.5f;
    public float stayBitingTime = 0.5f;

    public GameObject biteEffectPrefab;

    private Animation_Controller anim;
    private Sequence sequence;
    private float startY;

    protected override void Awake()
    {
        base.Awake();
        anim = controlledObject.GetComponent<Animation_Controller>();
    }

    public override void EnterState()
    {
        startY = controlledObject.transform.position.y;

        if (anim != null)
            anim.Play(2, 1f);

        if (sequence != null)
            sequence.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(controlledObject.transform.DOMoveY(startY - sinkDepth, sinkDuration));
        sequence.Append(controlledObject.transform.DOMoveY(targetY, biteDuration));
        sequence.AppendInterval(stayBitingTime);
        sequence.Append(controlledObject.transform.DOMoveY(startY - sinkDepth, sinkDuration));
        sequence.OnComplete(() =>
        {
            state_machine.SetState<BossPursueState>();
        });
    }

    public override void UpdateState()
    {
        if (anim != null)
            anim.Play(2, 1f);
    }

    public override void ExitState(string next)
    {
        if (sequence != null)
            sequence.Kill();
    }
}