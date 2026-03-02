// using UnityEngine;
// using System.Collections;

// public class BossBombState : State_Base
// {
//     [Header("Bomb Settings")]
//     public GameObject bombPrefab;
//     public int bombCount = 4;
//     public float throwInterval = 0.3f;
//     public float throwForce = 10f;

//     private Transform _playerTransform;

//     public override void EnterState()
//     {
//         Debug.Log("INICIANDO RÁFAGA DE DISPAROS");
        
//         // 1. Localizar al jugador para saber hacia dónde disparar
//         GameObject player = GameObject.FindGameObjectWithTag("Ship");
//         if (player != null)
//         {
//             _playerTransform = player.transform;
//         }


//         StartCoroutine(ThrowBombs());
//     }

//     IEnumerator ThrowBombs()
//     {
//         for (int i = 0; i < bombCount; i++)
//         {
//             // Instanciar la bomba
//             GameObject bomb = Instantiate(bombPrefab, controlledObject.transform.position, Quaternion.identity);

//             // 3. CALCULAR DIRECCIÓN
//             float directionX = 1f; // Por defecto a la derecha
//             if (_playerTransform != null)
//             {
//                 // Si la X del jefe es mayor que la del jugador, el jefe está a la derecha, debe disparar a la izquierda (-1)
//                 directionX = (controlledObject.transform.position.x > _playerTransform.position.x) ? -1f : 1f;
//             }

//             // Aplicar fuerza
//             Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
//             if (rb != null)
//             {
//                 // Multiplicamos la fuerza por la dirección (1 o -1)
//                 Vector2 force = new Vector2(throwForce * directionX, Random.Range(-2f, 5f));
//                 rb.AddForce(force, ForceMode2D.Impulse);
//             }

//             yield return new WaitForSeconds(throwInterval);
//         }

//         // Regresar al estado de persecución
//         state_machine.SetState<BossPursueState>();
//     }

//     public override void ExitState(string nextStateName)
//     {
//         // Resetear rotación al salir para que no se quede girado en el PursueState
//         controlledObject.transform.rotation = Quaternion.identity;
//     }
// }


using UnityEngine;
using DG.Tweening;

public class EcoSonicoProyectil : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 6f;
    public float duracionVida = 2.5f;
    public Vector3 escalaFinal = new Vector3(3f, 2f, 1f);

    [Header("Configuración de Daño")]
    public int dañoMaximo = 4;
    public float distanciaParaDañoMinimo = 12f;
    
    private Vector3 _posicionOrigen;
    private float _direccionX;

    public void Inicializar(float direccion, Vector3 origen)
    {
        _direccionX = direccion;
        _posicionOrigen = origen;

        // Efecto visual: Empieza invisible y pequeño, crece y aparece
        transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        transform.DOScale(escalaFinal, duracionVida).SetEase(Ease.OutQuad);
        
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr != null)
        {
            spr.DOFade(0, duracionVida).SetDelay(duracionVida * 0.7f).OnComplete(() => Destroy(gameObject));
        }
        else
        {
            Destroy(gameObject, duracionVida);
        }
    }

    void Update()
    {
        // Movimiento lineal en la dirección calculada
        transform.Translate(Vector2.right * _direccionX * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ship"))
        {
            float distanciaRecorrida = Vector3.Distance(_posicionOrigen, transform.position);
            
            // Lógica: Más lejos = Menos daño
            // Calculamos un factor de 0 a 1 basado en la distancia
            float factorCercania = 1f - Mathf.Clamp01(distanciaRecorrida / distanciaParaDañoMinimo);
            
            // Daño proporcional (mínimo 1)
            int dañoFinal = Mathf.Max(1, Mathf.RoundToInt(dañoMaximo * factorCercania));

            Ship barco = other.GetComponent<Ship>();
            if (barco != null)
            {
                barco.takeDamage(dañoFinal);
                Debug.Log($"Eco impactó a distancia {distanciaRecorrida}. Daño aplicado: {dañoFinal}");
            }

            // El eco atraviesa al jugador o se destruye? 
            // Si es un "eco", suele ser mejor que lo atraviese, pero aquí lo destruimos para ser claros:
            Destroy(gameObject);
        }
    }
}