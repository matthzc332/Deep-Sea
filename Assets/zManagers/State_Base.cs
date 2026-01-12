using UnityEngine;

public class State_Base : MonoBehaviour
{

    //Creo el atributo que me permitira controlar la maquina de estados.
    protected State_Machine state_machine;
    //Creo el atributo que me permitira controlar al objeto que se le aplica el estado
    protected GameObject controlledObject;

    // Buscar el componente State_Machine y controlledObject en la jerarquia del objeto.
    void Start()
    {
        state_machine = GetComponentInParent<State_Machine>();
        controlledObject = transform.parent != null ? transform.parent.gameObject : null;
    }

    // prueba para el boss
    // Usamos Awake para que se configure ANTES de cualquier ataque
    protected virtual void Awake()
    {
        state_machine = GetComponentInParent<State_Machine>();

        // El objeto controlado SIEMPRE es el que tiene la State_Machine
        if (state_machine != null)
        {
            controlledObject = state_machine.gameObject;
        }
    }


    // Metodos virtuales que pueden ser sobrescritos por estados derivados.
    public virtual void EnterState() {} //Se ejecuta cuando inicia el estado.
    public virtual void UpdateState() {} //El "update" del estado.
    public virtual void ExitState(string nextState) {} //Se ejecua al finalizar el estado, se debe acceder desde UpdateState.
}
