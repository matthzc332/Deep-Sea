using UnityEngine;

public class Gaviota_Explotando : State_Base
{
    private Animator animation;
    
    public override void EnterState()
    {

        animation = controlledObject.GetComponent<Animator>();

        controlledObject = transform.parent.gameObject;
        if (controlledObject == null) return;

        Gaviota gaviota = controlledObject.GetComponent<Gaviota>();
        if (gaviota == null) return;

        if (gaviota.debug)
        //    Debug.Log("Gaviota entra en Explota");
        animation.Play("Explotando");

        // Instancia explosión si hay prefab
        if (gaviota.prefabExplosion != null)
        {
            GameObject exp = Instantiate(gaviota.prefabExplosion, gaviota.transform.position, Quaternion.identity);
            Destroy(exp, gaviota.duracionExplosion);
        }

        // Destruye la gaviota después de duracionExplosion
        Destroy(controlledObject, gaviota.duracionExplosion);
    }

    public override void UpdateState() { }
    public override void ExitState(string nextState) { }
}
