using UnityEngine;
using DG.Tweening; // Asegúrate de tener DOTween instalado

public class BossPursueState : State_Base
{
    [Header("Settings")]
    [SerializeField] private float timeBeforeNextAttack = 3f;
    [SerializeField] private float entrySinkDepth = -2f; // Profundidad a la que se hunde al entrar
    [SerializeField] private float targetY = 0.5f;       // Altura final de persecución
    [SerializeField] private float transitionDuration = 1f;

    private Animator _animator;
    private Transform _playerTransform;
    private float _timer;
    private Sequence _entrySequence;
    private Vector3 _originalScale;

    protected override void Awake()
    {
        base.Awake();
        _originalScale = controlledObject.transform.localScale; // Guardamos (Ej: 2.5, 2.5, 1)
        _animator = controlledObject.GetComponentInChildren<Animator>();
    }

    public override void EnterState()
    {
        Debug.Log("Cachalote iniciando persecución con transición vertical.");
        _timer = 0f;

        // 1. Animación
        if (_animator != null)
        {
            _animator.Play("CachaPursuit");
        }

        // 2. Buscar jugador
        GameObject player = GameObject.FindGameObjectWithTag("Ship");
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        // 3. Reset de rotación
        controlledObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        // 4. LÓGICA DE HUNDIRSE Y SUBIR A Y: 0.5
        // Cancelamos cualquier movimiento previo para evitar conflictos
        _entrySequence.Kill();
        _entrySequence = DOTween.Sequence();

        // Primero se hunde un poco desde su posición actual (opcional, para dar impacto)
        _entrySequence.Append(controlledObject.transform.DOMoveY(entrySinkDepth, transitionDuration / 2).SetEase(Ease.InQuad));
        
        // Luego sube a la posición Y 0.5 que pediste
        _entrySequence.Append(controlledObject.transform.DOMoveY(targetY, transitionDuration).SetEase(Ease.OutQuad));
    }

    public override void UpdateState()
    {
        if (_playerTransform == null) return;

        _timer += Time.deltaTime;

        if (_timer >= timeBeforeNextAttack)
        {
            SelectRandomAttack();
            return; 
        }

        HandleFlip();
    }

    private void HandleFlip()
    {
        if (_playerTransform == null) return;

        float direction = _playerTransform.position.x - controlledObject.transform.position.x;

        // Usamos el valor absoluto de la escala original para evitar errores de signos previos
        float absX = Mathf.Abs(_originalScale.x);
        
        if (direction < 0)
        {
            controlledObject.transform.localScale = new Vector3(-absX, _originalScale.y, _originalScale.z);
        }
        else
        {
            controlledObject.transform.localScale = new Vector3(absX, _originalScale.y, _originalScale.z);
        }
    }

    private void SelectRandomAttack()
    {
        // Definimos probabilidades (puedes mover esto a variables serializadas)
        float chanceBomb = 35f;
        float chanceBite = 30f;
        float chanceJump = 35f;

        float total = chanceBomb + chanceBite + chanceJump;
        float randomPoint = Random.Range(0, total);

        if (randomPoint < chanceBomb)
        {
            state_machine.SetState<BossEcoState>();
        }
        else if (randomPoint < chanceBomb + chanceBite)
        {
            state_machine.SetState<BossBiteState>();
        }
        else
        {
            state_machine.SetState<BossJumpState>();
        }
    }

    public override void ExitState(string nextStateName)
    {
        // Es importante matar la secuencia al salir para que no siga moviendo al jefe
        // si el siguiente estado quiere tomar el control de la posición Y inmediatamente.
        _entrySequence.Kill();
        Debug.Log($"Saliendo de Persecución hacia: {nextStateName}");
    }
}