using UnityEngine;

public class Focus_Cargador : State_Base
{
    private Animation_Controller anim;
    private float timerRecarga = 2f; // Tiempo entre cada bala generada
    private ManagerMarineros marineroManager;
    private Ship playerShip;

    public override void EnterState()
    {
        anim = controlledObject.GetComponent<Animation_Controller>();
        marineroManager = controlledObject.GetComponent<ManagerMarineros>();
        playerShip = Object.FindFirstObjectByType<Ship>();

        if (anim != null)
            anim.Play(1); // Asumiendo que 1 es la animación de "recargando"

        timerRecarga = 2f;
    }

    public override void UpdateState()
    {
        // El cargador trabaja mientras haya enemigos acechando el barco
        if (marineroManager != null && marineroManager.enemiesInArea)
        {
            timerRecarga -= Time.deltaTime;

            if (timerRecarga <= 0f)
            {
                RecargarBala();
                timerRecarga = 2f; // Reset del cooldown de recarga
            }
        }
        else
        {
            state_machine.SetState<Idle_pistolero>(); // Vuelve a Idle si no hay peligro
        }
    }

   private void RecargarBala()
{
    // Buscamos el arma en la escena (que es la que tiene la variable amount_ammunition)
    Base_Gun gunSystem = Object.FindFirstObjectByType<Base_Gun>();

    if (gunSystem != null)
    {
        // Sumamos 1 a la variable pública del arma
        gunSystem.amount_ammunition += 1; 
        
        // Feedback visual con DOTween (opcional)
      //  controlledObject.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
        
        Debug.Log("Cargador: Bala añadida al Base_Gun. Total: " + gunSystem.amount_ammunition);
    }
    else 
    {
        Debug.LogError("No se encontró el script 'Base_Gun' en la escena.");
    }
}
}