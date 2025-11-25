using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    [Header("Perforación")]
    [Tooltip("Cantidad de enemigos que puede atravesar (>=1)")]
    public int maxPerforation = 1;

    private int currentPerforation;

    void Start()
    {
        currentPerforation = Mathf.Max(1, maxPerforation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sólo interesan los "enemigos"
        if (!other.CompareTag("Enemy")) return;

        // Si el enemigo tiene un TakeDamage (opcional), lo llamamos
        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(1); // ajusta el daño si hace falta
        }

        currentPerforation--;
        // Si perforación llega a 0 o menos, destruimos el proyectil
        if (currentPerforation <= 0)
        {
            Destroy(gameObject);
        }
        // Si currentPerforation > 0, el trigger permite que el proyectil siga y atraviese
    }

    /// <summary>
    /// Permite asignar la perforación cuando se instancia el proyectil desde el cañón.
    /// </summary>
    public void SetPerforation(int value)
    {
        maxPerforation = Mathf.Max(1, value);
        currentPerforation = Mathf.Max(currentPerforation, maxPerforation);
    }
}

/// <summary>
/// Interfaz opcional que un enemigo puede implementar para recibir daño.
/// (si tu enemigo usa otro método, adapta la llamada).
/// </summary>
public interface IDamageable
{
    void TakeDamage(int amount);
}
