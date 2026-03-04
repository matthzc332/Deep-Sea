// using UnityEngine;

// public class Shoot_Cannon : State_Base
// {
//     protected Cannon2 cannon;
//     public GameObject bulletPrefab;

//     [Header("Audio")]
//     public AudioClip sonidoDisparo;

//     private ShipData shipData;

//     public override void EnterState()
//     {
//         // 1. VALIDACIÓN INICIAL (¿Puedo disparar?)
//         if (Joystick.estoyTocando || BloqueoUI.TocandoBoton)
//         {
//             ExitState("Idle");
//             return;
//         }

      

//         cannon = controlledObject.GetComponent<Cannon2>();
//         AudioSource audioSource = controlledObject.GetComponent<AudioSource>();

//         // 2. FEEDBACK INSTANTÁNEO (Sonido apenas entra al estado)
//         if (audioSource != null && sonidoDisparo != null)
//         {
//             audioSource.PlayOneShot(sonidoDisparo);
//         }

//         // 3. LÓGICA DE PROCESAMIENTO (Búsqueda de datos y spawn)
//         if (shipData == null)
//         {
//             Ship playerShip = controlledObject.GetComponentInParent<Ship>();
//             if (playerShip != null) shipData = playerShip.shipData;
//         }

//         if (bulletPrefab != null)
//         {
//             GameObject bulletObj = Instantiate(bulletPrefab, controlledObject.transform.position, Quaternion.identity);
//             Bullet bulletScript = bulletObj.GetComponent<Bullet>();

//             if (bulletScript != null)
//             {
//                 bulletScript.Initialize(cannon.objective, cannon.power_shoot, 1, 0);
//                 cannon.amount_ammunition -= 1;

//                 if (shipData != null)
//                 {
//                     shipData.balasGastadas++;
//                 }
//             }
//         }

//         ExitState("Idle");
//     }

//     public override void ExitState(string nextState)
//     {
//         if (nextState == "Idle")
//         {
//             state_machine.SetState<Idle_Cannon>();
//         }
//     }
// }

using UnityEngine;

public class Shoot_Cannon : State_Base
{
    protected Cannon2 cannon;
    public GameObject bulletPrefab;

    [Header("Audio")]
    public AudioClip sonidoDisparo;

    private ShipData shipData;
    private AudioSource audioSource; // Referencia persistente para evitar GetComponent repetidos

    public override void EnterState()
    {
        // 3. VALIDACIÓN DE UI (¿El toque fue en un botón o joystick?)
        if (Joystick.estoyTocando || BloqueoUI.TocandoBoton)
        {
            ExitState("Idle");
            return;
        }
        // 1. OBTENER REFERENCIAS NECESARIAS
        if (cannon == null) cannon = controlledObject.GetComponent<Cannon2>();
        if (audioSource == null) audioSource = controlledObject.GetComponent<AudioSource>();

        // 2. FEEDBACK INSTANTÁNEO (Sonido)
        // Se coloca al principio para que el jugador sienta la respuesta inmediata al clic
        if (audioSource != null && sonidoDisparo != null)
        {
            audioSource.Stop(); 
            audioSource.clip = sonidoDisparo;
            audioSource.Play(); 
        }

        

        // 4. LÓGICA DE PROCESAMIENTO
        if (shipData == null)
        {
            Ship playerShip = controlledObject.GetComponentInParent<Ship>();
            if (playerShip != null) shipData = playerShip.shipData;
        }

        // 5. INSTANCIACIÓN DE BALA
        if (bulletPrefab != null && cannon != null)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, controlledObject.transform.position, Quaternion.identity);
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.Initialize(cannon.objective, cannon.power_shoot, 1, 0);
                cannon.amount_ammunition -= 1;

                if (shipData != null)
                {
                    shipData.balasGastadas++;
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