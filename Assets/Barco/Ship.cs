using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : Entity
{
    private Transform Ships;
    private SpriteRenderer spr;
    public Joystick joystick;

    public float vida;

    //Escena donde se necesita el joystick
    public string gameSceneName = "GAME";

    // Límites de movimiento
    private float limiteDerecho = 7.300274f;
    private float limiteIzquierdo = -7.300274f;
    public float initialSpeed = 0.9f;

    void Start()
    {
        Ships = GetComponent<Transform>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        speed = initialSpeed;

        // 1. SOLUCIÓN: Buscar el joystick apenas nace el barco
        BuscarJoystick();
    }

    void Update()
    {
        // 2. SOLUCIÓN: Si no hay joystick, no hacemos nada (evita errores rojos)
        if (joystick == null)
        {
            // Intentamos buscarlo de nuevo por si acaso apareció recién
            BuscarJoystick();
            return;
        }

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameSceneName)
        {
            BuscarJoystick();
        }
    }

    // --- CAMBIÉ EL NOMBRE PARA QUE SEA COHERENTE ---
    void BuscarJoystick()
    {
        // Solo buscamos si la variable está vacía
        if (joystick == null)
        {
            Debug.Log("Barco: Buscando Joystick en la escena...");
            // Nota: FindFirstObjectByType es lento, pero en Start está bien.
            joystick = FindFirstObjectByType<Joystick>();
        }
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
        // Aquí también protegemos por si acaso
        if (joystick == null) return;

        if (joystick.angulo > 0)
        {
            speed = initialSpeed * -1; // Mover hacia la izquierda
        }
        else if (joystick.angulo < 0)
        {
            speed = initialSpeed; // Mover hacia la derecha
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger con: " + other.name + " (tag: " + other.tag + ")");
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("colisionó un enemigo");
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
}