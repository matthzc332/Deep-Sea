using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : Entity
{
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
           // Debug.Log("Trigger con: " + other.name + " (tag: " + other.tag + ")");
            if (other.CompareTag("Enemy"))
            {
                //Debug.Log("colisionó un enemigo");
            }
        }

    public override void takeDamage(int damage)
    {
        if (isAlive)
        {
            HP -= damage;
            if (HP <= 0)
            {
                isAlive = false;
                SceneManager.LoadScene("prefabs");
            }
        }
    }

    public void InicializarStats(int vidaInicial)
    {
        HP = vidaInicial;
        vida = vidaInicial;
        isAlive = true;
    }
    
}