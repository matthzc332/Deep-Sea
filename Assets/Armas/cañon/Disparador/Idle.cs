using UnityEngine;
using UnityEngine.InputSystem; // ← Agregar este using

public class Idle_Cannon : State_Base
{
    protected float timer_recharge;
    protected Cannon2 cannon;

    public override void EnterState()
    {
        cannon = controlledObject.GetComponent<Cannon2>();
        if (cannon != null)
        {
            timer_recharge = cannon.time_recharge;
        }
    }

    public override void UpdateState()
    {
        if (timer_recharge >= 0)
        {
            timer_recharge -= Time.deltaTime;
        }

        // Usar Input System en lugar de Input tradicional
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && timer_recharge <= 0)
        {
            if (cannon.amount_ammunition <= 0){
                Debug.Log("No hay balas!");
            }
            else{
                Vector2 mousePos = Mouse.current.position.ReadValue();
                SaveClickPosition(mousePos);
            }
            
        }
        // Para touch móvil
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame && timer_recharge <= 0)
        {
            if (cannon.amount_ammunition <= 0){
                Debug.Log("No hay balas!");
            }
            else{
                Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
                SaveClickPosition(touchPos);
            }
        }
    }

    private void SaveClickPosition(Vector2 screenPos)
    {
        Vector3 mousePos = new Vector3(screenPos.x, screenPos.y, Mathf.Abs(Camera.main.transform.position.z));
        cannon.objective = Camera.main.ScreenToWorldPoint(mousePos);
        
    //    Debug.Log("Entro al condicional - Input detectado y timer listo");
    //    Debug.Log($"Posición guardada: {cannon.objective}");
        
        // Cambiar al estado de disparo
        ExitState("Shoot");
    }

    public override void ExitState(string nextState)
    {
        if (nextState == "Shoot")
        {
            state_machine.SetState<Shoot_Cannon>();
        }
    }
}