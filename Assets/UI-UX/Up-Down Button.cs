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
using UnityEngine.UI;

public class UpDownButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuración de Movimiento")]
    public float shopOpenY = 0f;
    public float shopClosedY = -1000f;
    public float shopAnimTime = 0.3f;

    [Header("Referencias UI")]
    public GameObject targetPanel;    // El panel que este botón abre (Marineros o Habilidades)
    public GameObject fondoNegro;     // El fondo oscuro único de la jerarquía
    public Button closeButtonHijo;    // El botón "X" que está DENTRO del panel

    private bool isOpen = false;
    private bool isAnimating = false;
    private RectTransform panelRect;

    void Awake()
    {
        if (targetPanel != null)
        {
            panelRect = targetPanel.GetComponent<RectTransform>();
            // Empezamos cerrados
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shopClosedY);
        }
    }

    void Start()
    {
        // Configuramos el botón de cerrar que está dentro del panel
        if (closeButtonHijo != null)
        {
            closeButtonHijo.onClick.RemoveAllListeners();
            closeButtonHijo.onClick.AddListener(CloseMenu);
        }
    }

    // Al hacer clic en el botón de la ISLA
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAnimating) return;
        
        if (!isOpen) OpenMenu();
        else CloseMenu();
    }

    public void OpenMenu()
    {
        if (isOpen || isAnimating) return;
        isOpen = true;
        
        // Animación del botón de la isla
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

        if (fondoNegro != null) fondoNegro.SetActive(true);
        AnimateMovement(shopOpenY);
    }

    public void CloseMenu()
    {
        if (!isOpen || isAnimating) return;
        isOpen = false;

        // Animación del botón de cerrar hijo
        if (closeButtonHijo != null)
            closeButtonHijo.transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), 0.2f);

        AnimateMovement(shopClosedY);
    }

    private void AnimateMovement(float targetY)
    {
        isAnimating = true;
        panelRect.DOKill();
        
        panelRect.DOAnchorPosY(targetY, shopAnimTime)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() => {
                isAnimating = false;
                // Si acabamos de cerrar, apagamos el fondo
                if (!isOpen && fondoNegro != null) fondoNegro.SetActive(false);
            });
    }
}