using UnityEngine;

public class Gaviota_Volando : State_Base
{
    private Gaviota gaviota;
    private float direccionX = 1f;
    private Animation_Controller anim;
    private Vector3 escalaOriginal;

    public override void EnterState()
    {
        controlledObject = transform.parent.gameObject;
        gaviota = controlledObject.GetComponent<Gaviota>();
        anim = controlledObject.GetComponent<Animation_Controller>();
        escalaOriginal = controlledObject.transform.localScale;
    }

    public override void UpdateState()
    {
        if (gaviota == null || anim == null) return;

        Vector3 movimiento = new Vector3(
            direccionX * gaviota.velocidadNormal * Time.deltaTime,
            -0.2f * Time.deltaTime,
            0
        );

        controlledObject.transform.Translate(movimiento);

        anim.Play(0, gaviota.velocidadNormal);

        if (gaviota.enAreaBarco)
        {
            state_machine.SetState<Gaviota_Picada>();
            return;
        }

        if (controlledObject.transform.position.x > gaviota.limiteXDerecha)
        {
            direccionX = -1f;
            controlledObject.transform.localScale = escalaOriginal;
        }
        else if (controlledObject.transform.position.x < gaviota.limiteXIzquierda)
        {
            direccionX = 1f;
            controlledObject.transform.localScale =
                new Vector3(-escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
        }

        if (gaviota.vida <= 0)
            state_machine.SetState<Gaviota_Explotando>();
    }
}