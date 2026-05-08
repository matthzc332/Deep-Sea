using UnityEngine;

[CreateAssetMenu(fileName = "ShipData", menuName = "Scriptable Objects/ShipData")]
public class ShipData : ScriptableObject
{
    [Header("Estadísticas")]
    public int puntosDeVida;
    public int municion = 50;
    public int dinero;

    [Header("Puntuación")]
    public int score;          // Puntaje acumulado
    public int balasGastadas;  // Contador de disparos

   

    [Header("Prefabs de Posiciones")]
    public GameObject posicion1;
    public GameObject posicion2;
    

     // Método útil para reiniciar datos al empezar el juego
    public void ResetRunData()
    {
        score = 0;
        balasGastadas = 0;
        // Reiniciar vida o munición si es necesario
    }
}