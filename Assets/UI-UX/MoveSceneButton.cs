using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveSceneButton : MonoBehaviour
{
    // Objeto a mover (normalmente un panel que contiene los botones)
    public GameObject Object;
    private RectTransform ObjectRectTransform;

    // Botones para mover y volver a la posición original
    public Button MoveButton;
    public Button BackButton;

    // Posiciones
    [Header("Posiciones")]
    public Vector2 initialPosition;
    public Vector2 finalPosition;

    [Header("Animación")]
    public float moveDuration = 1f;
    public Ease moveEase = Ease.InOutCubic;

    // Lista de todos los botones de la escena
    private List<Button> allButtons = new List<Button>();

    void Awake()
    {
        if (Object != null)
            ObjectRectTransform = Object.GetComponent<RectTransform>();
    }

    void Start()
    {
        if (ObjectRectTransform == null && Object != null)
            ObjectRectTransform = Object.GetComponent<RectTransform>();

        if (ObjectRectTransform != null)
            ObjectRectTransform.anchoredPosition = initialPosition;

        allButtons.AddRange(FindObjectsOfType<Button>(true));

        if (MoveButton != null)
            MoveButton.onClick.AddListener(MoveToFinalPosition);

        if (BackButton != null)
            BackButton.onClick.AddListener(MoveToInitialPosition);
    }

    private void MoveToFinalPosition()
    {
        if (ObjectRectTransform == null) return;

        DisableAllButtons();
        ObjectRectTransform.DOKill();

        ObjectRectTransform
            .DOAnchorPos(finalPosition, moveDuration)
            .SetEase(moveEase)
            .SetUpdate(true)
            .OnComplete(EnableAllButtons);
    }

    private void MoveToInitialPosition()
    {
        if (ObjectRectTransform == null) return;

        DisableAllButtons();
        ObjectRectTransform.DOKill();

        ObjectRectTransform
            .DOAnchorPos(initialPosition, moveDuration)
            .SetEase(moveEase)
            .SetUpdate(true)
            .OnComplete(EnableAllButtons);
    }

    // Desactiva TODOS los botones de la escena mientras se mueve
    private void DisableAllButtons()
    {
        foreach (Button btn in allButtons)
        {
            if (btn != null)
                btn.interactable = false;
        }
    }

    // Reactiva TODOS los botones al terminar la animación
    private void EnableAllButtons()
    {
        foreach (Button btn in allButtons)
        {
            if (btn != null)
                btn.interactable = true;
        }
    }
}
