using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot_Cannon : State_Base
{
    protected Cannon2 cannon;
    public GameObject bulletPrefab; // Referencia al prefab de la bala

    public override void EnterState()
    {

        cannon = controlledObject.GetComponent<Cannon2>();



        if (bulletPrefab != null)
        {
            // Posición de instancia (usar el controlledObject como referencia)
            Vector3 spawnPosition = controlledObject.transform.position;
            
            // Instanciar la bala
            GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            
            // Mantener la escala predeterminada (no modificar scale)
            // bulletObj.transform.localScale permanece como está en el prefab
            
            // Obtener el componente Bullet
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            
            if (bulletScript != null)
            {
                // Configurar la bala para que se dirija hacia cannon.objective
                float speed = cannon.power_shoot; // Velocidad de la bala
                int power = 1;     // Daño de la bala
                
                bulletScript.Initialize(cannon.objective, speed, power);
                bulletScript.LaunchTowards(cannon.objective, speed);
                
                Debug.Log($"Bala disparada hacia: {cannon.objective}");

                cannon.amount_ammunition = cannon.amount_ammunition - 1;
            }
            else
            {
                Debug.LogError("El prefab de bala no tiene el componente Bullet");
            }
        }
        else
        {
            Debug.LogError("bulletPrefab no asignado en Shoot_Cannon");
        }

        ExitState("Idle");
    }




    public override void ExitState(string nextState){
        if (nextState == "Idle"){
            state_machine.SetState<Idle_Cannon>();
        }
    }

}
