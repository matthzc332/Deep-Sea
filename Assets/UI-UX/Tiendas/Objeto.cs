using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Objeto : MonoBehaviour
{
    public enum TipoVista { Carta, Inspeccion }
    [SerializeField] private TipoVista tipoVista = TipoVista.Carta;
    
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI textoObjeto;
    [SerializeField] private TextMeshProUGUI precioObjeto;
    [SerializeField] private TextMeshProUGUI strongText;
    [SerializeField] private TextMeshProUGUI weakText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image marineroImage;
    
    [Header("Configuración Carta")]
    [SerializeField] private Button botonSeleccionar;
    
    // Referencia al Manager que creó este objeto (YA NO ES ESTÁTICO)
    private ShopManager miManager;
    private PlantillaObjeto datosMarinero;

    [Header("Animación")]
    [SerializeField] private List<Sprite> currentAnimationSprites;
    [SerializeField] private float animationSpeed = 1f;
    private int currentSpriteIndex = 0;
    private float animationTimer = 0f;

    // AHORA RECIBE EL MANAGER COMO PARÁMETRO
    public void ConfigurarObjeto(PlantillaObjeto datosObjeto, ShopManager manager)
    {
        datosMarinero = datosObjeto;
        miManager = manager;
        
        if (datosObjeto == null) return;

        // Configuración de Textos
        if (textoObjeto != null) textoObjeto.text = datosObjeto.nombre;
        if (precioObjeto != null) precioObjeto.text = (tipoVista == TipoVista.Carta ? "$" : "Precio: $") + datosObjeto.precio;

        if (tipoVista == TipoVista.Inspeccion)
        {
            if (strongText != null) strongText.text = "Fuerte vs: " + datosObjeto.strongWith;
            if (weakText != null) weakText.text = "Débil vs: " + datosObjeto.weakWith;
            if (description != null) description.text = datosObjeto.descripcion;
        }

        // Configuración de Animación
        if (datosObjeto.idleAnimationSprites != null && datosObjeto.idleAnimationSprites.Length > 0)
        {
            currentAnimationSprites = new List<Sprite>(datosObjeto.idleAnimationSprites);
            animationSpeed = datosObjeto.animationSpeed;
            if (marineroImage != null) marineroImage.sprite = currentAnimationSprites[0];
        }

        // Configurar el click de la carta
        if (tipoVista == TipoVista.Carta)
        {
            if (botonSeleccionar == null) botonSeleccionar = GetComponent<Button>();
            if (botonSeleccionar != null)
            {
                botonSeleccionar.onClick.RemoveAllListeners();
                botonSeleccionar.onClick.AddListener(OnCartaClick);
            }
        }
    }

    private void OnCartaClick()
    {
        // En lugar de llamar a ShopManager.Instance, llamamos a NUESTRO manager
        if (miManager != null && datosMarinero != null)
        {
            miManager.SeleccionarMarinero(datosMarinero);
        }
    }

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
        if (currentAnimationSprites != null && currentAnimationSprites.Count > 1)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= (1f / animationSpeed))
            {
                animationTimer = 0;
                currentSpriteIndex = (currentSpriteIndex + 1) % currentAnimationSprites.Count;
                if (marineroImage != null) marineroImage.sprite = currentAnimationSprites[currentSpriteIndex];
            }
        }
    }

    public PlantillaObjeto GetDatosMarinero() => datosMarinero;
}