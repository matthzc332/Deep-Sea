using UnityEngine;
using DG.Tweening;

public class BossBiteState : State_Base
{
    [Header("Configuración de la Emboscada")]
    public float sinkDepth = 10f;       
    public float targetY = 0.9f;        // Altura de la mordida
    public float sinkDuration = 1f;     
    public float biteDuration = 0.5f;   
    public float stayBitingTime = 0.5f; 
    
    [Header("Efectos")]
    public GameObject biteEffectPrefab; 
    public string biteAnimName = "mordida"; 

    private BossController boss;
    private Collider2D bossCollider;
    private Animator animator;
    
    private float startY; 
    private Sequence ambushSequence;

    protected override void Awake()
    {
        base.Awake();
        boss = controlledObject.GetComponent<BossController>();
        bossCollider = controlledObject.GetComponent<Collider2D>();
        animator = controlledObject.GetComponentInChildren<Animator>();
    }

    public override void EnterState()
    {
        Debug.Log("Cachalote iniciando: ATAQUE DE MORDIDA");
        
        startY = controlledObject.transform.position.y; 

        if (boss.GetTarget() == null)
        {
            state_machine.SetState<BossPursueState>();
            return;
        }

        StartAmbushSequence();
    }

    public override void UpdateState() { }

   void StartAmbushSequence()
{
    if(bossCollider) bossCollider.enabled = false;

    ambushSequence.Kill();
    ambushSequence = DOTween.Sequence();

    // PASO 1: HUNDIRSE
    ambushSequence.Append(controlledObject.transform.DOMoveY(startY - sinkDepth, sinkDuration).SetEase(Ease.InBack));

    // PASO 2: RESETEAR ESCALA Y ROTAR 90°
    ambushSequence.AppendCallback(() => {
        // Forzamos escala positiva para que al rotar 90 deg se vea bien
        Vector3 s = controlledObject.transform.localScale;
        controlledObject.transform.localScale = new Vector3(Mathf.Abs(s.x), s.y, s.z);
        
        controlledObject.transform.position = new Vector3(0f, controlledObject.transform.position.y, 0);
        controlledObject.transform.rotation = Quaternion.Euler(0, 0, 90); 
    });

    // PASO 3: SUBIR VERTICAL
    ambushSequence.Append(controlledObject.transform.DOMoveY(targetY, biteDuration).SetEase(Ease.OutBack));
    
    ambushSequence.AppendCallback(() => {
        if(bossCollider) bossCollider.enabled = true; 
        if (animator != null) animator.Play("CachaPursuit"); // O la de morder
        if (biteEffectPrefab != null) 
            Instantiate(biteEffectPrefab, controlledObject.transform.position, Quaternion.identity);
    });

    // PASO 4: ESPERAR
    ambushSequence.AppendInterval(stayBitingTime);

    // PASO 5: BAJAR EN PERFECTO VERTICAL
    float randomX = Random.value > 0.5f ? 10f : -10f;
    float bottomY = startY - sinkDepth;

    // Bajamos sin rotar (mantiene los 90 grados o la rotación que tenga)
    ambushSequence.Append(controlledObject.transform.DOMoveY(bottomY, sinkDuration).SetEase(Ease.InQuad));

    // PASO 6: MOVERSE LATERALMENTE Y RESETEAR ROTACIÓN
    ambushSequence.Append(controlledObject.transform.DOMoveX(randomX, 0.5f).SetEase(Ease.Linear));
    
    // Al final, el BossPursueState se encargará de re-ajustar el Flip 
    // automáticamente en su primer UpdateState()
    ambushSequence.Join(controlledObject.transform.DORotate(Vector3.zero, 0.3f)); 

    ambushSequence.OnComplete(() => {
        state_machine.SetState<BossPursueState>();
    });
}

    public override void ExitState(string nextState)
    {
        if (ambushSequence != null) ambushSequence.Kill();
        if(bossCollider) bossCollider.enabled = true;
        Debug.Log("Saliendo de Mordida.");
    }
}