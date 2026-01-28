using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject mob;
    public int percent_probability;
    float tiempo = 0;

    void Update()
    {
        // 1. Verificamos si el GameManager existe y si el estado es exactamente "OnWave"
        // Si no estamos en oleada, el código de abajo no se ejecuta.
        if (GameManager.instance == null || GameManager.instance.currentGameState != GameManager.GameState.OnWave)
        {
            return; 
        }

        tiempo = tiempo + Time.deltaTime;

        if (tiempo >= 1)
        {
            tiempo = 0;
            int numero1 = Random.Range(1, 101); // 101 para que incluya el 100
            if (numero1 <= percent_probability)
            {
                Instantiate(mob, transform.position, Quaternion.identity);
            }
        }
    }
}