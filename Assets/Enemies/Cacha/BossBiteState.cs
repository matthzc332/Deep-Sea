using UnityEngine;
using DG.Tweening; // ¡Importante para las animaciones!

public class BossBiteState : State_Base
{
    [Header("Configuración de la Emboscada")]
    public float sinkDepth = 10f;       // Cuánto baja para "desaparecer"
    public float riseHeight = 0f;       // A qué altura Y sube para morder (0 suele ser el nivel del mar)
    public float sinkDuration = 1f;     // Tiempo en bajar
    public float biteDuration = 0.5f;   // Tiempo en subir (rápido = ataque sorpesa)
    public float stayBitingTime = 0.5f; // Tiempo que se queda arriba mordiendo
    
    [Header("Efectos")]
    public GameObject bitePrefab;       // El prefab que pediste (ej. salpicadura o dientes)
    public string biteAnimTrigger = "Bite"; // Trigger en el Animator

    private BossController boss;
    private Transform target;
    private Collider2D bossCollider;

    public override void EnterState()
    {
        boss = controlledObject.GetComponent<BossController>();
        bossCollider = controlledObject.GetComponent<Collider2D>();
        target = boss.GetTarget();

        // Si no hay target, abortamos
        if (target == null)
        {
            state_machine.SetState<BossPursueState>();
            return;
        }

        // Iniciamos la secuencia de ataque
        StartAmbushSequence();
    }

    void StartAmbushSequence()
    {
        // Guardamos la posición original X para saber volver (opcional) o simplemente la Y
        float currentX = controlledObject.transform.position.x;
        float currentY = controlledObject.transform.position.y;

        // Desactivar colisiones si quieres que sea invulnerable mientras se hunde (opcional)
        if(bossCollider) bossCollider.enabled = false;

        Sequence ambush = DOTween.Sequence();

        // PASO 1: HUNDIRSE (Bajar verticalmente)
        ambush.Append(controlledObject.transform.DOMoveY(currentY - sinkDepth, sinkDuration).SetEase(Ease.InBack));

        // PASO 2: MOVERSE DEBAJO DEL JUGADOR (Mientras está abajo e invisible)
        ambush.AppendCallback(() => {
            if (target != null)
            {
                // Teletransporte "invisible" en el eje X hacia la posición del barco
                Vector3 ambushPos = new Vector3(target.position.x, controlledObject.transform.position.y, 0);
                controlledObject.transform.position = ambushPos;
            }
        });

        // PASO 3: SURGIR MORDDIENDO (Subir rápido)
        ambush.Append(controlledObject.transform.DOMoveY(riseHeight, biteDuration).SetEase(Ease.OutBack));
        
        // Ejecutar animación y prefab justo cuando sube
        ambush.AppendCallback(() => {
            // Activar colisionador para hacer daño
            if(bossCollider) bossCollider.enabled = true;

            // Animación
            Animator anim = controlledObject.GetComponent<Animator>();
            if (anim) anim.SetTrigger(biteAnimTrigger);

            // Instanciar el PREFAB (El efecto de mordida/agua)
            if (bitePrefab != null)
            {
                Instantiate(bitePrefab, controlledObject.transform.position, Quaternion.identity);
            }
        });

        // PASO 4: ESPERAR UN POCO ARRIBA
        ambush.AppendInterval(stayBitingTime);

        // PASO 5: VOLVER A HUNDIRSE
        ambush.Append(controlledObject.transform.DOMoveY(currentY - sinkDepth, sinkDuration).SetEase(Ease.InBack));

        // PASO 6: REAPARECER EN PERSECUCIÓN
        ambush.OnComplete(() => {
            // Opcional: Teletransportarlo un poco a la derecha/izquierda para que no salga "encima" del jugador al volver a perseguir
            // Vector3 resetPos = new Vector3(target.position.x + 5f, currentY, 0); 
            // controlledObject.transform.position = resetPos;

            if(bossCollider) bossCollider.enabled = true; // Asegurar que tenga colisión al volver
            state_machine.SetState<BossPursueState>();
        });
    }

    public override void ExitState(string nextState)
    {
        // Asegurarnos de que el collider quede activo por si acaso se interrumpe
        if(bossCollider) bossCollider.enabled = true;
    }
}