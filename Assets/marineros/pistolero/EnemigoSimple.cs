using UnityEngine;

public class EnemigoSimple : MonoBehaviour
{
    private float timer = 3f;
    private bool isMoving = false;

    public float speed = 2f;

    private Animation_Controller anim;

    void Start()
    {
        anim = GetComponent<Animation_Controller>();

        isMoving = true;
        timer = 3f;
    }

    void Update()
    {
        if (isMoving)
        {
            timer -= Time.deltaTime;

            transform.Translate(Vector3.left * speed * Time.deltaTime);

            if (anim != null)
                anim.Play(0); // Animacion movimiento

            if (timer <= 0f)
            {
                isMoving = false;
            }
        }
        else
        {
            if (anim != null)
                anim.Play(1); // Animacion idle si la tienes
        }
    }
}