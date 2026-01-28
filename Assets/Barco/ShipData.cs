using UnityEngine;

[CreateAssetMenu(fileName = "ShipData", menuName = "Scriptable Objects/ShipData")]
public class ShipData : ScriptableObject
{
    [Header("Estadísticas")]
    public int puntosDeVida;
    public int municion;

    [Header("Prefabs de Posiciones")]
    public GameObject posicion1;
    public GameObject posicion2;
}