using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
public class SingleSeaWaveUI : BaseMeshEffect
{
    [Header("Ajustes de Onda")]
    public float amplitude = 15f;
    public float frequency = 0.05f;
    public float speed = 5f;

    [Header("Control de Vida (Recorte)")]
    [Range(0f, 1f)]
    public float healthPercentage = 1f;

    [Header("Optimización")]
    [Tooltip("Veces por segundo que se recalcula la malla. Menos es mejor rendimiento.")]
    public float updatesPerSecond = 30f;

    private Graphic _graphic;
    private RectTransform _rectTransform;
    private Coroutine _updateCoroutine;

    protected override void Awake()
    {
        base.Awake();
        _graphic = GetComponent<Graphic>();
        _rectTransform = GetComponent<RectTransform>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        // Iniciamos la corrutina al activar el objeto
        if (Application.isPlaying)
        {
            _updateCoroutine = StartCoroutine(LowPriorityUpdate());
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // Limpieza de la corrutina para evitar errores de memoria
        if (_updateCoroutine != null)
        {
            StopCoroutine(_updateCoroutine);
            _updateCoroutine = null;
        }
    }

    // Corrutina de "Baja Prioridad"
    private IEnumerator LowPriorityUpdate()
    {
        // Usamos un YieldInstruction pre-calculado para evitar basura en memoria (GC)
        WaitForSeconds wait = new WaitForSeconds(1f / updatesPerSecond);

        while (true)
        {
            if (_graphic != null)
            {
                // Marcamos que los vértices necesitan redibujarse
                _graphic.SetVerticesDirty();
            }
            yield return wait;
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        List<UIVertex> vertices = new List<UIVertex>();
        vh.GetUIVertexStream(vertices);

        float totalHeight = _rectTransform.rect.height;
        float bottomY = _rectTransform.pivot.y * totalHeight * -1f;
        float currentTopY = bottomY + (totalHeight * healthPercentage);

        // Usamos Time.time directamente aquí; la corrutina solo decide CUÁNDO se llama a esta función
        float time = Time.time * speed;

        for (int i = 0; i < vertices.Count; i++)
        {
            UIVertex v = vertices[i];
            float worldX = v.position.x + transform.position.x;
            float wave = Mathf.Sin(worldX * frequency + time) * amplitude;

            // Lógica de recorte
            if (v.position.y > currentTopY)
            {
                v.position.y = currentTopY;
            }

            // Aplicación de onda
            if (v.position.y >= currentTopY - 1f)
            {
                v.position.y += wave;
            }

            vertices[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(vertices);
    }

    public void SetHealth(float percentage)
    {
        healthPercentage = Mathf.Clamp01(percentage);
        // Si el juego está en pausa o la corrutina es muy lenta, 
        // forzamos un refresh inmediato al cambiar la vida
        if (_graphic != null) _graphic.SetVerticesDirty();
    }
}