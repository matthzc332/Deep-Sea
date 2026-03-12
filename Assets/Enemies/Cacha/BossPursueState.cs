using UnityEngine;
using DG.Tweening;

public class BossPursueState : State_Base
{
    [SerializeField] private float timeBeforeNextAttack = 3f;
    [SerializeField] private float entrySinkDepth = -2f;
    [SerializeField] private float targetY = 0.5f;
    [SerializeField] private float transitionDuration = 1f;

    private Animation_Controller anim;
    private Transform player;
    private float timer;
    private Sequence entrySequence;
    private Vector3 originalScale;

    protected override void Awake()
    {
        base.Awake();
        anim = controlledObject.GetComponent<Animation_Controller>();
        originalScale = controlledObject.transform.localScale;
    }

    public override void EnterState()
    {
        timer = 0f;

        if (anim != null)
            anim.Play(1, 1f);

        GameObject ship = GameObject.FindGameObjectWithTag("Ship");
        if (ship != null)
            player = ship.transform;

        if (entrySequence != null)
            entrySequence.Kill();

        entrySequence = DOTween.Sequence();
        entrySequence.Append(controlledObject.transform.DOMoveY(entrySinkDepth, transitionDuration / 2));
        entrySequence.Append(controlledObject.transform.DOMoveY(targetY, transitionDuration));
    }

    public override void UpdateState()
    {
        if (anim != null)
            anim.Play(1, 1f);

        if (player == null) return;

        timer += Time.deltaTime;

        if (timer >= timeBeforeNextAttack)
        {
            state_machine.SetState<BossJumpState>();
            return;
        }

        HandleFlip();
    }

    void HandleFlip()
    {
        float dir = player.position.x - controlledObject.transform.position.x;
        float absX = Mathf.Abs(originalScale.x);

        controlledObject.transform.localScale =
            (dir < 0)
            ? new Vector3(-absX, originalScale.y, originalScale.z)
            : new Vector3(absX, originalScale.y, originalScale.z);
    }

    public override void ExitState(string next)
    {
        if (entrySequence != null)
            entrySequence.Kill();
    }
}