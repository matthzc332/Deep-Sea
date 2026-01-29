using UnityEngine;

public class Shoot_Cannon : State_Base
{
    protected Cannon2 cannon;
    public GameObject bulletPrefab;
    
    // 1. Agregamos una variable para el archivo de sonido
    [Header("Audio")]
    public AudioClip sonidoDisparo; 

    public override void EnterState()
    {
        if (Joystick.estoyTocando)
        {
            Debug.Log("No se puede disparar - Joystick en uso");
            ExitState("Idle");
            return;
        }

        cannon = controlledObject.GetComponent<Cannon2>();

        // 2. Intentamos obtener el AudioSource del objeto que controlamos (el cañón)
        AudioSource audioSource = controlledObject.GetComponent<AudioSource>();

        if (bulletPrefab != null)
        {
            Vector3 spawnPosition = controlledObject.transform.position;
            GameObject bulletObj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            
            if (bulletScript != null)
            {
                float speed = cannon.power_shoot;
                int power = 1; 
                int pierceCount = 0; 

                bulletScript.Initialize(cannon.objective, speed, power, pierceCount);
                
                cannon.amount_ammunition -= 1;

                // --- AQUÍ REPRODUCIMOS EL SONIDO ---
                // Usamos PlayOneShot para que si disparas rápido, los sonidos se superpongan y no se corten
                if (audioSource != null && sonidoDisparo != null)
                {
                    audioSource.PlayOneShot(sonidoDisparo);
                }
                else
                {
                    // Debug para saber si te olvidaste de asignar algo en Unity
                    if (audioSource == null) Debug.LogWarning("El Cañón no tiene componente AudioSource");
                    if (sonidoDisparo == null) Debug.LogWarning("No has asignado el AudioClip de disparo en el inspector");
                }
                // ------------------------------------
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