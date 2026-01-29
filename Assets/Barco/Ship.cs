using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class Ship : Entity
{
    private bool esInvulnerable = false; // Controla si puede recibir daño
    private Transform Ships;
    private SpriteRenderer spr;
    public Joystick joystick;

    public float vida;

    // Límites de movimiento
    private float limiteDerecho = 7.300274f;
    private float limiteIzquierdo = -7.300274f;
    public float initialSpeed = 0.9f;
    void Start()
    {
        Ships = GetComponent<Transform>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        speed = initialSpeed;
    }
    
    void Update()
    {
        // Solo mover si el joystick está activo y está dentro de los límites
        if (joystick.angulo != 0f)
        {
            splitSpeed();
            
            // Verificar límites específicos para cada dirección
            if ((speed > 0 && Ships.position.x < limiteDerecho) || 
                (speed < 0 && Ships.position.x > limiteIzquierdo))
            {
                move(speed, Ships);
            }
        }

        vida = HP;
    }

    void move(float speed, Transform body)
    {
        body.Translate(Vector3.right * speed * Time.deltaTime);
        
        // Aplicar límites después del movimiento
        Vector3 pos = body.position;
        pos.x = Mathf.Clamp(pos.x, limiteIzquierdo, limiteDerecho);
        body.position = pos;
    }
    
    public void splitSpeed()
    {
        if (joystick.angulo > 0)
        {
            speed = initialSpeed*-1; // Mover hacia la izquierda
        }
        else if (joystick.angulo < 0)
        {
            speed = initialSpeed; // Mover hacia la derecha
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other); 

            if (other.CompareTag("Enemy"))
            {
            // Debug.Log("Trigger con: " + other.name + " (tag: " + other.tag + ")");
                if (other.CompareTag("Enemy"))
                {
                    //Debug.Log("colisionó un enemigo");
                }
            }
        }

    // public override void takeDamage(int damage)
    // {
    //     if (isAlive)
    //     {
    //         HP -= damage;
    //         if (HP <= 0)
    //         {
    //             isAlive = false;
    //             SceneManager.LoadScene("prefabs");
    //         }
    //     }
    // }

    // modifico take damage para no morir de ataques constantes del jefe
    public override void takeDamage(int damage)
    {
        // 1. Si ya murió o es invulnerable, no hacemos nada
        if (!isAlive || esInvulnerable) return;

        // 2. Aplicamos daño
        HP -= damage;
        
        // Debug para ver que no baje a lo loco
        Debug.Log($"Barco golpeado. Vida restante: {HP}");

        if (HP <= 0)
        {
            isAlive = false;
            SceneManager.LoadScene("prefabs");
        }
        else
        {
            // 3. Si sigue vivo, activamos la invulnerabilidad temporal
            StartCoroutine(RutinaInvulnerabilidad());
        }
    }
    // CORRUTINA NUEVA PARA EL TIEMPO DE GRACIA
    IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;
        
        // Opcional: Hacemos que el barco parpadee (se ponga medio transparente)
        if (spr != null) spr.color = new Color(1f, 1f, 1f, 0.5f);

        // Esperamos 1.5 segundos (puedes cambiar este número)
        yield return new WaitForSeconds(1.5f);

        // Volvemos a la normalidad
        if (spr != null) spr.color = Color.white;
        esInvulnerable = false;
        
        Debug.Log("Barco vulnerable de nuevo.");
    }

}