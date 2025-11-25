using UnityEngine;

public class BulletMoveState : State_Base
{
    private Bullet bullet;
    private float timer = 2;


    public override void EnterState()
    {
        bullet = controlledObject.GetComponent<Bullet>();
        bullet.LaunchTowards(bullet.enemy, bullet.initialSpeed);
    }

    public override void UpdateState()
    {
        if (bullet != null)
        {
            if (bullet.rb.linearVelocity.sqrMagnitude > 1e-6f) 
                bullet.OrientToVelocity();
            
            if (timer >= 0){
                timer = timer - Time.deltaTime;
            }
            else{
                ExitState("Bullet_destroy");
            }

            // Verificar colisión de manera más robusta
            if (bullet.collision)
            {
                Debug.Log("Colisión detectada en UpdateState");
                ExitState("Bullet_destroy");
            }
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