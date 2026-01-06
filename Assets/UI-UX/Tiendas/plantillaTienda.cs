using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tienda data", menuName = "Tienda/tienda data")]
public class TiendaData : ScriptableObject
{
    [Header("Configuración de la Tienda")]
    public string nombreDeLaTienda;
    
    [Tooltip("Lista de todos los productos disponibles en esta tienda")]
    public List<ItemTienda> listaProductos;
}

[System.Serializable]
public class ItemTienda
{
    public string nombreProducto;
    public int precio;
    public Sprite icono;
    [TextArea] public string descripcion;
}