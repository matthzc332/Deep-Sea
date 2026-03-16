using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerStateBase currentState;

    void Start()
    {
        currentState.Enter();
    }

    void Update()
    {
        currentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }

    public void ChangeState(PlayerStateBase newState)
    {
        if (newState == currentState) return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
