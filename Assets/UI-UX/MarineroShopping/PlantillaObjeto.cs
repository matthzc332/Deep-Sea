using System.IO.Enumeration;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu (fileName = "objeto", menuName = "Objeto Tienda")]

public class PlantillaObjeto : ScriptableObject
{
    public Sprite imagenObjeto;
    public string textoObjeto;
    public int precioObjeto;
}
