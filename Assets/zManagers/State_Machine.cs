using UnityEngine;

public class State_Machine : MonoBehaviour
{
    [SerializeField] private State_Base currentState;  
    private bool hasEnteredState = false;
    private State_Base nextState; // Para manejar cambios de estado


    void Start()
    {
        var states = GetComponentsInChildren<State_Base>(includeInactive: true);


        if (currentState != null)
        {
            Debug.Log($"Estado inicial: {currentState.GetType().Name}");
        }
        else
        {
            Debug.LogError("No se encontró el estado inicial");
            foreach (var state in states)
            {
                var owner = state.GetComponentInParent<State_Machine>();
                Debug.Log($"[StateMachine] Encontré {state.GetType().Name} en {state.gameObject.name}. " +
                        $"Padre con state machine: {(owner ? owner.name : "ninguno")}");
            }
        }
    }






    void Update()
    {
        // verifica si hay un cambio de estado pendiente
        if (nextState != null)
        {   
            currentState = nextState;
            nextState = null;
            hasEnteredState = false;
            
            Debug.Log($"Estado cambiado a: {currentState.GetType().Name}");
        }

        if (currentState != null)
        {
            // Ejecutar EnterState
            if (!hasEnteredState)
            {
                currentState.EnterState();
                hasEnteredState = true;
            }

            // Ejecutar UpdateState cada frame
            currentState.UpdateState();
        }
    }






    public void SetState<T>() where T : State_Base
    {
        var newState = GetComponentInChildren<T>();
        if (newState != null && newState != currentState)
        {
            nextState = newState;
        }
        else if (newState == null)
        {
            Debug.LogError($"No se encontró el componente de estado: {typeof(T).Name}");
        }
    }
}