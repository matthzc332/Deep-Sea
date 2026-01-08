using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("UI References")]
    public Transform containerTienda;
    public GameObject cartaPrefab;
    public GameObject inspeccionPrefab;
    public Transform inspeccionContainer;
    public TextMeshProUGUI textoMonedas;

    [Header("Configuración Tienda")]
    [SerializeField] private int numeroObjetosEnTienda = 6;
    [SerializeField] private bool usarTiendaAleatoria = true;

    [Header("Posiciones de Barco")]
    public GameObject fondoModal; // Referencia al GameObject del fondo modal (ya creado en escena)
    public GameObject PosicionBarco;
    public List<PosicionMarinero> posicionesExistentes = new List<PosicionMarinero>();

    // Variable para guardar el marinero seleccionado temporalmente
    public PlantillaObjeto marineroSeleccionadoTemporal;
    public GameObject objetoTiendaOrigen; // Referencia al objeto de tienda que se compró

    [Header("Datos")]
    public List<PlantillaObjeto> todosLosObjetos = new List<PlantillaObjeto>();
    private List<PlantillaObjeto> objetosDisponibles = new List<PlantillaObjeto>();

    [Header("Economía")]
    public int monedaJugador = 500;

    private PlantillaObjeto seleccionActual;
    private GameObject inspeccionActual;

    // void Awake()
    // {
    //     Debug.Log("Instancia de ShopManager viva: " + gameObject.name);

    //     if (Instance == null) Instance = this;
    //     else
    //     {
    //         Debug.LogError("ShopManager duplicado destruido: " + gameObject.name);
    //         Destroy(gameObject);
    //     }
    // }

    void Start()
    {
        InicializarTienda();
        ActualizarUI();
        
        if (objetosDisponibles.Count > 0)
        {
            SeleccionarMarinero(objetosDisponibles[0]);
        }
    }

    public void InicializarTienda()
    {
        objetosDisponibles.Clear();
        
        if (usarTiendaAleatoria)
        {
            objetosDisponibles = GenerarTiendaAleatoria(numeroObjetosEnTienda);
        }
        else
        {
            objetosDisponibles = new List<PlantillaObjeto>(todosLosObjetos);
            numeroObjetosEnTienda = objetosDisponibles.Count;
        }
        
        LlenarTienda();
    }

    List<PlantillaObjeto> GenerarTiendaAleatoria(int cantidad)
    {
        if (todosLosObjetos.Count == 0)
        {
            Debug.LogError("No hay objetos en la lista 'todosLosObjetos'");
            return new List<PlantillaObjeto>();
        }
        
        List<PlantillaObjeto> listaMezclada = new List<PlantillaObjeto>(todosLosObjetos);
        listaMezclada = MezclarLista(listaMezclada);
        
        int cantidadFinal = Mathf.Min(cantidad, listaMezclada.Count);
        return listaMezclada.Take(cantidadFinal).ToList();
    }

    List<PlantillaObjeto> MezclarLista(List<PlantillaObjeto> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            PlantillaObjeto temp = lista[i];
            lista[i] = lista[randomIndex];
            lista[randomIndex] = temp;
        }
        return lista;
    }

    void LlenarTienda()
    {
        foreach (Transform child in containerTienda)
            Destroy(child.gameObject);

        foreach (var objeto in objetosDisponibles)
        {
            GameObject carta = Instantiate(cartaPrefab, containerTienda);
            Objeto objetoScript = carta.GetComponent<Objeto>();
            
            if (objetoScript != null)
            {
                objetoScript.ConfigurarObjeto(objeto);
            }
            else
            {
                Debug.LogError("Prefab carta no tiene componente Objeto");
            }
        }
    }

    public void SeleccionarMarinero(PlantillaObjeto objeto)
    {
        Debug.Log("Entrando a Seleccionar marinero");
        seleccionActual = objeto;
        CrearVistaInspeccion(objeto);
        Debug.Log($"Objeto seleccionado: {objeto.nombre}");
    }

    void CrearVistaInspeccion(PlantillaObjeto objeto)
    {
        Debug.Log("PREFAB " + inspeccionPrefab);
        Debug.Log("CONTENDOR " + inspeccionContainer);
        
        if (inspeccionActual != null)
        {
            Destroy(inspeccionActual);
        }

        if (inspeccionPrefab == null || inspeccionContainer == null)
        {
            Debug.LogError("Faltan referencias en ShopManager");
            return;
        }

        inspeccionActual = Instantiate(inspeccionPrefab, inspeccionContainer);
        
        RectTransform rt = inspeccionActual.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = new Vector2(-950f, 270f);
            rt.localPosition = new Vector3(-950f, 270f, 0f);
            rt.localScale = Vector3.one;
            Debug.Log($"Inspección posicionada en: {rt.anchoredPosition}");
        }
        else
        {
            inspeccionActual.transform.localPosition = new Vector3(-950f, 270f, 0f);
            inspeccionActual.transform.localScale = Vector3.one;
        }

        Objeto objetoScript = inspeccionActual.GetComponent<Objeto>();
        if (objetoScript != null)
        {
            objetoScript.ConfigurarObjeto(objeto);
            objetoScript.ConfigurarBotonCompra(ComprarObjetoActual);
        }
        else
        {
            Debug.LogError("El prefab no tiene componente Objeto");
        }
    }

    public void ComprarObjetoActual()
    {
        if (seleccionActual != null)
        {
            IntentarComprar(seleccionActual);
        }
    }

    public void IntentarComprar(PlantillaObjeto objeto)
    {
        if (monedaJugador >= objeto.precio)
        {
            // Guardar el marinero seleccionado temporalmente
            marineroSeleccionadoTemporal = objeto;
            
            // Reducir monedas INMEDIATAMENTE
            monedaJugador -= objeto.precio;
            ActualizarUI();
            
            // Buscar y guardar referencia al objeto de tienda
            objetoTiendaOrigen = EncontrarObjetoTiendaOrigen();
            
            // Activar la selección de posición
            ActivarSeleccionPosicion();
            
            Debug.Log($"¡{objeto.nombre} comprado! Selecciona una posición en el barco.");
        }
        else
        {
            Debug.Log("¡Fondos insuficientes!");
        }
    }

    private void ProcesarCompraCompleta()
    {
        Debug.Log($"Compra completada: {marineroSeleccionadoTemporal.nombre} por {marineroSeleccionadoTemporal.precio} monedas");
    }

    public void RemoverObjetoDeTienda(PlantillaObjeto objeto)
    {
        if (objetosDisponibles.Contains(objeto))
        {
            objetosDisponibles.Remove(objeto);
            
            // Destruir el GameObject de la tienda si existe
            if (objetoTiendaOrigen != null)
            {
                Destroy(objetoTiendaOrigen);
                objetoTiendaOrigen = null;
            }
            
            LlenarTienda();
            
            if (seleccionActual == objeto && objetosDisponibles.Count > 0)
            {
                SeleccionarMarinero(objetosDisponibles[0]);
            }
            else if (objetosDisponibles.Count == 0)
            {
                if (inspeccionActual != null)
                {
                    Destroy(inspeccionActual);
                    inspeccionActual = null;
                }
                seleccionActual = null;
            }
        }
    }

    void ActualizarUI()
    {
        if (textoMonedas != null)
        {
            textoMonedas.text = $"Monedas: {monedaJugador}";
        }
    }

    public void AgregarMonedas(int cantidad)
    {
        monedaJugador += cantidad;
        ActualizarUI();
    }

    public void ForzarRefrescoTienda()
    {
        InicializarTienda();
    }

    public void CambiarModoTienda(bool aleatorio)
    {
        usarTiendaAleatoria = aleatorio;
        InicializarTienda();
    }

    public void CambiarNumeroObjetos(int nuevoNumero)
    {
        numeroObjetosEnTienda = nuevoNumero;
        InicializarTienda();
    }

    // Nueva función para activar la selección de posición
    public void ActivarSeleccionPosicion()
    {
        // Activar fondo modal y PosicionBarco

            fondoModal.SetActive(true);
            PosicionBarco.SetActive(true);
            
        
        // Activar y resaltar solo las posiciones DISPONIBLES
        bool hayPosicionesDisponibles = false;
        
        foreach (var posicion in posicionesExistentes)
        {
            if (posicion != null)
            {
                posicion.gameObject.SetActive(true);
                
                if (posicion.EstaDisponible())
                {
                    posicion.ActivarParpadeo(true);
                    posicion.SetColorParpadeo(Color.green);
                    posicion.SetVelocidadParpadeo(3f);
                    
                    Button botonPosicion = posicion.GetComponent<Button>();
                    if (botonPosicion != null)
                    {
                        botonPosicion.interactable = true;
                    }
                    
                    hayPosicionesDisponibles = true;
                }
                else
                {
                    Button botonPosicion = posicion.GetComponent<Button>();
                    if (botonPosicion != null)
                    {
                        botonPosicion.interactable = false;
                    }
                }
            }
        }
        
        if (!hayPosicionesDisponibles)
        {
            Debug.LogWarning("No hay posiciones disponibles en el barco.");
            CancelarSeleccionPosicion();
            return;
        }
        
    
    }

    // Nueva función para cerrar modo selección
    private void CerrarModoSeleccionPosicion()
    {
        // Desactivar fondo modal
            fondoModal.SetActive(false);
            PosicionBarco.SetActive(false);
        
        // Restaurar estado normal de las posiciones
        foreach (var posicion in posicionesExistentes)
        {
            if (posicion != null)
            {
                posicion.ActivarParpadeo(false);
                posicion.SetColorParpadeo(Color.white);
                posicion.SetVelocidadParpadeo(2f);
                
                Button botonPosicion = posicion.GetComponent<Button>();
                if (botonPosicion != null)
                {
                    botonPosicion.interactable = true;
                }
                
                if (!posicion.EstaDisponible())
                {
                    posicion.gameObject.SetActive(true);
                }
            }
        }
        
        // Reactivar la UI de la tienda
        if (containerTienda != null)
            containerTienda.gameObject.SetActive(true);
        
        // Reactivar la inspección si existe
        if (inspeccionActual != null)
            inspeccionActual.SetActive(true);
    }

    // Nueva función para asignar el marinero a una posición específica
    public void AsignarMarineroAPosicion(PosicionMarinero posicionSeleccionada)
    {
        if (marineroSeleccionadoTemporal == null)
        {
            Debug.LogWarning("No hay marinero seleccionado para asignar");
            return;
        }
        
        if (!posicionesExistentes.Contains(posicionSeleccionada))
        {
            Debug.LogWarning("La posición seleccionada no está en la lista de posiciones del barco");
            return;
        }
        
        if (!posicionSeleccionada.EstaDisponible())
        {
            Debug.LogWarning("La posición seleccionada ya está ocupada");
            return;
        }
        
        bool asignado = posicionSeleccionada.AsignarMarinero(marineroSeleccionadoTemporal);
        
        if (asignado)
        {
            Debug.Log($"¡{marineroSeleccionadoTemporal.nombre} asignado al barco en posición {posicionSeleccionada.gameObject.name}!");
            
            ProcesarCompraCompleta();
            marineroSeleccionadoTemporal = null;
            CerrarModoSeleccionPosicion();
            
            if (inspeccionActual != null)
            {
                Destroy(inspeccionActual);
                inspeccionActual = null;
            }
            
            RemoverObjetoDeTienda(seleccionActual);
            
            if (objetosDisponibles.Count > 0)
            {
                SeleccionarMarinero(objetosDisponibles[0]);
            }
            
            ActualizarUI();
        }
        else
        {
            Debug.LogWarning("No se pudo asignar el marinero a esta posición");
        }
    }

    // Nueva función para cancelar la selección de posición
    public void CancelarSeleccionPosicion()
    {
        if (marineroSeleccionadoTemporal != null)
        {
            // Reembolsar al jugador
            monedaJugador += marineroSeleccionadoTemporal.precio;
            Debug.Log($"Selección cancelada. {marineroSeleccionadoTemporal.precio} monedas reembolsadas.");
            marineroSeleccionadoTemporal = null;
        }
        
        CerrarModoSeleccionPosicion();
        ActualizarUI();
        Debug.Log("Modo selección de posición CANCELADO.");
    }

    private GameObject EncontrarObjetoTiendaOrigen()
    {
        foreach (Transform child in containerTienda)
        {
            Objeto objetoScript = child.GetComponent<Objeto>();
            if (objetoScript != null && objetoScript.GetDatosMarinero() == seleccionActual)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public int ContarPosicionesDisponibles()
    {
        int disponibles = 0;
        foreach (var posicion in posicionesExistentes)
        {
            if (posicion != null && posicion.EstaDisponible())
            {
                disponibles++;
            }
        }
        return disponibles;
    }

    public void MostrarEstadoBarco()
    {
        Debug.Log("=== ESTADO DEL BARCO ===");
        for (int i = 0; i < posicionesExistentes.Count; i++)
        {
            if (posicionesExistentes[i] != null)
            {
                string estado = posicionesExistentes[i].EstaDisponible() ? "Disponible" : $"Ocupado: {posicionesExistentes[i].Getnombre()}";
                Debug.Log($"Posición {i} ({posicionesExistentes[i].gameObject.name}): {estado}");
            }
        }
        Debug.Log($"Total disponibles: {ContarPosicionesDisponibles()}/{posicionesExistentes.Count}");
    }
}