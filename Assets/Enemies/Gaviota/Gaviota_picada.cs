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

        // 1. Si se queda sin vida mientras baja, explota
        if (gaviota.getHP() <= 0)
        {
            state_machine.SetState<Gaviota_Explotando>();
            return;
        }

        // Movimiento hacia el barco
        Vector3 direccion = (gaviota.barco.position - controlledObject.transform.position).normalized;
        controlledObject.transform.Translate(direccion * gaviota.velocidadPicada * Time.deltaTime);

        // Animación de picada
        anim.Play(1); // Cambié esto a index 1, que suele ser picada en tu lógica

        // 2. Si llega al barco o el trigger detecta contacto
        float distancia = Vector3.Distance(controlledObject.transform.position, gaviota.barco.position);

        // Si la distancia es mínima O si el flag 'enAreaBarco' es verdadero (contacto del collider)
        if (distancia <= distanciaParaExplotar || gaviota.enAreaBarco)
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