using System.IO.Enumeration;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Objeto data", menuName = "Tienda/Objeto data")]
public class PlantillaObjeto : ScriptableObject
{
    public string nombre;
    public string strongWith;
    public string weakWith;
    public string descripcion;
    public int precio;
    public Sprite[] idleAnimationSprites; // Sprites de la animación idle
    public float animationSpeed = 1f;
}