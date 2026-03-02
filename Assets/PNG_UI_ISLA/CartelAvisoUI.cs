using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CartelAvisoUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Se ejecuta al hacer SetActive(true) desde BuyCrew
    void OnEnable()
    {
        // Configuración inicial para la animación
        canvasGroup.alpha = 0;
        rectTransform.localScale = Vector3.one * 0.5f;

        // Animación de entrada: Escala con rebote y Fade In
        rectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        canvasGroup.DOFade(1f, 0.4f).SetUpdate(true);
    }

    // Método para el botón de cerrar (la "X")
    public void CerrarCartel()
    {
        // Animación de salida: Se achica y desaparece
        rectTransform.DOScale(0.5f, 0.2f).SetEase(Ease.InBack).SetUpdate(true);
        canvasGroup.DOFade(0f, 0.2f).SetUpdate(true).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}