using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Objeto : MonoBehaviour
{
    // Configuración del tipo de objeto
    public enum TipoVista { Carta, Inspeccion }
    [SerializeField] private TipoVista tipoVista = TipoVista.Carta;
    
    // Referencias UI
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI textoObjeto;
    [SerializeField] private TextMeshProUGUI precioObjeto;
    [SerializeField] private TextMeshProUGUI strongText;
    [SerializeField] private TextMeshProUGUI weakText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image marineroImage;
    
    // Solo para cartas
    [Header("Configuración Carta")]
    [SerializeField] private Button botonSeleccionar;
    
    // Animación
    [Header("Animación")]
    [SerializeField] private List<Sprite> currentAnimationSprites;
    [SerializeField] private float animationSpeed = 1f;
    private int currentSpriteIndex = 0;
    private float animationTimer = 0f;
    
    // Datos
    private PlantillaObjeto datosMarinero;
    
    void Awake()
    {
        // SOLO configurar botón si es una carta Y si el botón está asignado
        if (tipoVista == TipoVista.Carta)
        {
            // Si no está asignado en el inspector, intentar encontrarlo
            if (botonSeleccionar == null)
            {
                botonSeleccionar = GetComponent<Button>();
                
                // Si aún es null, buscar en hijos
                if (botonSeleccionar == null)
                {
                    botonSeleccionar = GetComponentInChildren<Button>();
                }
                
                // Si aún no lo encuentra, mostrar advertencia
                if (botonSeleccionar == null)
                {
                    Debug.LogWarning("Objeto (Carta): No se encontró Button. La carta no será clickeable.");
                }
            }
            
            // Configurar el botón si existe
            if (botonSeleccionar != null)
            {
                botonSeleccionar.onClick.RemoveAllListeners();
                botonSeleccionar.onClick.AddListener(OnCartaClick);
            }
        }
        // Si es inspección, NO configurar botonSeleccionar
    }
    
    public void ConfigurarObjeto(PlantillaObjeto datosObjeto)
    {
        datosMarinero = datosObjeto;
        
        if (datosObjeto == null)
        {
            Debug.LogError("Objeto: datosObjeto es null");
            return;
        }
        
        Debug.Log($"Configurando {tipoVista}: {datosObjeto.nombreMarinero}");
        
        // Configurar texto básico
        if (textoObjeto != null)
            textoObjeto.text = datosObjeto.nombreMarinero;
        else
            Debug.LogWarning($"textoObjeto no asignado en {tipoVista}");
        
        if (precioObjeto != null)
        {
            if (tipoVista == TipoVista.Carta)
                precioObjeto.text = "$" + datosObjeto.precio.ToString();
            else
                precioObjeto.text = "Precio: $" + datosObjeto.precio.ToString();
        }
        
        // Solo mostrar detalles completos en vista de inspección
        if (tipoVista == TipoVista.Inspeccion)
        {
            if (strongText != null)
                strongText.text = "Fuerte vs: " + datosObjeto.strongWith;
            
            if (weakText != null)
                weakText.text = "Débil vs: " + datosObjeto.weakWith;
            
            if (description != null)
                description.text = datosObjeto.descripcion;
        }
        else if (tipoVista == TipoVista.Carta)
        {
            // Ocultar elementos no necesarios en cartas
            if (strongText != null && strongText.gameObject.activeSelf)
                strongText.gameObject.SetActive(false);
            if (weakText != null && weakText.gameObject.activeSelf)
                weakText.gameObject.SetActive(false);
            if (description != null && description.gameObject.activeSelf)
                description.gameObject.SetActive(false);
        }
        
        // Configurar animación
        if (datosObjeto.idleAnimationSprites != null && datosObjeto.idleAnimationSprites.Length > 0)
        {
            currentAnimationSprites = new List<Sprite>(datosObjeto.idleAnimationSprites);
            animationSpeed = datosObjeto.animationSpeed;
            
            if (marineroImage != null)
            {
                marineroImage.sprite = currentAnimationSprites[0];
                marineroImage.gameObject.SetActive(true);
            }
        }
        else if (marineroImage != null)
        {
            marineroImage.gameObject.SetActive(false);
        }
    }
    
    // Método para cartas
    private void OnCartaClick()
    {
        if (datosMarinero == null)
        {
            Debug.LogError("OnCartaClick: datosMarinero es null. ¿Se llamó ConfigurarObjeto?");
            return;
        }
        
        Debug.Log($"Carta clickeada: {datosMarinero.nombreMarinero}");
        
        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.SeleccionarMarinero(datosMarinero);
        }
        else
        {
            Debug.LogError("ShopManager.Instance es null");
        }
    }
    
    // Método público para configurar botón de compra (solo inspección)
    public void ConfigurarBotonCompra(System.Action accionCompra)
    {
        if (tipoVista == TipoVista.Inspeccion)
        {
            Button btnComprar = GetComponentInChildren<Button>();
            if (btnComprar != null)
            {
                btnComprar.onClick.RemoveAllListeners();
                btnComprar.onClick.AddListener(() => accionCompra?.Invoke());
            }
        }
    }
    
    void Update()
    {
        // Solo animar si hay múltiples sprites
        if (currentAnimationSprites != null && currentAnimationSprites.Count > 1)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= (1f / animationSpeed))
            {
                animationTimer = 0;
                currentSpriteIndex = (currentSpriteIndex + 1) % currentAnimationSprites.Count;
                
                if (marineroImage != null)
                    marineroImage.sprite = currentAnimationSprites[currentSpriteIndex];
            }
        }
    }
    
    // Métodos públicos auxiliares
    public PlantillaObjeto GetDatosMarinero()
    {
        return datosMarinero;
    }
    
    public TipoVista GetTipoVista()
    {
        return tipoVista;
    }
    
    // Para activar/desactivar funcionalidades según necesidad
    public void HabilitarAnimacion(bool habilitar)
    {
        enabled = habilitar;
    }
}