using UnityEngine;
using System.Collections;

public class BossEcoState : State_Base
{
    [Header("Eco Settings")]
    public GameObject ecoPrefab;
    public int ecoCount = 3;
    public float intervaloDisparo = 0.5f;

    private Transform _playerTransform;

    public override void EnterState()
    {
        Debug.Log("INICIANDO ATAQUE ECO SÓNICO");
        
        // 1. Localizar al jugador (usando tu lógica de Ship)
        GameObject player = GameObject.FindGameObjectWithTag("Ship");
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        StartCoroutine(EmitirEcos());
    }

    IEnumerator EmitirEcos()
    {
        for (int i = 0; i < ecoCount; i++)
        {
            // 2. Instanciar el eco
            GameObject ecoGO = Instantiate(ecoPrefab, controlledObject.transform.position, Quaternion.identity);
            EcoSonicoProyectil ecoScript = ecoGO.GetComponent<EcoSonicoProyectil>();

            if (ecoScript != null)
            {
                // 3. CALCULAR DIRECCIÓN (Igual que en tus bombas)
                float directionX = (controlledObject.transform.position.x > _playerTransform.position.x) ? -1f : 1f;
                
                // Iniciar el proyectil con su dirección y origen
                ecoScript.Inicializar(directionX, controlledObject.transform.position);
            }

            yield return new WaitForSeconds(intervaloDisparo);
        }

        // 4. Regresar al estado de persecución (Controlador de la FSM)
        state_machine.SetState<BossPursueState>();
    }

    public override void ExitState(string nextStateName)
    {
        // Resetear cualquier cambio visual si fuera necesario
    }
}