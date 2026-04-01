using UnityEngine;

public class Gaviota_Volando : State_Base
{
    private Gaviota gaviota;
    private float direccionX = 1f;
    private Animation_Controller anim;
    private Vector3 escalaOriginal;

    public override void EnterState()
    {
        // Buscamos en el padre de forma más robusta
        gaviota = GetComponentInParent<Gaviota>();

        if (gaviota != null)
        {
            controlledObject = gaviota.gameObject;
            anim = controlledObject.GetComponent<Animation_Controller>();
            escalaOriginal = controlledObject.transform.localScale;
        }
    }

    public override void UpdateState()
    {
        if (gaviota == null || anim == null) return;

        // 1. Prioridad Máxima: Si se queda sin vida, explota
        if (gaviota.getHP() <= 0)
        {
            state_machine.SetState<Gaviota_Explotando>();
            return;
        }

        // 2. Si toca el barco (detectado por el trigger en la clase Gaviota), pasa a Picada
        if (gaviota.enAreaBarco)
        {
            state_machine.SetState<Gaviota_Picada>();
            return;
        }

        // Movimiento normal
        Vector3 movimiento = new Vector3(
            direccionX * gaviota.velocidadNormal * Time.deltaTime,
            -0.2f * Time.deltaTime,
            0
        );
        controlledObject.transform.Translate(movimiento);

        anim.Play(0);
        ManejarLimitesYEscala();
    }

    private void ManejarLimitesYEscala()
    {
        if (controlledObject.transform.position.x > gaviota.limiteXDerecha)
        {
            direccionX = -1f;
            controlledObject.transform.localScale = escalaOriginal;
        }
        else if (controlledObject.transform.position.x < gaviota.limiteXIzquierda)
        {
            direccionX = 1f;
            // Invertimos el eje X para el efecto de "Espejo"
            controlledObject.transform.localScale = new Vector3(-escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
        }
    }
}