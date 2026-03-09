// using UnityEngine;

// public class Globo_Explotando : State_Base
// {

//     public override void EnterState()
//     {
//         Debug.Log("�El globo explota!");

//         Destroy(controlledObject); // destruir el globo
//     }
// }
// using UnityEngine;

// public class Globo_Explotando : State_Base
//{
    // public override void EnterState()
    // {
    //     Debug.Log("¡El globo explota!");

    //     // 1. Intentar obtener el AudioSource del globo
    //     AudioSource audioSource = controlledObject.GetComponent<AudioSource>();

    //     if (audioSource != null && audioSource.clip != null)
    //     {
    //         // 2. Reproducir el sonido en la posición actual
    //         // PlayClipAtPoint crea un objeto temporal que se destruye solo al terminar el audio
    //         AudioSource.PlayClipAtPoint(audioSource.clip, controlledObject.transform.position);
    //     }

    //     // 3. Destruir el globo
    //     Destroy(controlledObject); 
    // }
//}

using UnityEngine;



public class Globo_Explotando : State_Base
{
    public override void EnterState()
    {
        AudioSource audioSource = controlledObject.GetComponent<AudioSource>();

        if (audioSource != null)
        {
            // Si el silencio dura exactamente 1 segundo, saltamos a ese punto
            audioSource.time = 1.5f; 
            audioSource.Play();
        }

        // 1. Desactivar lo visual y físico inmediatamente
        var renderer = controlledObject.GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;

        var collider = controlledObject.GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        // 2. Destruir el objeto después de que termine la parte útil del sonido
        // Si el audio total dura 2s y saltaste el 1ro, queda 1s de sonido.
        Destroy(controlledObject, 1.1f); 
    }
}
