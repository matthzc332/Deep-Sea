using UnityEngine;
using DG.Tweening;

public class BossBiteState : State_Base
{
    [Header("Configuración de la Emboscada")]
    public float sinkDepth = 10f;       
    public float riseHeight = 0f;       
    public float sinkDuration = 1f;     
    public float biteDuration = 0.5f;   
    public float stayBitingTime = 0.5f; 
    
    [Header("Efectos")]
    [Tooltip("Deja esto VACÍO si solo quieres que rote.")]
    public GameObject biteEffectPrefab; 
    public string biteAnimTrigger = "Bite";

    private BossController boss;
    private Transform target;
    private Collider2D bossCollider;
    private Animator animator;
    
    // Variables para recordar estado original
    private Quaternion swimRotation; 
    private float startX; // <--- AQUÍ GUARDAMOS LA POSICIÓN X ORIGINAL
    private float startY; // <--- Y LA Y ORIGINAL

    // Rotación vertical (ajusta 180 si sale de cabeza)
    private Vector3 verticalRotation = new Vector3(0, 0, 0); 

    public override void EnterState()
    {
        boss = controlledObject.GetComponent<BossController>();
        bossCollider = controlledObject.GetComponent<Collider2D>();
        animator = controlledObject.GetComponentInChildren<Animator>();
        target = boss.GetTarget();

        // 1. GUARDAR ESTADO ORIGINAL
        swimRotation = controlledObject.transform.rotation;
        startX = controlledObject.transform.position.x; // Guardamos la X (carril de persecución)
        startY = controlledObject.transform.position.y; // Guardamos la Y

        if (target == null)
        {
            state_machine.SetState<BossPursueState>();
            return;
        }

        StartAmbushSequence();
    }

    void StartAmbushSequence()
    {
        float currentY = controlledObject.transform.position.y;
        
        // Desactivar colisiones al hundirse
        if(bossCollider) bossCollider.enabled = false;

        Sequence ambush = DOTween.Sequence();

        // PASO 1: HUNDIRSE (En horizontal)
        ambush.Append(controlledObject.transform.DOMoveY(currentY - sinkDepth, sinkDuration).SetEase(Ease.InBack));

        // PASO 2: MOVERSE DEBAJO Y ROTAR
        ambush.AppendCallback(() => {
            if (target != null)
            {
                // Teletransporte invisible bajo el barco
                Vector3 ambushPos = new Vector3(target.position.x, controlledObject.transform.position.y, 0);
                controlledObject.transform.position = ambushPos;
                
                // Rotar a vertical para el ataque
                controlledObject.transform.rotation = Quaternion.Euler(verticalRotation);
            }
        });

        // PASO 3: SUBIR (Ataque vertical)
        ambush.Append(controlledObject.transform.DOMoveY(riseHeight, biteDuration).SetEase(Ease.OutBack));
        
        // EVENTO DE MORDIDA
        ambush.AppendCallback(() => {
            if(bossCollider) bossCollider.enabled = true; 
            if (animator != null) animator.SetTrigger(biteAnimTrigger);
            if (biteEffectPrefab != null) Instantiate(biteEffectPrefab, controlledObject.transform.position, Quaternion.identity);
        });

        // PASO 4: ESPERAR ARRIBA
        ambush.AppendInterval(stayBitingTime);

        // PASO 5: BAJAR DE NUEVO (Aun en vertical)
        // Bajamos hasta el fondo otra vez
        ambush.Append(controlledObject.transform.DOMoveY(currentY - sinkDepth, sinkDuration).SetEase(Ease.InBack));

        // PASO 6: RETORNO A LA NORMALIDAD
        ambush.OnComplete(() => {
            // A. Recuperar rotación de nado (Horizontal)
            controlledObject.transform.rotation = swimRotation;
            
            // B. ¡EL TRUCO! Teletransportar de vuelta a la posición original X
            // Usamos startY para que aparezca justo donde empezó, o puedes dejarlo abajo y que suba solo.
            controlledObject.transform.position = new Vector3(startX, startY, 0);

            if(bossCollider) bossCollider.enabled = true;
            state_machine.SetState<BossPursueState>();
        });
    }

    public override void ExitState(string nextState)
    {
        // Seguridad por si se interrumpe
        controlledObject.transform.rotation = swimRotation;
        if(bossCollider) bossCollider.enabled = true;
    }
}