// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using DG.Tweening;
// using System.Collections;
//esta funcionando

// public class AchievementNotifyUI : MonoBehaviour
// {
//     public TextMeshProUGUI tituloTxt;
//     public Image iconoImg;

//     public void Show(AchievementSO logro)
//     {
//         tituloTxt.text = logro.titulo;
//         if (logro.icono != null)
//             iconoImg.sprite = logro.icono;

//         // Detener animaciones previas para que no se solapen
//         DOTween.Kill(transform);
//         StopAllCoroutines();
//         StartCoroutine(AnimarPopup());
//     }

//     private IEnumerator AnimarPopup()
//     {
//         transform.localScale = Vector3.zero;
//         gameObject.SetActive(true);

//         yield return null; // Espera un frame para asegurar el renderizado

//         // Animación de entrada
//         transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);

//         // Espera de 3 segundos visible
//         yield return new WaitForSeconds(3f);

//         // Animación de salida y desactivación
//         transform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(() => {
//             gameObject.SetActive(false);
//         });
//     }
// }
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using DG.Tweening;

// public class AchievementNotifyUI : MonoBehaviour
// {
//     public TextMeshProUGUI tituloTxt;
//     public Image iconoImg;

//     public void Show(AchievementSO logro)
//     {
//         tituloTxt.text = logro.titulo;

//         if (logro.icono != null)
//             iconoImg.sprite = logro.icono;

//         // Reset escala por si quedó en 0 de una animación anterior
//         transform.localScale = Vector3.zero;

//         DOTween.Kill(transform); // Cancela tweens anteriores si los hay

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





//claudia

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class AchievementNotifyUI : MonoBehaviour
{
    public TextMeshProUGUI tituloTxt;
    public Image iconoImg;

    public void Show(AchievementSO logro)
    {
        tituloTxt.text = logro.titulo;

        if (logro.icono != null)
            iconoImg.sprite = logro.icono;

        StopAllCoroutines();
        DOTween.Kill(transform);
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;

        StartCoroutine(AnimarPopup());
    }

    private IEnumerator AnimarPopup()
    {
        yield return null;

        yield return transform.DOScale(1f, 0.5f)
            .SetEase(Ease.OutBack)
            .WaitForCompletion();

        yield return new WaitForSeconds(3f);

        yield return transform.DOScale(0f, 0.5f)
            .SetEase(Ease.InBack)
            .WaitForCompletion();

        gameObject.SetActive(false);
    }
}