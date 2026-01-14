using UnityEngine;

public class SkillLoader : MonoBehaviour
{
    // Cambiamos 'SkillStatus' por 'WeaponSkillStatus' que es el nombre de tu clase ScriptableObject
    [SerializeField] private WeaponSkillStatus progreso; 

    void Start()
    {
        if (progreso == null)
        {
            Debug.LogError("No se ha asignado el ScriptableObject de progreso en SkillLoader");
            return;
        }

        // Buscamos todos los scripts que heredan de SkillTreeBase en este objeto
        SkillTreeBase[] todasLasHabilidades = GetComponents<SkillTreeBase>();

        foreach (SkillTreeBase habilidad in todasLasHabilidades)
        {
            // Obtenemos el nombre de la clase (ej: "pierce")
            string nombreHabilidad = habilidad.GetType().Name;

            if (progreso.EstaDesbloqueada(nombreHabilidad))
            {
                habilidad.ActivarHabilidad(); // active = true
                habilidad.Desbloquear();      // Ejecuta la Modificación()
                Debug.Log($"Auto-activada: {nombreHabilidad}");
            }
        }
    }
}