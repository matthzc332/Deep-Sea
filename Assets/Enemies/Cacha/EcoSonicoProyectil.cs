using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class EcoSonicoProyectil : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 6f;
    public float duracionVida = 2.5f;
    public Vector3 escalaFinal = new Vector3(4f, 2.5f, 1f);

    [Header("Daño")]
    public int dañoMaximo = 4;
    public float distanciaParaDañoMinimo = 12f;
    
    private Vector3 _posicionOrigen;
    private float _direccionX;
    
    // Lista para registrar quién ya recibió daño de esta onda
    private List<GameObject> objetosDañados = new List<GameObject>();

    public void Inicializar(float direccion, Vector3 origen)
    {
        _direccionX = direccion;
        _posicionOrigen = origen;

        // Orientación
        if (_direccionX < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.identity;

        // --- EFECTO DOTWEEN: Crecimiento y Punch ---
        transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        
        // 1. Crecimiento fluido hacia la escala final
        transform.DOScale(escalaFinal, duracionVida).SetEase(Ease.OutQuad).SetUpdate(true);
        
        // 2. Efecto de "vibración" sonora al nacer (opcional, le da fuerza)
        transform.DOPunchRotation(new Vector3(0, 0, 10), 0.5f, 10, 1).SetUpdate(true);

        // Desvanecimiento
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if(spr != null)
        {
            // El eco se vuelve más transparente conforme se aleja
            spr.DOFade(0, duracionVida).SetEase(Ease.InExpo).OnComplete(() => Destroy(gameObject)).SetUpdate(true);
        }
        else Destroy(gameObject, duracionVida);
    }

    void Update()
    {
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si es el barco y NO lo hemos dañado todavía
        if (other.CompareTag("Ship") && !objetosDañados.Contains(other.gameObject))
        {
            objetosDañados.Add(other.gameObject); // Lo registramos

            float distancia = Vector3.Distance(_posicionOrigen, transform.position);
            float factor = 1f - Mathf.Clamp01(distancia / distanciaParaDañoMinimo);
            int dañoFinal = Mathf.Max(1, Mathf.RoundToInt(dañoMaximo * factor));

            Ship barco = other.GetComponent<Ship>();
            if (barco != null)
            {
                barco.takeDamage(dañoFinal);
                
                // --- EFECTO DOTWEEN AL IMPACTAR EL BARCO ---
                // El eco hace un pequeño "flash" o pulso cuando toca al jugador
                transform.DOPunchScale(Vector3.one * 0.2f, 0.2f).SetUpdate(true);
            }
            
            // IMPORTANTE: Ya no llamamos a Destroy(gameObject) aquí
            // para que el proyectil siga su camino y atraviese el barco.
        }
    }
}