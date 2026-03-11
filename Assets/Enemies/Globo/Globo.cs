using UnityEngine;

public class Globo : Entity
{
    // Declarar la variable como campo de la clase
    private bool collisionWithShip;

    public void Start()
    {
        speed = 0.9f;
        collisionWithShip = false;
    }

    // Método para obtener el estado de colisión con la nave
    public bool GetCollisionWithShip()
    {
        return collisionWithShip;
    }

    // Método que se ejecuta automáticamente cuando ocurre una colisión

    private void OnCollisionEnter2D(Collision2D collision)
{
    // Si choca con el barco
    if (collision.gameObject.CompareTag("Ship"))
    {
        collisionWithShip = true;
    }
    
    // Si choca con una bala (fuera del if anterior)
    if (collision.gameObject.CompareTag("Bullet"))
    {
        // Aquí podrías activar otra variable como 'isDead' 
        // para que tu StateMachine cambie a Globo_Explotando
        collisionWithShip = true; 
    }
}
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Verificar si el objeto con el que colisionó tiene la etiqueta "Ship"
    //     if (collision.gameObject.CompareTag("Ship"))
    //     {
    //         collisionWithShip = true;
    //         // sonido al colisionar
    //         if (collision.gameObject.CompareTag("Bullet"))//
    //         {
    //             // Aquí podrías tener una variable llamada 'isDead' 
    //             // o simplemente forzar el cambio de estado en tu StateMachine
    //             collisionWithShip = true; // Si usas esta misma variable para activar la explosión
    //         }
    //     }


    }
