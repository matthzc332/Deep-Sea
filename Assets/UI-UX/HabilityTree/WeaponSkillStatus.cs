using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ProgresoHabilidades", menuName = "SkillTree/Progreso")]
public class WeaponSkillStatus : ScriptableObject
{
    public List<string> habilidadesDesbloqueadas = new List<string>();

    public bool EstaDesbloqueada(string nombre)
    {
        return habilidadesDesbloqueadas.Contains(nombre);
    }

    // Método útil para llamar desde un botón de trampas o al empezar partida nueva
    public void ResetearProgreso()
    {
        habilidadesDesbloqueadas.Clear();
    }
}