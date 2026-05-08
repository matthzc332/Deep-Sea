// using UnityEngine;
// using System.Collections;
//funciona 
// public class Entity : MonoBehaviour
// {
//     [SerializeField] public float HP;
//     [SerializeField] protected float speed;
//     [SerializeField] protected bool isAlive = true;

//     protected bool collision_with_ship = false;

//     public float getSpeed() { return speed; }
//     public float getHP() { return HP; }
//     public bool getIsAlive() { return isAlive; }
//     public bool getCollisionWithShip() { return collision_with_ship; }

//     private SpriteRenderer spriteRenderer;
//     private Color originalColor;
//     private bool isRetreating = false;
//     public float retreatSpeed = 1;

//     protected virtual void Awake()
//     {
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         if (spriteRenderer != null)
//         {
//             originalColor = spriteRenderer.color;
//         }

//         if (GameManager.instance != null)
//         {
//             HP += GameManager.difficultyLevel;
//         }
//     }

//     public virtual void takeDamage(int damage)
//     {
//         if (isAlive)
//         {
//             HP -= damage;

//             if (HP > 0)
//             {
//                 StartCoroutine(DamageEffectRoutine());
//             }
//             // Dentro de takeDamage, en la parte de muerte del enemigo:
//             else
//             {
//                 isAlive = false;
//                 if (CompareTag("Enemy"))
//                 {
//                     Ship playerShip = FindFirstObjectByType<Ship>();
//                     if (playerShip != null && playerShip.shipData != null)
//                     {
//                         playerShip.shipData.score += 10;
//                     }

//                     // NOTA: Usamos RegisterEnemyKill para que cuente y verifique logros de cantidad
//                     if (AchievementManager.instance != null)
//                     {
//                         AchievementManager.instance.RegisterEnemyKill();
//                         Destroy(gameObject); // Asegúrate de destruir el objeto al morir
//                     }

                    
//                 }

//                 // if (CompareTag("Enemy")) 
//                 // {
//                 //     // Buscamos el barco para acceder al ShipData (Forma segura)
//                 //     Ship playerShip = FindFirstObjectByType<Ship>();
//                 //     if (playerShip != null && playerShip.shipData != null)
//                 //     {
//                 //         playerShip.shipData.score += 10;
//                 //         Debug.Log("Enemigo eliminado. Puntos +10. Total: " + playerShip.shipData.score);
//                 //     }

//                 //     // NUEVO: Notificar al sistema de logros
//                 // if (AchievementManager.instance != null)
//                 // {
//                 //     AchievementManager.instance.CheckLogro("first_kill");
//                 // }


//             }
//         }
//     }

//     private IEnumerator DamageEffectRoutine()
//     {
//         if (spriteRenderer == null) yield break;
//         spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f);
//         yield return new WaitForSeconds(0.3f);
//         spriteRenderer.color = originalColor;
//     }

//     // MÉTODO CORREGIDO: Sin lógica de balas para evitar fuego amigo
//     protected virtual void OnTriggerEnter2D(Collider2D collision)
//     {
//         // La lógica de daño por balas ahora vive solo en Bullet.cs

//         if (collision.CompareTag("Ship"))
//         {
//             collision_with_ship = true;
//         }
//     }



//     //###############   Solo para enemigos   #################
//     public void StartRetreat(float speed)
//     {
//         // Evitamos disparar la corrutina dos veces si ya está en marcha
//         if (isRetreating) return;

//         isRetreating = true;
//         retreatSpeed = speed;

//         // Desactivamos colisiones para que atraviesen todo al irse
//         //if (TryGetComponent(out Collider2D col)) col.enabled = false;
//         //if (TryGetComponent(out Rigidbody2D rb)) rb.simulated = false; // Opcional: frena la física

//         StartCoroutine(RetreatRoutine());
//     }

//     private IEnumerator RetreatRoutine()
//     {
//         // El bucle ahora es infinito (mientras el objeto exista)
//         while (true)
//         {
//             transform.Translate(Vector2.left * retreatSpeed * Time.deltaTime);
//             yield return null;
//         }

//     }
// }


// using UnityEngine;
// using System.Collections;

// public class Entity : MonoBehaviour
// {
//     [SerializeField] public float HP;
//     [SerializeField] protected float speed;
//     [SerializeField] protected bool isAlive = true;

//     protected bool collision_with_ship = false;

//     public float getSpeed() { return speed; }
//     public float getHP() { return HP; }
//     public bool getIsAlive() { return isAlive; }
//     public bool getCollisionWithShip() { return collision_with_ship; }

//     private SpriteRenderer spriteRenderer;
//     private Color originalColor;
//     private bool isRetreating = false;
//     public float retreatSpeed = 1;

