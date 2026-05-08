using UnityEngine;

public class Cofre_MoveState : State_Base
{
    [SerializeField] private Animation_Controller animController;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private int animationIndex = 0; // Índice en el Animation_Controller

    public override void EnterState()
    {
        // Al entrar al estado, reproducimos la animación
        if (animController != null)
        {
            animController.Play(animationIndex);
        }
    }

    public override void UpdateState()
    {
        // Lógica de movimiento que antes estaba en Cofre.cs
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > 15f)
        {
            Destroy(gameObject);
        }
    }
}