// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using DG.Tweening;
// using System.Collections;

// public class AchievementNotifyUI : MonoBehaviour
// {
//     public TextMeshProUGUI tituloTxt;
//     public Image iconoImg;

//     public void Show(AchievementSO logro)
//     {
//         tituloTxt.text = logro.titulo;

//         if (logro.icono != null)
//             iconoImg.sprite = logro.icono;

//         StopAllCoroutines();
//         StartCoroutine(AnimarPopup());
//     }

//     private IEnumerator AnimarPopup()
//     {
//         transform.localScale = Vector3.zero;
//         gameObject.SetActive(true);

//         yield return null; // espera un frame para que Unity procese el SetActive

//         DOTween.Kill(transform);

//         transform.DOScale(1f, 0.5f)
//             .SetEase(Ease.OutBack)
//             .OnComplete(() =>
//             {
//                 transform.DOScale(0f, 0.5f)
//                     .SetDelay(3f)
//                     .OnComplete(() => gameObject.SetActive(false));
//             });
//     }
// }
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class AchievementNotifyUI : MonoBehaviour
{
    public TextMeshProUGUI tituloTxt;
    public Image iconoImg;

    public void PrepararDatos(AchievementSO logro)
    {
        tituloTxt.text = logro.titulo;

        if (logro.icono != null)
            iconoImg.sprite = logro.icono;
    }

    public void Animar()
    {
        DOTween.Kill(transform);
        transform.localScale = Vector3.zero;

        transform.DOScale(1f, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                transform.DOScale(0f, 0.5f)
                    .SetDelay(3f)
                    .OnComplete(() => gameObject.SetActive(false));
            });
    }
}