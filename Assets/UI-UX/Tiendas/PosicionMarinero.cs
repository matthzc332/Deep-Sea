using UnityEngine;
using UnityEngine.UI;

public class PosicionMarinero : MonoBehaviour
{
    [Header("Configuración")]
    public PlantillaObjeto marineroAsignado; // Referencia al ScriptableObject del marinero
    
    [Header("Parpadeo")]
    public Color colorParpadeo = Color.white;
    public float velocidadParpadeo = 2f;
    
    // Referencia al componente Image en el mismo GameObject
    private Image imagenPosicion;
    
    private bool parpadeando = false;
    private float timerParpadeo = 0f;
    private Sprite spriteOriginal;
    private int frameActual = 0;
    private float timerAnimacion = 0f;
    private bool estadoInicializado = false;
    
    // Awake se ejecuta antes que Start
    void Awake()
    {
        // Obtener el componente Image del mismo GameObject
        imagenPosicion = GetComponent<Image>();
        
        if (imagenPosicion == null)
        {
            Debug.LogError($"No se encontró el componente Image en {gameObject.name}. Asegurate de agregarlo.");
        }
    }
    
    // Start se ejecuta en el primer frame
    void Start()
    {
        // Guardar el sprite original si existe
        if (imagenPosicion != null && imagenPosicion.sprite != null)
        {
            spriteOriginal = imagenPosicion.sprite;
        }
        
        // Inicializar según si hay marinero asignado o no
        ActualizarEstadoPosicion();
        estadoInicializado = true;
    }
    
    // Método para asignar un marinero a esta posición
    public bool AsignarMarinero(PlantillaObjeto nuevoMarinero)
    {
        if (marineroAsignado != null)
        {
            Debug.LogWarning($"La posición {gameObject.name} ya tiene un marinero asignado");
            return false;
        }
        
        marineroAsignado = nuevoMarinero;
        ActualizarEstadoPosicion();
        return true;
    }
    
    // Método para liberar la posición
    public void LiberarPosicion()
    {
        marineroAsignado = null;
        ActualizarEstadoPosicion();
    }
    
    // Actualiza el estado visual de la posición
    private void ActualizarEstadoPosicion()
    {
        if (imagenPosicion == null) return;
        
        if (marineroAsignado == null)
        {
            // Posición vacía - establecer sprite de UI Square y activar parpadeo
            Sprite uiSquare = Resources.Load<Sprite>("UI Square");
            if (uiSquare != null)
            {
                imagenPosicion.sprite = uiSquare;
            }
            else
            {
                // Si no hay UI Square, usar el sprite original o dejar null
                imagenPosicion.sprite = spriteOriginal;
                Debug.LogWarning("No se encontró el sprite 'UI Square' en Resources.");
            }
            
            // IMPORTANTE: NO establecer color aquí, se hará en Update
            parpadeando = true;
            timerParpadeo = 0f; // Resetear timer
        }
        else
        {
            // Posición ocupada - desactivar parpadeo y mostrar primer sprite del marinero
            parpadeando = false;
            
            // Restaurar color blanco sólido
            imagenPosicion.color = Color.white;
            
            if (marineroAsignado.idleAnimationSprites != null && 
                marineroAsignado.idleAnimationSprites.Length > 0)
            {
                imagenPosicion.sprite = marineroAsignado.idleAnimationSprites[0];
                frameActual = 0;
                timerAnimacion = 0f;
            }
        }
    }
    
    // Update se ejecuta cada frame
    void Update()
    {
        if (!estadoInicializado) return;
        
        if (marineroAsignado == null)
        {
            // Lógica de parpadeo para posición vacía
            if (parpadeando && imagenPosicion != null)
            {
                timerParpadeo += Time.deltaTime * velocidadParpadeo;
                
                // Usar Mathf.PingPong para un efecto de parpadeo más controlado
                float alpha = Mathf.PingPong(timerParpadeo, 1f);
                
                Color nuevoColor = colorParpadeo;
                nuevoColor.a = alpha;
                imagenPosicion.color = nuevoColor;
                
                // Debug para verificar que está parpadeando
                if (Time.frameCount % 60 == 0) // Cada segundo aproximadamente
                {
                    // Debug.Log($"Parpadeando - Alpha: {alpha}, Timer: {timerParpadeo}", gameObject);
                }
            }
        }
        else
        {
            // Lógica de animación idle del marinero
            if (marineroAsignado.idleAnimationSprites != null && 
                marineroAsignado.idleAnimationSprites.Length > 1)
            {
                timerAnimacion += Time.deltaTime;
                
                // Cambiar frame según la velocidad de animación
                float tiempoPorFrame = 1f / marineroAsignado.animationSpeed;
                
                if (timerAnimacion >= tiempoPorFrame)
                {
                    timerAnimacion = 0f;
                    frameActual = (frameActual + 1) % marineroAsignado.idleAnimationSprites.Length;
                    imagenPosicion.sprite = marineroAsignado.idleAnimationSprites[frameActual];
                }
            }
        }
    }
    
