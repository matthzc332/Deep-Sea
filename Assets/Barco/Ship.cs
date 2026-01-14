using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : Entity
{
    private Transform Ships;
    private SpriteRenderer spr;
    public Joystick joystick;

    public PlayerScripteable datosBarco;

    public float vida;

    // Escena donde se necesita el joystick
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

        // --- CAMBIO 1: CARGAR VIDA AL INICIAR ---
        if (datosBarco != null)
        {
            // Cargamos la vida guardada en el ScriptableObject
            HP = datosBarco.Vida;
            vida = HP; // Actualizamos la variable local también
            Debug.Log("Barco: Vida cargada del ScriptableObject: " + HP);
        }
        else
        {
            Debug.LogError("Barco: ¡No has asignado el 'Datos Barco' (ScriptableObject) en el Inspector!");
        }
        // ----------------------------------------

        // Buscar el joystick apenas nace el barco
        BuscarJoystick();
    }

    void Update()
    {
        // Si no hay joystick, no hacemos nada (evita errores rojos)
        if (joystick == null)
        {
            BuscarJoystick();
            return;
        }

        if (joystick.angulo != 0f)
        {
            splitSpeed();

            if ((speed > 0 && Ships.position.x < limiteDerecho) ||
                (speed < 0 && Ships.position.x > limiteIzquierdo))
            {
                move(speed, Ships);
            }
        }

        // Solo visualización
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

    void BuscarJoystick()
    {
        if (joystick == null)
        {
            // Debug.Log("Barco: Buscando Joystick en la escena...");
            joystick = FindFirstObjectByType<Joystick>();
        }
    }

    void move(float speed, Transform body)
    {
        body.Translate(Vector3.right * speed * Time.deltaTime);

        Vector3 pos = body.position;
        pos.x = Mathf.Clamp(pos.x, limiteIzquierdo, limiteDerecho);
        body.position = pos;
    }

    public void splitSpeed()
    {
        if (joystick == null) return;

        if (joystick.angulo > 0)
        {
            speed = initialSpeed * -1;
        }
        else if (joystick.angulo < 0)
        {
            speed = initialSpeed;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Trigger con: " + other.name + " (tag: " + other.tag + ")");
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

            if (datosBarco != null)
            {
                datosBarco.Vida = (int)HP;
            }
            // ---------------------------------------------

            if (HP <= 0)
            {
                isAlive = false;
                SceneManager.LoadScene("prefabs");
            }
        }
    }
}