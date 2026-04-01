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


// using UnityEngine;
// using UnityEngine.EventSystems;
// using DG.Tweening;
// using UnityEngine.UI;

// public class UpDownButton : MonoBehaviour, IPointerClickHandler
// {
//     [Header("Configuración de Movimiento")]
//     public float shopOpenY = 0f;
//     public float shopClosedY = -1000f;
//     public float shopAnimTime = 0.4f; // Un poco más lento para notar el Ease.OutBack

//     [Header("Referencias UI")]
//     public GameObject targetPanel;    
//     public GameObject fondoNegro;     
//     public Button closeButtonHijo;    

//     private bool isOpen = false;
//     private bool isAnimating = false;
//     private RectTransform panelRect;

//     void Awake()
//     {
//         if (targetPanel != null)
//         {
//             panelRect = targetPanel.GetComponent<RectTransform>();
//             // Posición inicial cerrada
//             panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shopClosedY);
//         }
//     }

//     void Start()
//     {
//         if (closeButtonHijo != null)
//         {
//             closeButtonHijo.onClick.AddListener(CloseMenu);
//         }

//         if (fondoNegro != null) fondoNegro.SetActive(false);
//     }

//     public void OnPointerClick(PointerEventData eventData)
//     {
//         // Si ya está abierto o animando, no hacemos nada aquí 
//         // (El botón de cerrar se encarga de lo demás)
//         if (isOpen || isAnimating) return;

//         transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
//         OpenMenu();
//     }

//     public void OpenMenu()
//     {
//         if (isOpen || isAnimating) return;

//         isOpen = true;
//         if (fondoNegro != null) fondoNegro.SetActive(true);

//         // Habilitar el botón de cierre
//         if (closeButtonHijo != null) closeButtonHijo.interactable = true;

//         AnimateMovement(shopOpenY);
//     }

//     public void CloseMenu()
//     {
//         if (!isOpen || isAnimating) return;

//         isOpen = false;

//         // Deshabilitar el botón de cierre para evitar doble clic
//         if (closeButtonHijo != null)
//         {
//             closeButtonHijo.interactable = false;
//             closeButtonHijo.transform.DOPunchScale(new Vector3(-0.1f, -0.1f, -0.1f), 0.2f);
//         }

//         AnimateMovement(shopClosedY);
//     }

//     private void AnimateMovement(float targetY)
//     {
//         if (panelRect == null) return;

//         isAnimating = true;
//         panelRect.DOKill(); // Detiene cualquier animación previa

//         panelRect.DOAnchorPosY(targetY, shopAnimTime)
//             .SetEase(Ease.OutBack)
//             .SetUpdate(true) // Funciona aunque el juego esté pausado
//             .OnComplete(() => {
//                 isAnimating = false;
//                 if (!isOpen && fondoNegro != null)
//                     fondoNegro.SetActive(false);
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
    public float shopAnimTime = 0.4f; // Un poco más lento para que sea suave

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
            // 1. Forzar estado inicial: Cerrado y activo (pero fuera de pantalla)
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shopClosedY);
            targetPanel.SetActive(true); 
        }
        
        if (fondoNegro != null) fondoNegro.SetActive(false);
    }

    void Start()
    {
        // 2. Limpiar y reasignar el listener para evitar duplicados o fallos
        if (closeButtonHijo != null)
        {
            closeButtonHijo.onClick.RemoveAllListeners();
            closeButtonHijo.onClick.AddListener(CloseMenu);
        }
        
        isOpen = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 3. Evitar registros de clics si ya está en movimiento
      //  if (isAnimating) return;

        // Feedback visual al tocar el botón de la isla
      //  transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

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
        // 4. Si intentas cerrar algo que no está abierto, salimos
        if (!isOpen || isAnimating) return;
        
        isOpen = false;

        // Feedback visual del botón "X"
        if (closeButtonHijo != null)
            closeButtonHijo.transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), 0.2f);

        AnimateMovement(shopClosedY);
    }

    private void AnimateMovement(float targetY)
    {
        if (panelRect == null) return;

        isAnimating = true;
        panelRect.DOKill(); // Detiene cualquier animación previa
        
        // 5. Usamos OutCubic o OutQuad para evitar el rebote del OutBack si causa problemas de clics
        panelRect.DOAnchorPosY(targetY, shopAnimTime)
            .SetEase(Ease.OutCubic) 
            .SetUpdate(true) // Funciona aunque el tiempo esté pausado
            .OnComplete(() => {
                isAnimating = false;
                // Solo apagamos el fondo si el menú terminó de cerrarse
                if (!isOpen && fondoNegro != null) fondoNegro.SetActive(false);
            });
    }
}


// using UnityEngine;
// using UnityEngine.EventSystems;
// using DG.Tweening;
// using UnityEngine.UI;

// public class UpDownButton : MonoBehaviour, IPointerClickHandler
// {
//     [Header("Configuración de Movimiento")]
//     public float shopOpenY = 0f;
//     public float shopClosedY = -1000f;
//     public float shopAnimTime = 0.4f;

//     [Header("Referencias UI")]
//     public GameObject targetPanel;    
//     public GameObject fondoNegro;     
//     public Button closeButtonHijo;    

//     private bool isOpen = false;
//     private bool isAnimating = false;
//     private RectTransform panelRect;

//     void Awake()
//     {
//         if (targetPanel != null)
//         {
//             panelRect = targetPanel.GetComponent<RectTransform>();
//             // Forzamos la posición inicial cerrada al arrancar
//             panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shopClosedY);
//             targetPanel.SetActive(true); 
//         }
        
//         if (fondoNegro != null) fondoNegro.SetActive(false);
//     }

//     void Start()
//     {
//         // Configuramos el botón X (el que está dentro del panel)
//         if (closeButtonHijo != null)
//         {
//             closeButtonHijo.onClick.RemoveAllListeners();
//             closeButtonHijo.onClick.AddListener(CloseMenu);
//         }
        
//         isOpen = false;
//     }

//     // Este es el clic en el botón de la barra de navegación (Isla)
//     public void OnPointerClick(PointerEventData eventData)
//     {
//         if (isAnimating) return;

//         // Feedback visual
//         transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

//         if (!isOpen) OpenMenu();
//         else CloseMenu(); // Si ya está abierto, lo cerramos desde aquí también
//     }

//     public void OpenMenu()
//     {
//         if (isOpen || isAnimating) return;
        
//         isOpen = true;
//         if (fondoNegro != null) fondoNegro.SetActive(true);
        
//         AnimateMovement(shopOpenY);
//     }

//     public void CloseMenu()
//     {
//         // Si no está abierto o está animando, no hacemos nada
//         if (!isOpen || isAnimating) return;
        
//         isOpen = false;

//         // Animación del botón X
//         if (closeButtonHijo != null)
//             closeButtonHijo.transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), 0.2f);

//         AnimateMovement(shopClosedY);
//     }

//     private void AnimateMovement(float targetY)
//     {
//         if (panelRect == null) return;

//         isAnimating = true;
//         panelRect.DOKill(); // Evita que se solapen animaciones si clickeas rápido
        
//         panelRect.DOAnchorPosY(targetY, shopAnimTime)
//             .SetEase(Ease.OutCubic) 
//             .SetUpdate(true) // Importante por si el juego está pausado
//             .OnComplete(() => {
//                 isAnimating = false;
//                 // Al terminar de bajar, apagamos el fondo negro
//                 if (!isOpen && fondoNegro != null) fondoNegro.SetActive(false);
//             });
//     }
// }
