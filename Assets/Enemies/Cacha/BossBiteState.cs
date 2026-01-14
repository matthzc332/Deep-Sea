using UnityEngine;
using System.Collections; // Necesario si usas corrutinas

public class BossBiteState : State_Base
{
    private BossController boss;
    private Transform target;

    // Variables para configurar el ataque (puedes ajustarlas)
    private float biteSpeed = 5f;
    private float stopDistance = 0.5f;

    public override void EnterState()
    {
        // 1. OBTENER EL CONTROLADOR
        boss = controlledObject.GetComponent<BossController>();

        // Chequeo de seguridad: ¿Tiene el script BossController?
        if (boss == null)
        {
            Debug.LogError("ERROR: El objeto no tiene el componente BossController.");
            return;
        }

        // 2. OBTENER EL OBJETIVO DESDE EL CONTROLADOR
        // Aquí es donde probablemente tenías el error en la línea 11
        target = boss.GetTarget();

        // Chequeo de seguridad: ¿Existe el barco?
        if (target == null)
        {
            Debug.LogWarning("BossBiteState: No hay objetivo (Ship). Volviendo a perseguir.");
            state_machine.SetState<BossPursueState>();
            return;
        }

        // Si todo está bien, inicia la lógica del ataque
        // Debug.Log("¡GRRR! Boss inicia mordisco hacia " + target.name);
    }

    public override void UpdateState()
    {
        // Si por alguna razón el target desaparece (se destruye el barco), salimos
        if (target == null || boss == null) return;

        // LÓGICA DEL MORDISCO (Ejemplo simple: ir rápido hacia el barco)
        float distance = Vector2.Distance(controlledObject.transform.position, target.position);

        if (distance > stopDistance)
        {
            // Moverse hacia el barco
            controlledObject.transform.position = Vector2.MoveTowards(
                controlledObject.transform.position,
                target.position,
                biteSpeed * Time.deltaTime
            );
        }
        else
        {
            // Llegó al objetivo: Causar daño y salir del estado
            // Aquí podrías llamar a una función de daño en el barco

            // Volver a perseguir después del mordisco
            state_machine.SetState<BossPursueState>();
        }
    }

    public override void ExitState(string nextState)
    {
        // Limpieza si es necesaria
        // Debug.Log("Terminó el mordisco.");
    }
}