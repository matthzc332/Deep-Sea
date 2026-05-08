using UnityEngine;
using System.Collections;

public class AutoDesactivar : MonoBehaviour
{
    [SerializeField] private float retraso = 0.3f;
    private Coroutine rutinaActual;

    // OnEnable se ejecuta CADA VEZ que el objeto pasa de estar apagado a encendido
    void OnEnable()
    {
        // Por seguridad, si ya había una cuenta atrás, la cancelamos para no solapar
        if (rutinaActual != null) StopCoroutine(rutinaActual);
        
        rutinaActual = StartCoroutine(DesactivarTrasTiempo());
    }

    IEnumerator DesactivarTrasTiempo()
    {
        yield return new WaitForSeconds(retraso);
        gameObject.SetActive(false);
    }
}