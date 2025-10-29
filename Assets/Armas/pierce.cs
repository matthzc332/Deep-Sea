using UnityEngine;

public class Pierce : SkillTreeBase
{
    [Header("Configuración - Perforación")]
    [Tooltip("Nivel de perforación que aplicará el arma (afecta solo a proyectiles nuevos).")]
    [Range(1, 3)]
    [SerializeField] private int nivel = 1;

    [SerializeField] private int power = 10; // Daño base
    private int pierce; // Contador de perforaciones restantes

    private void Start()
    {
        pierce = nivel; // Inicializamos el valor de perforación
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si el objeto es un enemigo
        if (!other.CompareTag("Enemy")) return;

        Debug.Log("Bullet: colisionó un enemigo");

        // Intentar aplicar daño si el enemigo implementa IDamageable
        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(power);
        }

        // Si pierce > 0, decrementamos y permitimos que el proyectil siga (atraviesa)
        if (pierce > 0)
        {
            pierce -= 1;
            Debug.Log($"Bullet: atravesó al enemigo. Pierce restante: {pierce}");
            return; // No destruimos el proyectil aún
        }

        // Si pierce == 0 -> este impacto consume y destruye el proyectil
        Debug.Log("Bullet: se destruye tras colisión final");
        Destroy(gameObject, 0.02f);
    }
}