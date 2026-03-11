// using UnityEngine;



// public class Globo_Explotando : State_Base
// {
//     public override void EnterState()
//     {
//         AudioSource audioSource = controlledObject.GetComponent<AudioSource>();

//         if (audioSource != null)
//         {
//             // Si el silencio dura exactamente 1 segundo, saltamos a ese punto
//             audioSource.time = 1.5f; 
//             audioSource.Play();
//         }

//         // 1. Desactivar lo visual y físico inmediatamente
//         var renderer = controlledObject.GetComponent<SpriteRenderer>();
//         if (renderer != null) renderer.enabled = false;

//         var collider = controlledObject.GetComponent<Collider2D>();
//         if (collider != null) collider.enabled = false;

//         // 2. Destruir el objeto después de que termine la parte útil del sonido
//         // Si el audio total dura 2s y saltaste el 1ro, queda 1s de sonido.
//         Destroy(controlledObject, 1.1f); 
//     }
// }



using UnityEngine;

public class Globo_Explotando : State_Base
{
    public override void EnterState()
    {
        // 1. Intentar obtener componentes
        AudioSource audioSource = controlledObject.GetComponent<AudioSource>();
        Animator animator = controlledObject.GetComponent<Animator>();
        Collider2D collider = controlledObject.GetComponent<Collider2D>();

        // 2. Reproducir sonido (manteniendo tu ajuste de tiempo)
        if (audioSource != null)
        {
            audioSource.time = 1.5f; 
            audioSource.Play();
        }

        // 3. Lanzar la animación de explosión
        if (animator != null)
        {
            // Asegúrate de que en el Animator el parámetro se llame "Explotar"
            animator.SetTrigger("Explotar");
        }

        // 4. Desactivar colisiones para que no siga haciendo daño mientras explota
        if (collider != null) collider.enabled = false;

        // 5. Destruir el objeto después de la animación y el sonido
        // Ajusta el tiempo (1.1f) según lo que dure tu nueva animación
        Destroy(controlledObject, 1.1f);
    }
}