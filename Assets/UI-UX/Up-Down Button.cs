// using UnityEngine;
// using UnityEngine.EventSystems;
// using DG.Tweening;
// using UnityEngine.UI;

// public class UpDownButton : MonoBehaviour, IPointerClickHandler
// {
//     [Header("Configuración de Movimiento")]
//     public float shopOpenY = 0f;
//     public float shopClosedY = -1000f;
//     public float shopAnimTime = 0.3f;

//     [Header("Referencias UI")]
//     public GameObject targetPanel;    // El panel que este botón abre (Marineros o Habilidades)
//     public GameObject fondoNegro;     // El fondo oscuro único de la jerarquía
//     public Button closeButtonHijo;    // El botón "X" que está DENTRO del panel

//     private bool isOpen = false;
//     private bool isAnimating = false;
//     private RectTransform panelRect;

//     void Awake()
//     {
//         if (targetPanel != null)
//         {
//             panelRect = targetPanel.GetComponent<RectTransform>();
//             // Empezamos cerrados
//             panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shopClosedY);
//         }
//     }

//     void Start()
//     {
//         // Configuramos el botón de cerrar que está dentro del panel
//         if (closeButtonHijo != null)
//         {
//             closeButtonHijo.onClick.RemoveAllListeners();
//             closeButtonHijo.onClick.AddListener(CloseMenu);
//         }
//     }

//     // Al hacer clic en el botón de la ISLA
//     public void OnPointerClick(PointerEventData eventData)
//     {
//         if (isAnimating) return;
        
//         if (!isOpen) OpenMenu();
//         else CloseMenu();
//     }

//     public void OpenMenu()
//     {
//         if (isOpen || isAnimating) return;
//         isOpen = true;
        
//         // Animación del botón de la isla
//         transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

//         if (fondoNegro != null) fondoNegro.SetActive(true);
//         AnimateMovement(shopOpenY);
//     }


//   public void CloseMenu()
//     {
//         if (!isOpen || isAnimating) return;
//         isOpen = false;

//         // Animación del botón de cerrar hijo
//         if (closeButtonHijo != null)
//             closeButtonHijo.transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), 0.2f);

//         AnimateMovement(shopClosedY);
//     }
//     // public void CloseMenu()
//     // {
//     //     if (!isOpen || isAnimating) return;
//     //     isOpen = false;

//     //     // Animación del botón de cerrar hijo
//     //     if (closeButtonHijo != null)
//     //         closeButtonHijo.transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), 0.2f);

//     //     AnimateMovement(shopClosedY);
//     // }


//     private void AnimateMovement(float targetY)
// {
//     // SEGURIDAD: Si el panel no existe, salimos antes de que DOTween explote
//     if (targetPanel == null || panelRect == null) 
//     {
//         Debug.LogError($"¡Ojo! Falta el Target Panel en {gameObject.name}");
//         isAnimating = false;
//         return;
//     }

//     isAnimating = true;
//     panelRect.DOKill();
    
//     panelRect.DOAnchorPosY(targetY, shopAnimTime)
//         .SetEase(Ease.OutBack)
//         .SetUpdate(true)
//         .OnComplete(() => {
//             isAnimating = false;
//             if (!isOpen && fondoNegro != null) fondoNegro.SetActive(false);
//         });
// }}



//     private void AnimateMovement(float targetY)
//     {
//         isAnimating = true;
//         panelRect.DOKill();
        
//         panelRect.DOAnchorPosY(targetY, shopAnimTime)
//             .SetEase(Ease.OutBack)
//             .SetUpdate(true)
//             .OnComplete(() => {
//                 isAnimating = false;
//                 // Si acabamos de cerrar, apagamos el fondo
//                 if (!isOpen && fondoNegro != null) fondoNegro.SetActive(false);
//             });
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
    public GameObject targetPanel;
    public GameObject fondoNegro;
    public Button closeButtonHijo;

    private bool isOpen = false;
    private bool isAnimating = false;
    private RectTransform panelRect;

    void Awake()
    {
        if (targetPanel != null)
        {
            panelRect = targetPanel.GetComponent<RectTransform>();
            // Forzamos la posición inicial CERRADA para que no haya dudas
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shopClosedY);
            targetPanel.SetActive(true);
        }
    }

    void Start()
    {
        // SINCRONIZACIÓN: Forzamos el estado lógico a falso porque Awake lo cerró
        isOpen = false;

        if (closeButtonHijo != null)
        {
            closeButtonHijo.onClick.RemoveAllListeners();
            closeButtonHijo.onClick.AddListener(CloseMenu);
        }

        // Si por alguna razón el fondo negro quedó prendido en la escena, lo apagamos
        if (fondoNegro != null) fondoNegro.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAnimating) return;
        
        // Si clickeamos el botón de la isla
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

        if (!isOpen) OpenMenu();
        else CloseMenu();
    }

    public void OpenMenu()
    {
        if (isOpen || isAnimating) return;
        isOpen = true;
        
        if (fondoNegro != null) fondoNegro.SetActive(true);
        AnimateMovement(shopOpenY);
    }

    public void CloseMenu()
    {
        if (!isOpen || isAnimating) return;
        isOpen = false;

        if (closeButtonHijo != null)
            closeButtonHijo.transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), 0.2f);

        AnimateMovement(shopClosedY);
    }

    private void AnimateMovement(float targetY)
    {
        if (targetPanel == null || panelRect == null) return;

        isAnimating = true;
        panelRect.DOKill();
        
        panelRect.DOAnchorPosY(targetY, shopAnimTime)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() => {
                isAnimating = false;
                // Verificación extra: si cerramos, el fondo SE APAGA sí o sí
                if (!isOpen && fondoNegro != null) fondoNegro.SetActive(false);
            });
    }
}