using UnityEngine;

public class Gaviota_Picada : State_Base
{
    private Gaviota gaviota;
    private float distanciaParaExplotar = 0.3f;
    private Animator animation;

    public override void EnterState()
    {
        controlledObject = transform.parent.gameObject;
        gaviota = controlledObject.GetComponent<Gaviota>();
        animation = controlledObject.GetComponent<Animator>();

        if (gaviota != null && gaviota.debug)
            Debug.Log("Gaviota entra en Picada");
    }

    public override void UpdateState()
    {
        if (gaviota == null || gaviota.barco == null) return;

        // Movimiento hacia el barco
        Vector3 direccion = (gaviota.barco.position - controlledObject.transform.position).normalized;
        controlledObject.transform.Translate(direccion * gaviota.velocidadPicada * Time.deltaTime);
        animation.Play("Gaviota");

        // Cambia a Explotando solo al llegar al barco o si vida <= 0
        float distancia = Vector3.Distance(controlledObject.transform.position, gaviota.barco.position);
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
