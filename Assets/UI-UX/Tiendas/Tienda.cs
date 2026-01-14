using UnityEngine;
using System.Collections.Generic;

public class TiendaModular : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject cartaPrefab;
    public GameObject inspeccionPrefab;
    public Transform inspeccionContainer;

    [Header("Configuración")]
    public List<PlantillaObjeto> todosLosObjetos = new List<PlantillaObjeto>();
    public int cantidadObjetosEnTienda = 5;
    
    [Header("Posicionamiento Inspección")]
    public Vector2 posicionInspeccion = new Vector2(-950f, 270f);
    public Vector3 escalaInspeccion = Vector3.one;

    [Header("Estado")]
    private List<PlantillaObjeto> objetosDisponibles = new List<PlantillaObjeto>();
    private GameObject inspeccionActual;
    private PlantillaObjeto seleccionActual;
    private List<GameObject> cartasInstanciadas = new List<GameObject>();

    void Start()
    {
        InicializarTienda();
    }

    public void InicializarTienda()
    {
        objetosDisponibles = GenerarTiendaAleatoria(cantidadObjetosEnTienda);
        LlenarTienda();
        LimpiarInspeccion();
    }

    List<PlantillaObjeto> GenerarTiendaAleatoria(int cantidad)
    {
        if (todosLosObjetos == null || todosLosObjetos.Count == 0)
        {
            Debug.LogError("No hay objetos en la lista 'todosLosObjetos'");
            return new List<PlantillaObjeto>();
        }
        
        List<PlantillaObjeto> resultado = new List<PlantillaObjeto>();
        
        for (int i = 0; i < cantidad; i++)
        {
            int randomIndex = Random.Range(0, todosLosObjetos.Count);
            resultado.Add(todosLosObjetos[randomIndex]);
        }
        
        return resultado;
    }

    void LlenarTienda()
    {
        LimpiarTiendaVisual();
        cartasInstanciadas.Clear();

        if (cartaPrefab == null)
        {
            Debug.LogError("No se asignó 'cartaPrefab' en el Inspector");
            return;
        }

        foreach (var objeto in objetosDisponibles)
        {
            GameObject carta = Instantiate(cartaPrefab, transform);
            Objeto objetoScript = carta.GetComponent<Objeto>();
            
            if (objetoScript != null)
            {
                objetoScript.ConfigurarObjeto(objeto);
                
                // Configurar evento de selección
                if (objetoScript.TryGetComponent<UnityEngine.UI.Button>(out var button))
                {
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => SeleccionarObjeto(objeto));
                }
                
                cartasInstanciadas.Add(carta);
            }
            else
            {
                Debug.LogError("Prefab carta no tiene componente Objeto");
                Destroy(carta);
            }
        }
    }

    void LimpiarTiendaVisual()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SeleccionarObjeto(PlantillaObjeto objeto)
    {
        Debug.Log($"Objeto seleccionado en tienda {gameObject.name}: {objeto.nombre}");
        seleccionActual = objeto;
        CrearVistaInspeccion(objeto);
    }

    void CrearVistaInspeccion(PlantillaObjeto objeto)
    {
        LimpiarInspeccion();

        if (inspeccionPrefab == null || inspeccionContainer == null)
        {
            Debug.LogError("Faltan referencias de inspección en la tienda");
            return;
        }

        inspeccionActual = Instantiate(inspeccionPrefab, inspeccionContainer);
        
        // Configurar posición y escala
        RectTransform rt = inspeccionActual.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = posicionInspeccion;
            rt.localScale = escalaInspeccion;
        }
        else
        {
            inspeccionActual.transform.localPosition = new Vector3(posicionInspeccion.x, posicionInspeccion.y, 0f);
            inspeccionActual.transform.localScale = escalaInspeccion;
        }

        // Configurar el objeto de inspección
        Objeto objetoScript = inspeccionActual.GetComponent<Objeto>();
        if (objetoScript != null)
        {
            objetoScript.ConfigurarObjeto(objeto);
            
            // Configurar botón de compra para esta tienda específica
            objetoScript.ConfigurarBotonCompra(() => ComprarObjetoSeleccionado());
        }
        else
        {
            Debug.LogError("El prefab de inspección no tiene componente Objeto");
        }
    }

    void ComprarObjetoSeleccionado()
    {
        if (seleccionActual != null)
        {
            ComprarObjeto(seleccionActual);
        }
    }

    public void ComprarObjeto(PlantillaObjeto objetoComprado)
    {
        if (objetosDisponibles.Contains(objetoComprado))
        {
            objetosDisponibles.Remove(objetoComprado);
            Debug.Log($"Marinero {objetoComprado.nombre} comprado en tienda {gameObject.name}");
            
            // Buscar y destruir la carta correspondiente
            foreach (var carta in cartasInstanciadas.ToArray())
            {
                var objetoScript = carta.GetComponent<Objeto>();
                if (objetoScript != null && objetoScript.GetPlantillaObjeto() == objetoComprado)
                {
                    cartasInstanciadas.Remove(carta);
                    Destroy(carta);
                    break;
                }
            }
            
            LimpiarInspeccion();
            // Notificar compra (opcional para otros sistemas)
            OnObjetoComprado?.Invoke(objetoComprado);
        }
    }

    void LimpiarInspeccion()
    {
        if (inspeccionActual != null)
        {
            Destroy(inspeccionActual);
            inspeccionActual = null;
            seleccionActual = null;
        }
    }

    public void RecargarTienda()
    {
        InicializarTienda();
    }

    public void LimpiarTienda()
    {
        objetosDisponibles.Clear();
        LimpiarTiendaVisual();
        LimpiarInspeccion();
        cartasInstanciadas.Clear();
    }

    public List<PlantillaObjeto> GetObjetosDisponibles()
    {
        return new List<PlantillaObjeto>(objetosDisponibles);
    }

    // Evento para notificar compras (opcional)
    public delegate void ObjetoCompradoHandler(PlantillaObjeto objeto);
    public event ObjetoCompradoHandler OnObjetoComprado;
}