//     protected virtual void Awake()
//     {
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         if (spriteRenderer != null)
//             originalColor = spriteRenderer.color;

//         if (GameManager.instance != null)
//             HP += GameManager.difficultyLevel;
//     }
// public virtual void takeDamage(int damage)
// {
//     if (!isAlive) return;

//     HP -= damage;

//     if (HP > 0)
//     {
//         StartCoroutine(DamageEffectRoutine());
//     }
//     else
//     {
//         isAlive = false;

//         if (CompareTag("Enemy"))
//         {
//             // Sumar puntos
//             Ship playerShip = FindFirstObjectByType<Ship>();
//             if (playerShip != null && playerShip.shipData != null)
//             {
//                 playerShip.shipData.score += 10;
//                 Debug.Log("Enemigo eliminado. Puntos +10. Total: " + playerShip.shipData.score);
//             }

//             // Notificar al sistema de logros
//             if (AchievementManager.instance != null)
//             {
//                 AchievementManager.instance.RegisterEnemyKill();
//             } // ← llave que faltaba

//         } // ← cierra CompareTag("Enemy")

//         Destroy(gameObject); // ← siempre se destruye, independiente del AchievementManager

//     } // ← cierra else
// }
//     // public virtual void takeDamage(int damage)
//     // {
//     //     if (!isAlive) return;

//     //     HP -= damage;

//     //     if (HP > 0)
//     //     {
//     //         StartCoroutine(DamageEffectRoutine());
//     //     }
//     //     else
//     //     {
//     //         isAlive = false;

//     //         if (CompareTag("Enemy"))
//     //         {
//     //             // Sumar puntos
//     //             Ship playerShip = FindFirstObjectByType<Ship>();
//     //             if (playerShip != null && playerShip.shipData != null)
//     //             {
//     //                 playerShip.shipData.score += 10;
//     //                 Debug.Log("Enemigo eliminado. Puntos +10. Total: " + playerShip.shipData.score);
//     //             }

//     //             // Notificar al sistema de logros
//     //             if (AchievementManager.instance != null)
//     //             {
//     //                 AchievementManager.instance.RegisterEnemyKill();
//     //             //ultimo agregado
//     //             Destroy(gameObject);
//     //         }
//     //     }
//     // }}

//     private IEnumerator DamageEffectRoutine()
//     {
//         if (spriteRenderer == null) yield break;
//         spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f);
//         yield return new WaitForSeconds(0.3f);
//         spriteRenderer.color = originalColor;
//     }

//     protected virtual void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.CompareTag("Ship"))
//             collision_with_ship = true;
//     }

//     public void StartRetreat(float speed)
//     {
//         if (isRetreating) return;
//         isRetreating = true;
//         retreatSpeed = speed;
//         StartCoroutine(RetreatRoutine());
//     }

//     private IEnumerator RetreatRoutine()
//     {
//         while (true)
//         {
//             transform.Translate(Vector2.left * retreatSpeed * Time.deltaTime);
//             yield return null;
//         }
//     }
// }

//claudia

using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    [SerializeField] public float HP;
    [SerializeField] protected float speed;
    [SerializeField] protected bool isAlive = true;

    protected bool collision_with_ship = false;

    public float getSpeed() { return speed; }
    public float getHP() { return HP; }
    public bool getIsAlive() { return isAlive; }
    public bool getCollisionWithShip() { return collision_with_ship; }

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isRetreating = false;
    public float retreatSpeed = 1;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (GameManager.instance != null)
            HP += GameManager.difficultyLevel;
    }

    public virtual void takeDamage(int damage)
    {
        if (!isAlive) return;

        HP -= damage;

        if (HP > 0)
        {
            StartCoroutine(DamageEffectRoutine());
        }
        else
        {
            isAlive = false;

            if (CompareTag("Enemy"))
            {
                // Suma puntos
                Ship playerShip = FindFirstObjectByType<Ship>();
                if (playerShip != null && playerShip.shipData != null)
                    playerShip.shipData.score += 10;
                Debug.Log("Entity: enemigo muerto, llamando GameEvents.EnemyKilled()");
                // Solo avisa que pasó algo — no sabe nada de logros
                GameEvents.EnemyKilled();
            }
        }
    }

    private IEnumerator DamageEffectRoutine()
    {
        if (spriteRenderer == null) yield break;
        spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f);
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = originalColor;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ship"))
            collision_with_ship = true;
    }

    public void StartRetreat(float speed)
    {
        if (isRetreating) return;
        isRetreating = true;
        retreatSpeed = speed;
        StartCoroutine(RetreatRoutine());
    }

    private IEnumerator RetreatRoutine()
    {
        while (true)
        {
            transform.Translate(Vector2.left * retreatSpeed * Time.deltaTime);
            yield return null;
        }
    }
}