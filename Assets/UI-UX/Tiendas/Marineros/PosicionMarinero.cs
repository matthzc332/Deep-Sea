using UnityEngine;
using UnityEngine.UI;

public class PosicionMarinero : MonoBehaviour
{
    public enum TipoPosicion { Posicion1, Posicion2 }

    [Header("Referencias de Managers")]
    public ShipPlacementManager shipPlacementManager; 

    [Header("Conexión con Datos del Barco")]
    // Arrastra aquí tu archivo ShipData desde la carpeta Assets
    public ShipData datosDelBarco; 
    public TipoPosicion quePosicionRepresenta;

    [Header("Configuración")]
    public PlantillaObjeto marineroAsignado;
    
    [Header("Silueta (para posición vacía)")]
    public Sprite spriteSilueta; 
    
    private Image imagenPosicion;
    private Sprite spriteOriginal;
    private int frameActual = 0;
    private float timerAnimacion = 0f;
    private bool estadoInicializado = false;
    
void Awake()
{
    // 1. Referencias de componentes
    imagenPosicion = GetComponent<Image>();
    
    if (imagenPosicion != null) 
        imagenPosicion.color = Color.white;

    ConfigurarBotonPosicion();

    // 2. LIMPIEZA INMEDIATA
    // Forzamos el estado null antes de que pase el primer frame
    marineroAsignado = null;
    LimpiarReferenciaEnScriptableObject();
    
    Debug.Log($"<color=cyan>[INICIO]</color> Posición {quePosicionRepresenta}: Marinero asignado es <b>NULL</b>.");
}

void Start()
{
    // Guardamos el sprite original si no se hizo en Awake
    if (imagenPosicion != null && imagenPosicion.sprite != null)
        spriteOriginal = imagenPosicion.sprite;

    // Actualizamos lo visual para que muestre la silueta desde el segundo 0
    ActualizarEstadoPosicion();
    estadoInicializado = true;
}

    private void LimpiarReferenciaEnScriptableObject()
    {
        if (datosDelBarco == null) return;

        if (quePosicionRepresenta == TipoPosicion.Posicion1)
        {
            datosDelBarco.posicion1 = null;
            Debug.Log("DEBUG: POSICION 1 LIMPIADA");
        }
        else if (quePosicionRepresenta == TipoPosicion.Posicion2)
        {
            datosDelBarco.posicion2 = null;
            Debug.Log("DEBUG: POSICION 2 LIMPIADA");
        }
    }

    private void ActualizarPrefabEnScriptableObject(GameObject nuevoPrefab)
    {
        if (datosDelBarco == null) return;

        if (quePosicionRepresenta == TipoPosicion.Posicion1)
            datosDelBarco.posicion1 = nuevoPrefab;
        else if (quePosicionRepresenta == TipoPosicion.Posicion2)
            datosDelBarco.posicion2 = nuevoPrefab;
    }
    
    public bool AsignarMarinero(PlantillaObjeto nuevoMarinero)
    {
        // 1. Si ya hay alguien, rebotamos la acción
        if (marineroAsignado != null) return false;
        
        // 2. Asignamos la referencia local
        marineroAsignado = nuevoMarinero;

        // 3. Sincronizamos con el ScriptableObject (ShipData)
        if (nuevoMarinero != null && datosDelBarco != null)
        {
            // Pasamos el prefab que contiene la plantilla al ScriptableObject
            ActualizarPrefabEnScriptableObject(nuevoMarinero.prefabDelObjeto);
            Debug.Log($"<color=green>Sincronización:</color> Prefab {nuevoMarinero.prefabDelObjeto.name} guardado en {quePosicionRepresenta} de ShipData.");
        }

        // 4. Actualizamos el aspecto visual
        if (imagenPosicion != null) imagenPosicion.material = null;
        ActualizarEstadoPosicion();
        
        return true;
    }
    
    public void LiberarPosicion()
    {
        marineroAsignado = null;
        // También limpiamos el ScriptableObject al liberar la posición
        LimpiarReferenciaEnScriptableObject();
        ActualizarEstadoPosicion();
    }
    
    private void ActualizarEstadoPosicion()
    {
        if (imagenPosicion == null) return;
        
        if (marineroAsignado == null)
        {
            if (spriteSilueta != null)
            {
                imagenPosicion.sprite = spriteSilueta;
                imagenPosicion.color = Color.white;
            }
            else
            {
                imagenPosicion.sprite = spriteOriginal;
                imagenPosicion.color = Color.white;
            }
            
            frameActual = 0;
            timerAnimacion = 0f;
        }
        else
        {
            imagenPosicion.color = Color.white; 
            
            if (marineroAsignado.idleAnimationSprites != null && marineroAsignado.idleAnimationSprites.Length > 0)
            {
                imagenPosicion.sprite = marineroAsignado.idleAnimationSprites[0];
                frameActual = 0;
                timerAnimacion = 0f;
            }
        }
    }
    
    void Update()
    {
        if (!estadoInicializado || marineroAsignado == null) return;
        
        if (marineroAsignado.idleAnimationSprites.Length > 1)
        {
            timerAnimacion += Time.deltaTime;
            float tiempoPorFrame = 1f / marineroAsignado.animationSpeed;
            
            if (timerAnimacion >= tiempoPorFrame)
            {
                timerAnimacion = 0f;
                frameActual = (frameActual + 1) % marineroAsignado.idleAnimationSprites.Length;
                imagenPosicion.sprite = marineroAsignado.idleAnimationSprites[frameActual];
            }
        }
    }

    public void OnClickPosicion()
    {
        if (!EstaDisponible()) return;

        if (shipPlacementManager == null)
            shipPlacementManager = Object.FindFirstObjectByType<ShipPlacementManager>();

        if (shipPlacementManager != null)
            shipPlacementManager.IntentarColocarEnSlot(this);
    }

    public void ConfigurarBotonPosicion()
    {
        Button boton = GetComponent<Button>();
        if (boton == null) boton = gameObject.AddComponent<Button>();
        
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(OnClickPosicion);
        
        ColorBlock colors = boton.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        boton.colors = colors;
    }

    public bool EstaDisponible() => marineroAsignado == null;
    
    
    public void ActivarParpadeo(bool activar) 
    {
        if (imagenPosicion != null) imagenPosicion.color = Color.white;
    }
}