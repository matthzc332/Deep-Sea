using UnityEngine;

public class GlobalSeaScroll : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float scrollSpeed = 5f;
    public float limitX = -23f;
    public float resetX = 24f;

    void Update()
    {
        // Iteramos por todos los hijos directos
        foreach (Transform child in transform)
        {
            // 1. Mover hijo
            child.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

            // 2. Teletransportar si sale de rango
            if (child.position.x <= limitX)
            {
                Vector3 newPos = child.position;
                newPos.x = resetX;
                child.position = newPos;
            }
        }
    }
}