using UnityEngine;

public class GlobalSeaScroll : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float scrollSpeed = 5f;
    public float limitX = -23f;
    public float resetX = 24f;

    void Update()
    {
        float distance = scrollSpeed * Time.deltaTime;

        foreach (Transform child in transform)
        {
            // 1. Mover
            child.position += Vector3.left * distance;

            // 2. Teletransportar compensando el error
            if (child.position.x <= limitX)
            {
                // Calculamos cuánto se pasó del límite
                float overflow = limitX - child.position.x;

                // Reposicionamos respecto al resetX pero manteniendo el overflow
                Vector3 newPos = child.position;
                newPos.x = resetX - overflow;
                child.position = newPos;
            }
        }
    }
}