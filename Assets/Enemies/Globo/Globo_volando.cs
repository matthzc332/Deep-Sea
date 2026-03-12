// using UnityEngine;

// public class Globo_Volando : State_Base
// {
//     private Transform barco;
//     private Globo globo;
//     Ship shipScript;

//     public override void EnterState()
//     {
//         globo = controlledObject.GetComponent<Globo>();
//         barco = GameObject.FindGameObjectWithTag("Ship")?.transform;
//         shipScript = barco?.GetComponent<Ship>();
//         //Debug.Log("El globo comienza a volar hacia el barco.");
//     }

//     public override void UpdateState()
//     {
//         if (barco == null || globo == null) return;

//         // Mover el globo hacia el barco
//         Vector3 direccion = (barco.position - controlledObject.transform.position).normalized;
//         controlledObject.transform.Translate(direccion * globo.getSpeed() * Time.deltaTime);

//         // Verificar si el globo ha alcanzado el barco
//         if (globo.getCollisionWithShip() == true)
//         {
//             ExitState("Colision_con_barco");
//         }
//         if (globo.getHP() <= 0){
//             ExitState("Globito_Explotando");
//         }
//     }

//     public override void ExitState(string nextState)
//     {
//         //Debug.Log($"El globo sale de Volando y va a {nextState}.");
//         if(nextState == "Colision_con_barco"){
//             shipScript.takeDamage(1);
//             state_machine.SetState<Globo_Explotando>();
//         }

//         if (nextState == "Globito_Explotando"){
//             state_machine.SetState<Globo_Explotando>();
//         }
//     }


// }
using UnityEngine;

public class Globo_Volando : State_Base
{
    private Transform barco;
    private Globo globo;
    Ship shipScript;

    public override void EnterState()
    {
        globo = controlledObject.GetComponent<Globo>();
        barco = GameObject.FindGameObjectWithTag("Ship")?.transform;
        shipScript = barco?.GetComponent<Ship>();
        //Debug.Log("El globo comienza a volar hacia el barco.");
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
            ExitState("Colision_con_barco");
        }
        if (globo.getHP() <= 0){
            ExitState("Globito_Explotando");
        }
    }

    public override void ExitState(string nextState)
    {
        //Debug.Log($"El globo sale de Volando y va a {nextState}.");
        if(nextState == "Colision_con_barco"){
            shipScript.takeDamage(1);
            state_machine.SetState<Globo_Explotando>();
        }

        if (nextState == "Globito_Explotando"){
            state_machine.SetState<Globo_Explotando>();
        }
    }


}
