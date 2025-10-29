using UnityEngine;

[System.Serializable]
public class MarineroShopping
{
    public string nombre;
    public string ventajasGenerales;
    [TextArea(2, 6)]
    public string descripcion;
    public int precio;
}