    // Método alternativo para parpadeo más suave
    private void ParpadeoAlternativo()
    {
        if (parpadeando && imagenPosicion != null)
        {
            timerParpadeo += Time.deltaTime * velocidadParpadeo;
            
            // Usar seno para un efecto más suave
            float alpha = (Mathf.Sin(timerParpadeo * Mathf.PI * 2) + 1f) * 0.5f;
            
            // Asegurar que alpha esté entre 0.3 y 1 para que no desaparezca completamente
            alpha = Mathf.Lerp(0.3f, 1f, alpha);
            
            imagenPosicion.color = new Color(colorParpadeo.r, colorParpadeo.g, colorParpadeo.b, alpha);
        }
    }
    
    // Método para verificar si el componente está configurado correctamente
    public void VerificarConfiguracion()
    {
        Debug.Log($"=== Verificación {gameObject.name} ===");
        Debug.Log($"ImagenPosicion: {imagenPosicion}");
        Debug.Log($"MarineroAsignado: {marineroAsignado}");
        Debug.Log($"Parpadeando: {parpadeando}");
        Debug.Log($"VelocidadParpadeo: {velocidadParpadeo}");
        Debug.Log($"Estado Inicializado: {estadoInicializado}");
        
        if (imagenPosicion != null)
        {
            Debug.Log($"Color actual: {imagenPosicion.color}");
            Debug.Log($"Sprite actual: {imagenPosicion.sprite}");
        }
    }
    
    // Métodos públicos para obtener información
    public bool EstaDisponible()
    {
        return marineroAsignado == null;
    }
    
    public string Getnombre()
    {
        return marineroAsignado != null ? marineroAsignado.nombre : "Vacío";
    }
    
    public string GetHabilidades()
    {
        if (marineroAsignado == null) return "Sin marinero";
        
        return $"Fuerte con: {marineroAsignado.strongWith}\n" +
               $"Débil con: {marineroAsignado.weakWith}";
    }
    
    public int GetPrecioMarinero()
    {
        return marineroAsignado != null ? marineroAsignado.precio : 0;
    }
    
    public string GetDescripcion()
    {
        return marineroAsignado != null ? marineroAsignado.descripcion : "Posición disponible";
    }
    
    // Método para forzar la actualización visual
    public void ForzarActualizacionVisual()
    {
        ActualizarEstadoPosicion();
    }
    
    // Método para cambiar el color de parpadeo
    public void SetColorParpadeo(Color nuevoColor)
    {
        colorParpadeo = nuevoColor;
    }
    
    // Método para cambiar la velocidad de parpadeo
    public void SetVelocidadParpadeo(float nuevaVelocidad)
    {
        velocidadParpadeo = nuevaVelocidad;
    }
    
    public void OnClickPosicion()
{
    Debug.Log("CLICKEANDO POSICION");
    
    // Verificar si está disponible
    if (!EstaDisponible())
    {
        Debug.Log($"Posición ocupada: {Getnombre()}");
        return;
    }
    
    // Cambiar material a "None" y color a blanco
    if (imagenPosicion != null)
    {
        imagenPosicion.material = null; // Material = None
        imagenPosicion.color = new Color(1f, 1f, 1f, 1f); // Color blanco
        
        Debug.Log($"Imagen actualizada: Material = None, Color = Blanco");
    }
    else
    {
        Debug.LogWarning("imagenPosicion es null, no se puede actualizar");
    }
    
    // Notificar al ShopManager que esta posición fue seleccionada
    if (ShopManager.Instance != null)
    {
        Debug.Log($"Posición seleccionada: {gameObject.name}");
        ShopManager.Instance.AsignarMarineroAPosicion(this);
    }
    else
    {
        Debug.LogWarning("No se encontró ShopManager.Instance");
    }
}
    
    // Método para activar/desactivar parpadeo manualmente
    public void ActivarParpadeo(bool activar)
{
    parpadeando = activar;
    if (!activar && imagenPosicion != null)
    {
        imagenPosicion.color = Color.white;
    }
}

public void ConfigurarBotonPosicion()
{
    Button boton = GetComponent<Button>();
    if (boton == null)
    {
        boton = gameObject.AddComponent<Button>();
    }
    
    boton.onClick.RemoveAllListeners();
    boton.onClick.AddListener(OnClickPosicion);
    
    // Configurar transparencia
    ColorBlock colors = boton.colors;
    colors.normalColor = new Color(1, 1, 1, 0); // Transparente
    colors.highlightedColor = new Color(1, 1, 1, 0.3f);
    colors.pressedColor = new Color(1, 1, 1, 0.5f);
    colors.selectedColor = new Color(1, 1, 1, 0.3f);
    boton.colors = colors;
}
}