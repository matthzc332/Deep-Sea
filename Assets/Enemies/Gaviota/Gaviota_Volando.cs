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
        // Early exit si algo falta o si ya no está viva (para evitar errores si Entity ya procesó la muerte)
        if (gaviota == null || anim == null || !gaviota.getIsAlive()) return;

        // Comprobar muerte primero: Si ya murió, saltamos al estado de explosión y salimos
        if (gaviota.getHP() <= 0)
        {
            state_machine.SetState<Gaviota_Explotando>();
            return;
        }

        // Movimiento
        Vector3 movimiento = new Vector3(
            direccionX * gaviota.velocidadNormal * Time.deltaTime,
            -0.2f * Time.deltaTime, // Un ligero descenso constante
            0
        );

        controlledObject.transform.Translate(movimiento);

        // Animación: Estado 0 = Volando. 
        // Tip: Ajusta el multiplicador (0.5f por ejemplo) para que los frames no pasen tan rápido.
        anim.Play(0);

        // Cambio de estado por proximidad
        if (gaviota.enAreaBarco)
        {
            state_machine.SetState<Gaviota_Picada>();
            return;
        }

        // Límites de pantalla y cambio de escala (Flip)
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