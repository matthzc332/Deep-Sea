using UnityEngine;

[System.Serializable]
public class MarineroShopping
{
    public string nombre;
    public Sprite retrato;
    public string ventajasGenerales;
    [TextArea(2, 6)]
    public string descripcion;
    public int precio;
}