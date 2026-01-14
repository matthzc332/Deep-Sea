using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : Entity
{
    [Header("Referencias")]
    private Transform Ships;
    private SpriteRenderer spr;
    public Joystick joystick;

    [Header("Datos Persistentes")]
    public PlayerScripteable datosBarco; // Referencia al Scriptable Object

    [Header("Estado")]
    public float vida; // Variable visual para ver en el inspector

    [Header("Configuración")]
    public string gameSceneName = "GAME"; // Escena donde se necesita el joystick
    public float initialSpeed = 0.9f;

    // Límites de movimiento
    private float limiteDerecho = 7.300274f;
    private float limiteIzquierdo = -7.300274f;

    void Start()
    {
        Ships = GetComponent<Transform>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        speed = initialSpeed;

        // 1. CARGAR VIDA DEL SCRIPTABLE OBJECT
        if (datosBarco != null)
        {
            HP = datosBarco.Vida;
            vida = HP; // Actualizamos la variable visual
            // Debug.Log("Barco: Vida cargada del ScriptableObject: " + HP);
        }
        else
        {
            Debug.LogError("Barco: ¡No has asignado el 'Datos Barco' (PlayerScripteable) en el Inspector!");
        }

        // 2. BUSCAR JOYSTICK
        BuscarJoystick();
    }

    void Update()
    {
        // Seguridad: Si no hay joystick, intentamos buscarlo y no ejecutamos movimiento
        if (joystick == null)
        {
            BuscarJoystick();
            return;
        }

        // Lógica de Movimiento
        if (joystick.angulo != 0f)
        {
            splitSpeed();

            // Verificar límites específicos para cada dirección antes de mover
            if ((speed > 0 && Ships.position.x < limiteDerecho) ||
                (speed < 0 && Ships.position.x > limiteIzquierdo))
            {
                move(speed, Ships);
            }
        }

        // Actualizar variable visual para el inspector
        vida = HP;
    }

    // --- GESTIÓN DE ESCENAS PARA ENCONTRAR EL JOYSTICK ---
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

    void BuscarJoystick()
    {
        if (joystick == null)
        {
            joystick = FindFirstObjectByType<Joystick>();
        }
    }
    // -----------------------------------------------------

    // --- LÓGICA DE MOVIMIENTO ---
    void move(float speed, Transform body)
    {
        body.Translate(Vector3.right * speed * Time.deltaTime);

        // Clamp para asegurar que no se pase de los límites ni un píxel
        Vector3 pos = body.position;
        pos.x = Mathf.Clamp(pos.x, limiteIzquierdo, limiteDerecho);
        body.position = pos;
    }

    public void splitSpeed()
    {
        if (joystick == null) return;

        if (joystick.angulo > 0)
        {
            speed = initialSpeed * -1; // Izquierda
        }
        else if (joystick.angulo < 0)
        {
            speed = initialSpeed; // Derecha
        }
    }

    // --- COLISIONES Y DAÑO ---
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Colisionó un enemigo");
        }
    }

    public override void takeDamage(int damage)
    {
        if (isAlive)
        {
            HP -= damage;

            // ACTUALIZAR SCRIPTABLE OBJECT AL RECIBIR DAÑO
            if (datosBarco != null)
            {
                datosBarco.Vida = (int)HP;
            }

            // MORIR
            if (HP <= 0)
            {
                isAlive = false;
                // Cargar escena de menú o reinicio (asegúrate que el nombre sea correcto)
                SceneManager.LoadScene("prefabs");
            }
        }
    }
}