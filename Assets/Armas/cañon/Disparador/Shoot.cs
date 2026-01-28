using UnityEngine;

public class Shoot_Cannon : State_Base
{
    protected Cannon2 cannon;
    public GameObject bulletPrefab;

    public override void EnterState()
    {
        if (Joystick.estoyTocando)
        {
            Debug.Log("No se puede disparar - Joystick en uso");
            ExitState("Idle");
            return;
        }

        cannon = controlledObject.GetComponent<Cannon2>();

        if (bulletPrefab != null)
        {
            Vector3 spawnPosition = controlledObject.transform.position;
            GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            
            if (bulletScript != null)
            {
                float speed = cannon.power_shoot;
                int power = 1; 
                int pierceCount = 0; // Añadimos el valor de pierce que faltaba

                // Corregido: Ahora enviamos los 4 parámetros que pide Bullet.cs
                bulletScript.Initialize(cannon.objective, speed, power, pierceCount);
                
                // IMPORTANTE:
                // Si 'cannon.objective' es un Vector3, no podemos usar LaunchTowards(Transform).
                // Como Initialize ya aplica la velocidad, NO es necesario llamar a LaunchTowards aquí.
                // bulletScript.LaunchTowards(...) -> Se elimina para evitar conflictos de tipos.

                //Debug.Log($"Bala disparada hacia: {cannon.objective}");
                cannon.amount_ammunition -= 1;
            }
            else
            {
                Debug.LogError("El prefab de bala no tiene el componente Bullet");
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