using UnityEngine;
using UnityEngine.UI;

public class PosicionMarinero : MonoBehaviour
{
    [Header("Referencias de Managers")]
    public ShipPlacementManager shipPlacementManager; 

    [Header("Configuración")]
    public PlantillaObjeto marineroAsignado;
    
    [Header("Silueta (para posición vacía)")]
    public Sprite spriteSilueta; // Asigna esto en el inspector
    
    private Image imagenPosicion;
    private Sprite spriteOriginal;
    private int frameActual = 0;
    private float timerAnimacion = 0f;
    private bool estadoInicializado = false;
    
    void Awake()
    {
        imagenPosicion = GetComponent<Image>();
        
        // Forzamos color blanco opaco para que no sea invisible
        if (imagenPosicion != null) 
            imagenPosicion.color = Color.white;

        ConfigurarBotonPosicion();
    }
    
    void Start()
    {
        // Guardamos el sprite original que tenga en el inspector
        if (imagenPosicion != null && imagenPosicion.sprite != null)
            spriteOriginal = imagenPosicion.sprite;
        
        ActualizarEstadoPosicion();
        estadoInicializado = true;
    }
    
    public bool AsignarMarinero(PlantillaObjeto nuevoMarinero)
    {
        if (marineroAsignado != null) return false;
        
        marineroAsignado = nuevoMarinero;
        imagenPosicion.material = null;
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
            // Cuando NO hay marinero asignado: mostrar SILUETA BLANCA
            if (spriteSilueta != null)
            {
                // Usar la silueta específica asignada en el inspector
                imagenPosicion.sprite = spriteSilueta;
                imagenPosicion.color = Color.white;
            }
            else
            {
                // Si no hay silueta, mostrar el sprite original (si existe)
                imagenPosicion.sprite = spriteOriginal;
                imagenPosicion.color = Color.white;
            }
            
            // Resetear cualquier animación
            frameActual = 0;
            timerAnimacion = 0f;
        }
        else
        {
            // Cuando SÍ hay marinero asignado: mostrar el marinero REAL con animación
            imagenPosicion.color = Color.white; // Color normal para sprites con color
            
            // ASIGNAR TEXTURA DEL MARINERO
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
        
        // Animación si hay más de un frame
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
        
        // Colores del botón sólidos para que se vea el cuadro
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