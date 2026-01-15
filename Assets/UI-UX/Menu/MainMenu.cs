//using UnityEngine;

//public class MainMenu : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Esta función la llamaremos desde el Botón "Jugar" en Unity
    public void Jugar()
    {
        // 1. Verificar si existe el GameManager para evitar errores
        if (GameManager.instance != null)
        {
            // 2. ¡AQUÍ ESTÁ LA CLAVE! Cambiamos el estado a 'OnWave'
            GameManager.instance.currentGameState = GameManager.GameState.OnWave;

            // Asegúrate también de que el tiempo corra (por si estaba pausado)
            Time.timeScale = 1f;

            Debug.Log("Botón pulsado: Estado cambiado a OnWave");
        }
        else
        {
            Debug.LogError("Error: No se encuentra el GameManager en la escena.");
        }

        // 3. Opcional: Desactivar este menú para que no estorbe en la pantalla
        // gameObject.SetActive(false); // Descomenta esto si el menú es un objeto que debe desaparecer
        // Opcional: Desactivar el menú visualmente
        gameObject.SetActive(false);
    }
}