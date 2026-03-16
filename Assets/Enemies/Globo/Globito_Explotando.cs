using UnityEngine;

public class Globo_Explotando : State_Base
{
    private Animation_Controller anim;
    private Globo globo;

    public override void EnterState()
    {
        globo = controlledObject.GetComponent<Globo>();
        anim = controlledObject.GetComponent<Animation_Controller>();

        if (globo != null)
        {
            // Destruir despues de la duracion configurada
            Destroy(controlledObject, globo.duracionExplosion);
        }
    }

    public override void UpdateState()
    {
        if (anim == null)
            return;

        // Animacion EXPLOTANDO (indice 1)
        anim.Play(1);
    }
}