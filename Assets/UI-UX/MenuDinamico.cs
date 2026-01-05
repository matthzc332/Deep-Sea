using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuDinamico : MonoBehaviour
{
    [Header("Botón que queda activo")]
    public Button closeButton;

    [Header("Animación")]
    public Ease ease = Ease.InOutCubic;

    private readonly List<Button> allButtons = new List<Button>();
    private RectTransform currentRect;
    private Tween currentTween;

    void Start()
    {
        // Cachea todos los botones de la escena (incluye inactivos)
        allButtons.AddRange(FindObjectsOfType<Button>(true));

        // Listener seguro para Unity (SIN parámetros)
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(CerrarActualBtn);
            closeButton.onClick.AddListener(CerrarActualBtn);
        }
    }

    // ================= API PÚBLICA =================

    /// <summary>
    /// Despliega un menú de ARRIBA hacia ABAJO
    /// </summary>
    public void DesplegarVertical(GameObject objNoVisible, float tiempo)
    {
        if (objNoVisible == null) return;

        Preparar(objNoVisible);

        // Pivot arriba
        currentRect.pivot = new Vector2(currentRect.pivot.x, 1f);
        currentRect.localScale = new Vector3(1f, 0f, 1f);

        AnimarA(new Vector3(1f, 1f, 1f), tiempo);
    }

    /// <summary>
    /// Despliega un menú de IZQUIERDA a DERECHA
    /// </summary>
    public void DesplegarHorizontal(GameObject objNoVisible, float tiempo)
    {
        if (objNoVisible == null) return;

        Preparar(objNoVisible);

        // Pivot izquierda
        currentRect.pivot = new Vector2(0f, currentRect.pivot.y);
        currentRect.localScale = new Vector3(0f, 1f, 1f);

        AnimarA(new Vector3(1f, 1f, 1f), tiempo);
    }

    /// <summary>
    /// Cierra el menú actual
    /// </summary>
    public void CerrarActual(float tiempo = 0.25f)
    {
        if (currentRect == null) return;

        Vector3 targetScale;

        bool esVertical = currentRect.pivot.y > 0.9f;
        bool esHorizontal = currentRect.pivot.x < 0.1f;

        if (esVertical && !esHorizontal)
            targetScale = new Vector3(1f, 0f, 1f);
        else if (esHorizontal && !esVertical)
            targetScale = new Vector3(0f, 1f, 1f);
        else
            targetScale = Vector3.zero;

        DisableAllButtonsExceptClose();

        currentTween?.Kill();
        currentRect.DOKill();

        currentTween = currentRect
            .DOScale(targetScale, tiempo)
            .SetEase(ease)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                currentRect.gameObject.SetActive(false);
                EnableAllButtons();
            });
    }

    // ================= INTERNOS =================

    // Wrapper SIN parámetros (UnityAction compatible)
    private void CerrarActualBtn()
    {
        CerrarActual();
    }

    private void Preparar(GameObject obj)
    {
        currentRect = obj.GetComponent<RectTransform>();
        if (currentRect == null) return;

        obj.SetActive(true);

        DisableAllButtonsExceptClose();

        currentTween?.Kill();
        currentRect.DOKill();
    }

    private void AnimarA(Vector3 targetScale, float tiempo)
    {
        currentTween = currentRect
            .DOScale(targetScale, tiempo)
            .SetEase(ease)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // Menú abierto: solo cerrar activo
                DisableAllButtonsExceptClose();
            });
    }

    private void DisableAllButtonsExceptClose()
    {
        foreach (Button btn in allButtons)
        {
            if (btn == null) continue;
            btn.interactable = (btn == closeButton);
        }
    }

    private void EnableAllButtons()
    {
        foreach (Button btn in allButtons)
        {
            if (btn != null)
                btn.interactable = true;
        }
    }
}
