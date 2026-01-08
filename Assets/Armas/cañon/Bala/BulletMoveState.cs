using UnityEngine;

public class BulletMoveState : State_Base
{
    private Bullet bullet;
    private float timer = 2f; // Tiempo de vida de la bala

    public override void EnterState()
    {
        // 'controlledObject' debe ser la referencia al objeto raíz que tiene el script Bullet
        bullet = controlledObject.GetComponent<Bullet>();
        
        if (bullet != null)
        {
            bullet.LaunchTowards(bullet.enemy, bullet.initialSpeed);
        }
    }

    public override void UpdateState()
    {
        if (bullet == null) return;

        // 1. Orientar la bala según su velocidad actual
        if (bullet.rb.linearVelocity.sqrMagnitude > 1e-6f) 
        {
            bullet.OrientToVelocity();
        }

        // 2. Manejo del temporizador de autodestrucción
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            ExitState("Bullet_destroy");
        }

        // 3. Corregido: Usamos 'hasHit' en lugar de 'collision'
        if (bullet.hasHit)
        {
            Debug.Log("Colisión detectada: Pasando a estado de destrucción");
            ExitState("Bullet_destroy");
        }
    }

    public override void ExitState(string nextState)
    {
        if (nextState == "Bullet_destroy")
        {
            state_machine.SetState<Bullet_destroy>();
        }
    }
}