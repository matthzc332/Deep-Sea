using UnityEngine;

public abstract class SkillTreeBase : MonoBehaviour
{
    [Header("Configuración de Habilidad")]
    [SerializeField] private GameObject habilidad; // Referencia visual o lógica
    [SerializeField] private bool desbloqueado = false;
    [SerializeField] private bool active = false;

    // Propiedades
    public GameObject Habilidad => habilidad;
    public bool Desbloqueado => desbloqueado;
    public bool Active => active;

    public void Desbloquear()
    {
        // El objeto a modificar es "this.gameObject"
        if (active)
        {
            desbloqueado = true;
            Modificación();
            Debug.Log($"Habilidad {habilidad.name} desbloqueada en {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"No se puede desbloquear {gameObject.name}: habilidad no activa.");
        }
    }

    public abstract void Modificación();

    public void ActivarHabilidad() => active = true;
    public void DesactivarHabilidad() => active = false;
}