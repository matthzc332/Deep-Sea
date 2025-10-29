using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject mob;
    public int percent_probability;
    float tiempo = 0;

    void Update()
    {
        tiempo = tiempo + Time.deltaTime;

        if (tiempo >= 1)
        {
            tiempo = 0;
            int numero1 = Random.Range(1, 100);
            if (numero1 <= percent_probability)
            {
                Instantiate(mob, transform.position, Quaternion.identity);
            }
        }
    }
}
