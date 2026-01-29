using UnityEngine;

public class Shoot_Cannon : State_Base
{
    protected Cannon2 cannon;
    public GameObject bulletPrefab;
    
    [Header("Audio")]
    public AudioClip sonidoDisparo;

    // Referencia temporal para acceder a los datos
    private ShipData shipData; 

    public override void EnterState()
    {
        if (Joystick.estoyTocando)
        {
            Debug.Log("No se puede disparar - Joystick en uso");
            ExitState("Idle");
            return;
        }

        cannon = controlledObject.GetComponent<Cannon2>();
        AudioSource audioSource = controlledObject.GetComponent<AudioSource>();

        // Intentamos obtener el ShipData desde el barco (asumiendo que el cañón es hijo del Barco)
        if(shipData == null)
        {
            // Busca el componente Ship en el padre o en el mismo objeto
            Ship playerShip = controlledObject.GetComponentInParent<Ship>();
            if (playerShip != null) shipData = playerShip.shipData;
        }

        if (bulletPrefab != null)
        {
            // ... (código de instanciación igual) ...
            GameObject bulletObj = Instantiate(bulletPrefab, controlledObject.transform.position, Quaternion.identity);
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            
            if (bulletScript != null)
            {
                // ... (inicialización de bala igual) ...
                bulletScript.Initialize(cannon.objective, cannon.power_shoot, 1, 0);
                
                cannon.amount_ammunition -= 1;

                // --- NUEVO: REGISTRAR BALA GASTADA ---
                if (shipData != null)
                {
                    shipData.balasGastadas++;
                }
                // -------------------------------------

                if (audioSource != null && sonidoDisparo != null)
                {
                    audioSource.PlayOneShot(sonidoDisparo);
                }
            }
        }
        
        ExitState("Idle");
    }

    public override void ExitState(string nextState)
    {
        if (nextState == "Idle")
        {
            state_machine.SetState<Idle_Cannon>();
        }
    }
}