using System.Collections;
using UnityEngine;

public class State_Machine : MonoBehaviour
{
    [SerializeField] private State_Base currentState;
    private State_Base nextState;
    private Coroutine stateMachineCoroutine;

    void Start()
    {
        if (currentState != null)
        {
            // Iniciamos la máquina de estados
            stateMachineCoroutine = StartCoroutine(StateMachineLoop());
        }
    }

    /// <summary>
    /// El corazón de la máquina de estados. 
    /// Corre en paralelo y no bloquea el Update general del juego.
    /// </summary>
    private IEnumerator StateMachineLoop()
    {
        while (true)
        {
            if (currentState == null)
            {
                yield return null; // Espera al siguiente frame si no hay estado
                continue;
            }

            // --- Lógica de Entrada al Estado ---
            currentState.EnterState();

            // --- Lógica de Ejecución (Update del Estado) ---
            // Mientras no haya un cambio de estado pendiente, ejecutamos el UpdateState
            while (nextState == null)
            {
                currentState.UpdateState();

                // Aquí podés controlar la "frecuencia" de actualización.
                // yield return null; // Ejecuta cada frame
                // yield return new WaitForSeconds(0.1f); // Ejecuta 10 veces por segundo (ahorro de recursos)
                yield return null;
            }

            // --- Cambio de Estado ---
            // Si el código llega aquí, es porque nextState ya no es null
            currentState = nextState;
            nextState = null;

            // Al no haber un 'break', el bucle exterior reinicia y llama al EnterState del nuevo estado.
        }
    }

    public void SetState(State_Base newState)
    {
        if (newState != null && newState != currentState)
        {
            nextState = newState;
        }
    }

    public void SetState<T>() where T : State_Base
    {
        var newState = GetComponentInChildren<T>();
        if (newState != null && newState != currentState)
        {
            nextState = newState;
        }
    }

    public State_Base GetCurrentState() => currentState;

    // Limpieza por seguridad
    private void OnDisable()
    {
        if (stateMachineCoroutine != null) StopCoroutine(stateMachineCoroutine);
    }
}