using UnityEngine;

public class Gaviota_Picada : State_Base
{
    private Gaviota gaviota;
    private float distanciaParaExplotar = 0.3f;

    private Animation_Controller anim;

    public override void EnterState()
    {
        controlledObject = transform.parent.gameObject;
        gaviota = controlledObject.GetComponent<Gaviota>();

        // 🔥 Usar tu animator personalizado
        anim = controlledObject.GetComponent<Animation_Controller>();

        if (gaviota != null && gaviota.debug)
            Debug.Log("Gaviota entra en Picada");
    }

    public override void UpdateState()
    {
        if (gaviota == null || gaviota.barco == null || anim == null)
            return;

        // Movimiento hacia el barco
        Vector3 direccion =
            (gaviota.barco.position - controlledObject.transform.position).normalized;

        controlledObject.transform.Translate(
            direccion * gaviota.velocidadPicada * Time.deltaTime
        );

        // 👉 Animación PICADA (índice 1)
        anim.Play(
            1,
            gaviota.velocidadPicada
        );

        // Cambio de estado
        float distancia = Vector3.Distance(
            controlledObject.transform.position,
            gaviota.barco.position
        );

        if (distancia <= distanciaParaExplotar || gaviota.vida <= 0)
        {
            state_machine.SetState<Gaviota_Explotando>();
        }
    }

    public override void ExitState(string nextState)
    {
        if (gaviota != null && gaviota.debug)
            Debug.Log($"Gaviota sale de Picada hacia {nextState}");
    }
}