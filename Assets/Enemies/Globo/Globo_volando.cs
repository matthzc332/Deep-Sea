using UnityEngine;

public class Globo_Volando : State_Base
{
    private Transform barco;
    private Globo globo;
    private Ship shipScript;
    private Animation_Controller anim;

    public override void EnterState()
    {
        globo = controlledObject.GetComponent<Globo>();
        barco = GameObject.FindGameObjectWithTag("Ship")?.transform;
        shipScript = barco != null ? barco.GetComponent<Ship>() : null;

        anim = controlledObject.GetComponent<Animation_Controller>();
    }

    public override void UpdateState()
    {
        if (barco == null || globo == null || anim == null)
            return;

        // Movimiento hacia el barco
        Vector3 direccion =
            (barco.position - controlledObject.transform.position).normalized;

        controlledObject.transform.Translate(
            direccion * globo.getSpeed() * Time.deltaTime
        );

        // Animacion VOLANDO (indice 0)
        anim.Play(0, globo.getSpeed());

        // Colision con el barco
        if (globo.getCollisionWithShip())
        {
            ExitState("Colision_con_barco");
            return;
        }

        // Si muere
        if (globo.getHP() <= 0)
        {
            ExitState("Globo_Explotando");
        }
    }

    public override void ExitState(string nextState)
    {
        if (nextState == "Colision_con_barco")
        {
            if (shipScript != null)
                shipScript.takeDamage(1);

            state_machine.SetState<Globo_Explotando>();
        }

        if (nextState == "Globo_Explotando")
        {
            state_machine.SetState<Globo_Explotando>();
        }
    }
}