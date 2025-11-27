using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tienda : MonoBehaviour
{
    [SerializeField] GameObject prefabObjetoTienda;
    [SerializeField] int numeroMaximoObjetosTienda;
    [SerializeField] PlantillaObjeto[] listaTienda;
    [SerializeField] List<PlantillaObjeto> listaProvicionalTienda;

    private Objeto objeto;


    private void Start()
    {
        listaProvicionalTienda.AddRange(listaTienda);

        for (int i = 0; i <= numeroMaximoObjetosTienda -1; i++) 
            {
                GameObject tienda = GameObject.Instantiate(prefabObjetoTienda, Vector2.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("Tienda").transform);
                int indice = Random.Range(0, listaProvicionalTienda.Count);
                objeto = tienda.GetComponent<Objeto>();
                objeto.CrearObjeto (listaProvicionalTienda[indice]);
            listaProvicionalTienda.Remove(listaProvicionalTienda[indice]);
        }
    }
}
