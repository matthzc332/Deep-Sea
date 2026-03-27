// using UnityEngine;
// using UnityEngine.EventSystems;
// using DG.Tweening;

// public class UpDownButton : MonoBehaviour, IPointerClickHandler
// {
//     [Header("Configuración de Movimiento")]
//     public float shopOpenY = 300f;
//     public float shopClosedY = -300f;
//     public float shopAnimTime = 0.25f;

//     [Header("Referencias UI")]
//     public GameObject NPCShop;
//     public bool upper = false; // true = abierto/abajo, false = cerrado/arriba

//     [Header("Background (opcional)")]
//     public GameObject background;

//     private RectTransform npcShopRect;

//     void Start()
//     {
//         if (NPCShop != null)
//         {
//             npcShopRect = NPCShop.GetComponent<RectTransform>();
//         }
//     }

//     public void OnPointerClick(PointerEventData eventData)
//     {
//         ExecuteButtonClick();
//     }

//     void ExecuteButtonClick()
//     {
//         // 1. Invertimos el booleano (Alternamos el estado)
//         upper = !upper;

//         // 2. Definimos el destino basado en el nuevo estado
//         float targetY = upper ? shopOpenY : shopClosedY;

//         // Log realista para debug
//         Debug.Log(upper ? "Abriendo Tienda (Bajando)" : "Cerrando Tienda (Subiendo)");

//         // 3. Controlar background (se activa si la tienda se abre)
//         if (background != null)
//         {
//             background.SetActive(upper);
//         }

//         // 4. Ejecutar la animación con DOTween
//         if (npcShopRect != null)
//         {
//             // Matamos cualquier animación previa para evitar conflictos si el usuario cliquea rápido
//             npcShopRect.DOKill();

//             npcShopRect
//                 .DOAnchorPosY(targetY, shopAnimTime)
//                 .SetEase(Ease.OutCubic)
//                 .SetUpdate(true) // Importante si el juego está en pausa (Timescale 0)
//                 .OnComplete(() => Debug.Log("Tienda en posición: " + targetY));
//         }
//     }
// }

using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI; // Importante para el componente Button

public class UpDownButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuración de Movimiento")]
    public float shopOpenY = 0f;      
    public float shopClosedY = -1000f; 
    public float shopAnimTime = 0.3f;

    [Header("Referencias UI")]
    public GameObject targetPanel;    
    public GameObject fondoNegro;     
    public Button closeButtonHijo; // Arrastra aquí el botón que creaste dentro del panel

    [HideInInspector] public bool isOpen = false;
    private RectTransform panelRect;

    void Start()
    {
        if (targetPanel != null)
        {
            panelRect = targetPanel.GetComponent<RectTransform>();
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shopClosedY);
        }
        
        if (fondoNegro != null) fondoNegro.SetActive(false);

        // Configuramos el botón hijo por código para que siempre funcione
        if (closeButtonHijo != null)
        {
            closeButtonHijo.onClick.AddListener(CloseMenu);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        AnimateMenu(isOpen);
    }

    public void AnimateMenu(bool open)
    {
        float targetY = open ? shopOpenY : shopClosedY;
        
        // Control del fondo oscuro (Si existe)
        if (fondoNegro != null) fondoNegro.SetActive(open);

        if (panelRect != null)
        {
            panelRect.DOKill();
            panelRect.DOAnchorPosY(targetY, shopAnimTime)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);
        }
        
        isOpen = open;
    }

    // Función que llama el botón hijo y el fondo negro
    public void CloseMenu()
    {
        if (isOpen)
        {
            AnimateMenu(false);
        }
    }
}