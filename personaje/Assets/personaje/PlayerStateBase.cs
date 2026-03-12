using UnityEngine;

public abstract class PlayerStateBase : MonoBehaviour
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;

    protected virtual void Awake()
    {
        stateMachine = GetComponentInParent<PlayerStateMachine>();
        player = stateMachine.GetComponent<PlayerController>();
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
