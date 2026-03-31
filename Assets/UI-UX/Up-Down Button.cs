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
            Debug.Log($"Instancia creada: {gameObject.name} | ID: {GetInstanceID()}");
            panelRect = targetPanel.GetComponent<RectTransform>();
            // IMPORTANTE: Forzamos la posición cerrada antes de que se vea nada
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shopClosedY);
        }
    }

    void Start()
    {
        // Forzamos el estado lógico a CERRADO para que el primer clic sea siempre OPEN
        isOpen = false;
        isAnimating = false;

        if (closeButtonHijo != null)
        {
            closeButtonHijo.onClick.RemoveAllListeners();
            closeButtonHijo.onClick.AddListener(CloseMenu);

            // Deshabilitar el botón hijo cuando el panel está cerrado
      //      closeButtonHijo.gameObject.SetActive(false);
        }

        if (fondoNegro != null) fondoNegro.SetActive(false);
    }

    // Clic en el botón de la ISLA
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"CLICK recibido | isOpen: {isOpen} | isAnimating: {isAnimating}");

        if (isAnimating) return;

        // Feedback visual
         DOTween.Kill(transform);
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);

        // Lógica directa: si no está abierto, abre. Si está abierto, cierra.
        if (!isOpen)
        {
            OpenMenu();
        }
        // else
        // {
        //     CloseMenu();
        // }
    }

    public void OpenMenu()
    {
        if (isOpen || isAnimating) return;

        isOpen = true; // Cambiamos el estado ANTES de la animación
        Debug.Log($"isOpen seteado a TRUE");

        if (fondoNegro != null) fondoNegro.SetActive(true);
        //prueba boton
     //   if (closeButtonHijo != null) closeButtonHijo.gameObject.SetActive(true);
    //    AnimateMovement(shopOpenY);
    }

    public void CloseMenu()
    {
        Debug.Log($"CloseMenu llamado desde:\n{System.Environment.StackTrace}");
        if (!isOpen || isAnimating) return;

        isOpen = false; // Cambiamos el estado ANTES de la animación

        if (closeButtonHijo != null)
        {
            closeButtonHijo.transform.DOKill();
            closeButtonHijo.transform.localScale = Vector3.one;
            closeButtonHijo.transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), 0.2f);
        }

        AnimateMovement(shopClosedY);
    }
private void AnimateMovement(float targetY)
{
    if (panelRect == null) return;

    DOTween.Kill(panelRect); // ← reemplazá panelRect.DOKill()
    isAnimating = true;
    
    Debug.Log($"AnimateMovement hacia Y={targetY} | isOpen al iniciar: {isOpen}");
    
    panelRect.DOAnchorPosY(targetY, shopAnimTime)
        .SetEase(Ease.OutBack)
        .SetUpdate(true)
        .OnComplete(() => {
            Debug.Log($"Animación completa | isOpen: {isOpen}");
            isAnimating = false;
            if (!isOpen && fondoNegro != null)
                fondoNegro.SetActive(false);
        });
}
    // private void AnimateMovement(float targetY)
    // {
    //     if (panelRect == null) return;

    //     isAnimating = true;
    //     panelRect.DOKill();

    //     panelRect.DOAnchorPosY(targetY, shopAnimTime)
    //         .SetEase(Ease.OutBack)
    //         .SetUpdate(true)
    //         .OnComplete(() =>
    //         {
    //             isAnimating = false;
    //             // Al terminar, si el estado es cerrado, apagamos el fondo
    //             if (!isOpen && fondoNegro != null)
    //             {
    //                 fondoNegro.SetActive(false);

    //                 //prueba boton cerrar
    //                  if (closeButtonHijo != null) 
    //                 closeButtonHijo.gameObject.SetActive(false);
    //             }
    //         });
    // }
    
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