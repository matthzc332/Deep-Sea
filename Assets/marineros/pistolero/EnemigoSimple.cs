using UnityEngine;

public class EnemigoSimple : MonoBehaviour
{
    private float timer = 3f;
    private bool isMoving = false;
    public float speed = 2f; // Velocidad de movimiento

    void Start()
    {
        // Iniciar el movimiento automáticamente al empezar
        isMoving = true;
        timer = 3f;
    }

    void Update()
    {
        if (isMoving)
        {
            // Reducir el timer
            timer -= Time.deltaTime;
            
            // Mover a la izquierda
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            
            // Detener el movimiento cuando el timer llegue a 0
            if (timer <= 0f)
            {
                isMoving = false;
                Debug.Log("El enemigo dejó de moverse");
            }
        }
    }
}