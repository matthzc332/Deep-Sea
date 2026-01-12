using UnityEngine;
using UnityEngine.UI;

public class PosicionMarinero : MonoBehaviour
{
    [Header("Referencias de Managers")]
    // Referencia al manager que controla el barco y el posicionamiento
    public ShipPlacementManager shipPlacementManager; 

    [Header("Configuración")]
    public PlantillaObjeto marineroAsignado; 
    
    [Header("Parpadeo")]
    public Color colorParpadeo = Color.white;
    public float velocidadParpadeo = 2f;
    
    private Image imagenPosicion;
    private bool parpadeando = false;
    private float timerParpadeo = 0f;
    private Sprite spriteOriginal;
    private int frameActual = 0;
    private float timerAnimacion = 0f;
    private bool estadoInicializado = false;
    
    void Awake()
    {
        imagenPosicion = GetComponent<Image>();
        
        if (imagenPosicion == null)
            Debug.LogError($"No se encontró Image en {gameObject.name}.");

        ConfigurarBotonPosicion();
    }
    
    void Start()
    {
        if (imagenPosicion != null && imagenPosicion.sprite != null)
            spriteOriginal = imagenPosicion.sprite;
        
        ActualizarEstadoPosicion();
        estadoInicializado = true;
    }
    
    public bool AsignarMarinero(PlantillaObjeto nuevoMarinero)
    {
        if (marineroAsignado != null)
        {
            Debug.LogWarning($"La posición {gameObject.name} ya está ocupada");
            return false;
        }
        
        marineroAsignado = nuevoMarinero;
        ActualizarEstadoPosicion();
        return true;
    }
    
    public void LiberarPosicion()
    {
        marineroAsignado = null;
        ActualizarEstadoPosicion();
    }
    
    private void ActualizarEstadoPosicion()
    {
        if (imagenPosicion == null) return;
        
        if (marineroAsignado == null)
        {
            Sprite uiSquare = Resources.Load<Sprite>("UI Square");
            imagenPosicion.sprite = (uiSquare != null) ? uiSquare : spriteOriginal;
            parpadeando = true;
        }
        else
        {
            parpadeando = false;
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
        if (!estadoInicializado) return;
        
        // Lógica de parpadeo (Slot vacío)
        if (marineroAsignado == null && parpadeando && imagenPosicion != null)
        {
            timerParpadeo += Time.deltaTime * velocidadParpadeo;
            float alpha = Mathf.PingPong(timerParpadeo, 1f);
            Color nuevoColor = colorParpadeo;
            nuevoColor.a = alpha;
            imagenPosicion.color = nuevoColor;
        }
        // Lógica de animación (Slot ocupado)
        else if (marineroAsignado != null && marineroAsignado.idleAnimationSprites.Length > 1)
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
        if (!EstaDisponible())
        {
            Debug.Log($"Posición {gameObject.name} ocupada por {marineroAsignado.nombre}");
            return;
        }

        // Buscamos el placement manager si no está asignado
        if (shipPlacementManager == null)
            shipPlacementManager = Object.FindFirstObjectByType<ShipPlacementManager>();

        if (shipPlacementManager != null)
        {
            // Notificamos al Manager del barco que este slot ha sido clickeado
            shipPlacementManager.IntentarColocarEnSlot(this);
        }
        else
        {
            Debug.LogError("No se encontró ShipPlacementManager en la escena.");
        }
    }

    public void ConfigurarBotonPosicion()
    {
        Button boton = GetComponent<Button>();
        if (boton == null) boton = gameObject.AddComponent<Button>();
        
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(OnClickPosicion);
        
        // Configurar los colores del botón para que sea interactivo
        ColorBlock colors = boton.colors;
        colors.normalColor = new Color(1, 1, 1, 0f); // Invisible por defecto
        colors.highlightedColor = new Color(1, 1, 1, 0.2f);
        colors.pressedColor = new Color(1, 1, 1, 0.4f);
        boton.colors = colors;
    }

    public bool EstaDisponible() => marineroAsignado == null;
    
    public void ActivarParpadeo(bool activar) 
    {
        parpadeando = activar;
        if (!activar && imagenPosicion != null) imagenPosicion.color = Color.white;
    }
}