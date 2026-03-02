using UnityEngine;
using System.Collections;

public class BossEcoState : State_Base 
{
    [Header("Eco Settings")]
    public GameObject ecoPrefab;
    public int ecoCount = 3;
    public float throwInterval = 0.5f;

    private Transform _playerTransform;

    public override void EnterState()
    {
        Debug.Log("INICIANDO ATAQUE ECO SÓNICO");
        
        GameObject player = GameObject.FindGameObjectWithTag("Ship");
        if (player != null) _playerTransform = player.transform;

        StartCoroutine(EmitirEcos());
    }

    IEnumerator EmitirEcos()
    {
        for (int i = 0; i < ecoCount; i++)
        {
            GameObject ecoGO = Instantiate(ecoPrefab, controlledObject.transform.position, Quaternion.identity);
            
            // BUSCAMOS EL SCRIPT DEL PROYECTIL (EcoSonicoProyectil)
            EcoSonicoProyectil ecoScript = ecoGO.GetComponent<EcoSonicoProyectil>();
            if (ecoScript != null)
            {
                float directionX = (controlledObject.transform.position.x > _playerTransform.position.x) ? -1f : 1f;
                ecoScript.Inicializar(directionX, controlledObject.transform.position);
            }

            yield return new WaitForSeconds(throwInterval);
        }

        state_machine.SetState<BossPursueState>();
    }
}