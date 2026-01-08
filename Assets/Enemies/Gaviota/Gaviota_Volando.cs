using UnityEngine;

public class Gaviota_Volando : State_Base
{
    private Gaviota gaviota;
    private float direccionX = 1f;
    private Animator animation;
    private Vector3 escalaOriginal;

    public override void EnterState()
    {
        controlledObject = transform.parent.gameObject;
        gaviota = controlledObject.GetComponent<Gaviota>();
        animation = controlledObject.GetComponent<Animator>();
        escalaOriginal = controlledObject.transform.localScale;


        if (gaviota != null && gaviota.debug)
            Debug.Log("Gaviota entra en Volando");
    }

    public override void UpdateState()
    {
        if (gaviota == null) return;

        // Movimiento X con leve descenso
        Vector3 movimiento = new Vector3(direccionX * gaviota.velocidadNormal * Time.deltaTime, -0.2f * Time.deltaTime, 0);
        controlledObject.transform.Translate(movimiento);
        animation.Play("Gaviota");

        // Cambiar a Picada si entra en área del barco
        if (gaviota.enAreaBarco)
        {
            state_machine.SetState<Gaviota_Picada>();
            return;
        }

        // Cambia dirección en límites
        if (controlledObject.transform.position.x > gaviota.limiteXDerecha) 
        {
            direccionX = -1f;
            controlledObject.transform.localScale = escalaOriginal;
            
        }
        else if (controlledObject.transform.position.x < gaviota.limiteXIzquierda) 
        {
            direccionX = 1f;
            controlledObject.transform.localScale = new Vector3(-escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
        }
        

        // Si vida <= 0 → Explotando
        if (gaviota.vida <= 0)
            state_machine.SetState<Gaviota_Explotando>();
    }

    public override void ExitState(string nextState)
    {
        if (gaviota != null && gaviota.debug)
            Debug.Log($"Gaviota sale de Volando hacia {nextState}");
    }
}
