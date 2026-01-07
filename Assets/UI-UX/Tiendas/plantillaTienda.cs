using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nueva Tienda", menuName = "Tienda/Tienda Data")]
public class TiendaData : ScriptableObject
{
    [Tooltip("Arrastra aquí los ScriptableObjects de tipo PlantillaObjeto")]
    public List<PlantillaObjeto> listaProductos; 
}