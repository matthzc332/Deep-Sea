using UnityEngine;

public class BossPursueState : State_Base
{
    // Este estado sirve como el punto de retorno despu�s de cada ataque.
    // Mientras est� activo, el BossController maneja el movimiento vertical
    // y espera a que el temporizador elija un nuevo ataque.
    public override void EnterState()
    {
        Debug.Log("Jefe en estado de persecuci�n.");
        
        transform.rotation = Quaternion.Euler(0, 0, -90f);
    }
}
