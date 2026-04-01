using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class UpDownButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuración de Movimiento")]
    public float shopOpenY = 0f;
    public float shopClosedY = -1080f; // Ajusta según el alto de tu resolución
    public float shopAnimTime = 0.4f;

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
            // Forzamos posición inicial cerrada
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, shopClosedY);
        }
    }

    void Start()
    {
        if (closeButtonHijo != null)
        {
            closeButtonHijo.onClick.RemoveAllListeners();
            closeButtonHijo.onClick.AddListener(CloseMenu);
        }

        if (fondoNegro != null) fondoNegro.SetActive(false);
    }

    // Clic en el botón de la ISLA
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAnimating || isOpen) return;

        // Feedback visual del botón clicado
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
        OpenMenu();
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

        // Animación del botón X
        if (closeButtonHijo != null)
        {
            closeButtonHijo.transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), 0.2f);
        }

        AnimateMovement(shopClosedY);
    }

    private void AnimateMovement(float targetY)
    {
        if (panelRect == null) return;

        isAnimating = true;
        panelRect.DOKill(); // Detiene animaciones previas para evitar conflictos

        panelRect.DOAnchorPosY(targetY, shopAnimTime)
            .SetEase(Ease.OutBack) // Ese efecto "rebote" profesional
            .SetUpdate(true)       // Funciona aunque el juego esté en pausa (TimeScale = 0)
            .OnComplete(() => {
                isAnimating = false;
                // Solo apagamos el fondo si el menú terminó de cerrarse
                if (!isOpen && fondoNegro != null)
                    fondoNegro.SetActive(false);
            });
    }
}