using UnityEngine;

public class SkillLoader : MonoBehaviour
{
    // Cambiamos 'SkillStatus' por 'WeaponSkillStatus' que es el nombre de tu clase ScriptableObject
    [SerializeField] private WeaponSkillStatus progreso; 

    void Awake() // Asegúrate de que diga Awake, no Start
    {
        if (progreso == null) return;

        SkillTreeBase[] todasLasHabilidades = GetComponents<SkillTreeBase>();
        foreach (SkillTreeBase habilidad in todasLasHabilidades)
        {
            string nombreHabilidad = habilidad.GetType().Name;
            if (progreso.EstaDesbloqueada(nombreHabilidad))
            {
                habilidad.ActivarHabilidad(); // Esto activa el componente
                habilidad.Desbloquear();       // Esto llama a Modificación()
            }
        }
    }
}