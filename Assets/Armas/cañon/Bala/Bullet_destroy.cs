using UnityEngine;

public class Bullet_destroy : State_Base
{
    public override void EnterState()
    {
        //Debug.Log("Bala se destruye");
        
        // Si controlledObject es la bala misma
        Destroy(controlledObject); // Pequeño delay para evitar errores
    }
}