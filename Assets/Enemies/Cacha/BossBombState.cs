using UnityEngine;
using System.Collections;

public class BossBombState : State_Base
{
    [Header("Bomb Settings")]
    public GameObject bombPrefab;
    public int bombCount = 4;
    public float throwInterval = 0.3f;
    public float throwForce = 10f;

    private Transform _playerTransform;

    public override void EnterState()
    {
        Debug.Log("INICIANDO RÁFAGA DE DISPAROS");
        
        // 1. Localizar al jugador para saber hacia dónde disparar
        GameObject player = GameObject.FindGameObjectWithTag("Ship");
        if (player != null)
        {
            _playerTransform = player.transform;
        }


        StartCoroutine(ThrowBombs());
    }

    IEnumerator ThrowBombs()
    {
        for (int i = 0; i < bombCount; i++)
        {
            // Instanciar la bomba
            GameObject bomb = Instantiate(bombPrefab, controlledObject.transform.position, Quaternion.identity);

            // 3. CALCULAR DIRECCIÓN
            float directionX = 1f; // Por defecto a la derecha
            if (_playerTransform != null)
            {
                // Si la X del jefe es mayor que la del jugador, el jefe está a la derecha, debe disparar a la izquierda (-1)
                directionX = (controlledObject.transform.position.x > _playerTransform.position.x) ? -1f : 1f;
            }

            // Aplicar fuerza
            Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Multiplicamos la fuerza por la dirección (1 o -1)
                Vector2 force = new Vector2(throwForce * directionX, Random.Range(-2f, 5f));
                rb.AddForce(force, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(throwInterval);
        }

        // Regresar al estado de persecución
        state_machine.SetState<BossPursueState>();
    }

    public override void ExitState(string nextStateName)
    {
        // Resetear rotación al salir para que no se quede girado en el PursueState
        controlledObject.transform.rotation = Quaternion.identity;
    }
}