using UnityEngine;

public class BossPursueState : State_Base
{
    // Este estado sirve como el punto de retorno después de cada ataque.
    // Mientras está activo, el BossController maneja el movimiento vertical
    // y espera a que el temporizador elija un nuevo ataque.
    public override void EnterState()
    {
        Debug.Log("Jefe en estado de persecución.");
    }
}
