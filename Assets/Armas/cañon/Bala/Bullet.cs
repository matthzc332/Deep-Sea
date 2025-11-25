using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int power = 1;
    [SerializeField] private float spriteForwardOffsetDeg = 0f; // si tu sprite mira +Y, poné -90
    [SerializeField] public State_Machine state_machine;
    public Vector3 enemy;
    public float initialSpeed;
    public Rigidbody2D rb;
    public int pierce = 0;
    public bool collision = false;

    void Awake()
    {
        state_machine = GetComponent<State_Machine>();
        rb = GetComponent<Rigidbody2D>();
    }


    public int getDamage()
    {
        return power;
    }

    public void Initialize(Vector3 enemy, float initialSpeed, int power)
    {
        this.power = power;
        this.enemy = enemy;
        this.initialSpeed = initialSpeed;

    }

    public void LaunchTowards(Vector3 targetWorld, float initialSpeed)
    {
        Vector2 dir = ((Vector2)(targetWorld - transform.position)).normalized;
        rb.linearVelocity = dir * initialSpeed;
        OrientToVelocity();
    }



    public void OrientToVelocity()
    {
        Vector2 v = rb.linearVelocity;
        if (v.sqrMagnitude > 1e-6f)
        {
            float ang = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + spriteForwardOffsetDeg;
            transform.rotation = Quaternion.Euler(0f, 0f, ang);
        }
    }



    protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Trigger con: " + other.name + " (tag: " + other.tag + ")");
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("colisionó un enemigo");
                if(pierce >= 1){
                    pierce = pierce -1;
                }
                else {collision = true;}
                
                // collision = false;
            }
        }


}

