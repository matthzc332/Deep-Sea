using UnityEngine;

public class Gaviota_Explotando : State_Base
{
    private bool yaExploto = false;

    public override void EnterState()
    {
        controlledObject = state_machine.gameObject;
        if (controlledObject == null) return;

        Gaviota gaviota = controlledObject.GetComponent<Gaviota>();
        if (gaviota == null) return;

        Animator anim = controlledObject.GetComponent<Animator>();
        if (anim != null)
            anim.Play("Explotando");

        if (!yaExploto)
        {
            yaExploto = true;

            // 🔴 DAÑO DE LA EXPLOSIÓN
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                controlledObject.transform.position,
                gaviota.radioExplosion);

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Ship"))
                {
                    Ship ship = hit.GetComponent<Ship>();
                    if (ship != null)
                        ship.takeDamage(gaviota.dañoAlBarco);
                }
            }

            if (gaviota.prefabExplosion != null)
            {
                GameObject exp = Instantiate(
                    gaviota.prefabExplosion,
                    controlledObject.transform.position,
                    Quaternion.identity);

                Destroy(exp, gaviota.duracionExplosion);
            }
        }

        Destroy(controlledObject, gaviota.duracionExplosion);
    }

    public override void UpdateState() { }
    public override void ExitState(string nextState) { }
}
