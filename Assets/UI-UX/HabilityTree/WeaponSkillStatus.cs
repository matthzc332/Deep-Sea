using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ProgresoHabilidades", menuName = "SkillTree/Progreso")]
public class WeaponSkillStatus : ScriptableObject
{
    // Guardaremos los nombres de las habilidades desbloqueadas
    public List<string> habilidadesDesbloqueadas = new List<string>();

    public bool EstaDesbloqueada(string nombre)
    {
        return habilidadesDesbloqueadas.Contains(nombre);
    }
}