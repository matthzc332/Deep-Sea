using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

// Heredamos de ShopManager para que 'this' sea un manager válido para Objeto.cs
public class TiendaModular : ShopManager
{
    [Header("Referencias UI Modular")]
    public GameObject cartaPrefab;
    public GameObject inspeccionPrefab;
    public Transform inspeccionContainer;

    [Header("Configuración de Inventario")]
    public List<PlantillaObjeto> todosLosObjetosEnEstaTienda = new List<PlantillaObjeto>();
    public int cantidadObjetosEnTienda = 5;
    
    [Header("Posicionamiento Inspección")]
    public Vector2 posicionInspeccion = new Vector2(-950f, 270f);
    public Vector3 escalaInspeccion = Vector3.one;

    [Header("Estado Interno")]
    private List<PlantillaObjeto> objetosDisponibles = new List<PlantillaObjeto>();
    private GameObject inspeccionActual;
    private PlantillaObjeto seleccionActual;
    private List<GameObject> cartasInstanciadas = new List<GameObject>();

    // Evento para sistemas externos (Economía, Logros, etc.)
    public delegate void ObjetoCompradoHandler(PlantillaObjeto objeto);
    public event ObjetoCompradoHandler OnObjetoComprado;

    void Start()
    {
        InicializarTiendaModular();
    }

    public void InicializarTiendaModular()
    {
        objetosDisponibles = GenerarTiendaAleatoria(cantidadObjetosEnTienda);
        LlenarTiendaVisual();
        LimpiarInspeccion();
    }

    List<PlantillaObjeto> GenerarTiendaAleatoria(int cantidad)
    {
        if (todosLosObjetosEnEstaTienda == null || todosLosObjetosEnEstaTienda.Count == 0)
        {
            Debug.LogError($"No hay objetos asignados en la tienda: {gameObject.name}");
            return new List<PlantillaObjeto>();
        }
        
        List<PlantillaObjeto> resultado = new List<PlantillaObjeto>();
        List<PlantillaObjeto> copiaLista = new List<PlantillaObjeto>(todosLosObjetosEnEstaTienda);
        
        int limite = Mathf.Min(cantidad, copiaLista.Count);
        for (int i = 0; i < limite; i++)
        {
            int randomIndex = Random.Range(0, copiaLista.Count);
            resultado.Add(copiaLista[randomIndex]);
            copiaLista.RemoveAt(randomIndex); // Evitar duplicados en la misma tanda
        }
        
        return resultado;
    }

    void LlenarTiendaVisual()
    {
        LimpiarTiendaVisual();
        cartasInstanciadas.Clear();

        if (cartaPrefab == null) return;

        foreach (var objeto in objetosDisponibles)
        {
            GameObject carta = Instantiate(cartaPrefab, transform);
            Objeto objetoScript = carta.GetComponent<Objeto>();
            
            if (objetoScript != null)
            {
                // CORRECCIÓN: Ahora pasamos 'this' como el manager
                objetoScript.ConfigurarObjeto(objeto, this);
                
                // Sobrescribimos el click para que use la lógica de esta tienda modular
                if (objetoScript.TryGetComponent<Button>(out var button))
                {
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => SeleccionarObjetoModular(objeto));
                }
                
                cartasInstanciadas.Add(carta);
            }
        }
    }

    public void SeleccionarObjetoModular(PlantillaObjeto objeto)
    {
        seleccionActual = objeto;
        CrearVistaInspeccionModular(objeto);
    }

    void CrearVistaInspeccionModular(PlantillaObjeto objeto)
    {
        LimpiarInspeccion();

        if (inspeccionPrefab == null || inspeccionContainer == null) return;

        inspeccionActual = Instantiate(inspeccionPrefab, inspeccionContainer);
        
        RectTransform rt = inspeccionActual.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = posicionInspeccion;
            rt.localScale = escalaInspeccion;
        }

        Objeto objetoScript = inspeccionActual.GetComponent<Objeto>();
        if (objetoScript != null)
        {
            // CORRECCIÓN: Pasamos 'this' como manager
            objetoScript.ConfigurarObjeto(objeto, this);
            objetoScript.ConfigurarBotonCompra(() => ComprarObjetoModular());
        }
    }

    void ComprarObjetoModular()
    {
        if (seleccionActual == null) return;

        // Aquí podrías añadir la lógica de dinero:
        // if (GlobalEconomy.Money >= seleccionActual.precio) { ... }

        if (objetosDisponibles.Contains(seleccionActual))
        {
            PlantillaObjeto comprado = seleccionActual;
            objetosDisponibles.Remove(comprado);
            
            Debug.Log($"Compra exitosa: {comprado.nombre}");

            // Destruir la carta física en la tienda
            GameObject cartaADestruir = cartasInstanciadas.Find(c => 
                c.GetComponent<Objeto>().GetDatosMarinero() == comprado);
            
            if (cartaADestruir != null)
            {
                cartasInstanciadas.Remove(cartaADestruir);
                Destroy(cartaADestruir);
            }

            LimpiarInspeccion();
            OnObjetoComprado?.Invoke(comprado);
        }
    }

    void LimpiarTiendaVisual()
    {
        foreach (Transform child in transform) Destroy(child.gameObject);
    }

    void LimpiarInspeccion()
    {
        if (inspeccionActual != null) Destroy(inspeccionActual);
        inspeccionActual = null;
        seleccionActual = null;
    }
}