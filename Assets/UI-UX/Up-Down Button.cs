using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UpDownButton : MonoBehaviour, IPointerClickHandler
{
    public  float shopOpenY = 300f;
    private const float shopClosedY = -300f;
    private const float shopAnimTime = 0.25f;

    public GameObject NPCShop;
    public GameObject otherButton;
    public bool upper = false;
    
    [Header("Background (opcional)")]
    public GameObject background;
    private RectTransform npcShopRect;
    
    void Start()
    {
        if (NPCShop != null)
        {
            npcShopRect = NPCShop.GetComponent<RectTransform>();
        }
    }

    // Este método se llama automáticamente cuando se hace click
    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteButtonClick();
    }

    void ExecuteButtonClick()
{
    float targetY = upper ? shopOpenY : shopClosedY;
    
    Debug.Log(upper ? "Bajando Cartel (Abriendo Tienda)" : "Subiendo Cartel (Cerrando Tienda)");
    
    // Alternar botones
    if (otherButton != null) 
        otherButton.SetActive(true);
    
    gameObject.SetActive(false);
    
    // Controlar background según si se abre (upper=true) o cierra (upper=false)
    if (background != null)
    {
        // Mostrar background cuando se ABRE la tienda, ocultar cuando se CIERRA
        background.SetActive(upper);
        
        // O si prefieres al revés (mostrar cuando se cierra):
        // background.SetActive(!upper);
    }

    // Mover la tienda
    if (npcShopRect != null)
    {
        npcShopRect
            .DOAnchorPosY(targetY, shopAnimTime)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() => Debug.Log("Animación completada"));
    }
    else if (NPCShop != null)
    {
        npcShopRect = NPCShop.GetComponent<RectTransform>();
        if (npcShopRect != null)
        {
            npcShopRect
                .DOAnchorPosY(targetY, shopAnimTime)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);
        }
    }
}
}