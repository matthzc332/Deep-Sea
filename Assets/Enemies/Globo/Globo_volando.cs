using UnityEngine;

public class Globo_Volando : State_Base
{
    private Transform barco;
    private Globo globo;

    public override void EnterState()
    {
        globo = controlledObject.GetComponent<Globo>();
        barco = GameObject.FindGameObjectWithTag("Ship")?.transform;
        Debug.Log("El globo comienza a volar hacia el barco.");
    }

    public override void UpdateState()
    {
        if (barco == null || globo == null) return;

        // Mover el globo hacia el barco
        Vector3 direccion = (barco.position - controlledObject.transform.position).normalized;
        controlledObject.transform.Translate(direccion * globo.getSpeed() * Time.deltaTime);

        // Verificar si el globo ha alcanzado el barco
        if (globo.getCollisionWithShip() == true)
        {
            ExitState("Globo_Explotando");
            
        }
        if (globo.getHP() <= 0){
            ExitState("Globo_Explotando");
        }
    }

    public override void ExitState(string nextState)
    {
        Debug.Log($"El globo sale de Volando y va a {nextState}.");
        if (nextState == "Globo_Explotando"){
            state_machine.SetState<Globo_Explotando>();
        }
    }


}
