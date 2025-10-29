using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : Entity
{
    private Transform Ships;
    private SpriteRenderer spr;
    public Joystick joystick;

    public float vida;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Ships = GetComponent<Transform>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        speed = 0.7f;
    }
    void Update()
    {
        if (joystick.angulo != 0f)
        {
            move(speed, Ships);
        }
        splitSpeed();

        vida = HP;
    }

    void move(float speed, Transform body)
    {
        body.Translate(Vector3.right * speed * Time.deltaTime);
    }
    public void splitSpeed()
    {
        if (joystick.angulo > 0)
        {
            speed = -0.7f;
        }
        else if (joystick.angulo < 0)
        {
            speed = 0.9f;
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
                SceneManager.LoadScene("Scene1");
            }
        }
    }
}