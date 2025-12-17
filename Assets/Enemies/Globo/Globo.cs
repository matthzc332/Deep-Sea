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
        // Verificar si el objeto con el que colisionó tiene la etiqueta "Ship"
        if (collision.gameObject.CompareTag("Ship"))
        {
            collisionWithShip = true;
        }
    }
    
}