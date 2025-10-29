using UnityEngine;

public class SkillTreeBase : MonoBehaviour
{
    [Header("Configuración de Habilidad")]
    [SerializeField] private GameObject objetoAModificar;
    [SerializeField] private GameObject habilidad;
    [SerializeField] private bool desbloqueado = false;
    [SerializeField] private bool active = false;

    // Propiedades para acceder a los campos privados desde otras clases
    public GameObject ObjetoAModificar => objetoAModificar;
    public GameObject Habilidad => habilidad;
    public bool Desbloqueado => desbloqueado;
    public bool Active => active;

    /// <summary>
    /// Desbloquea la habilidad si está activa
    /// </summary>
    public void Desbloquear()
    {
        if (active && objetoAModificar != null)
        {
            desbloqueado = true;
            
            // Aplicar la modificación al objeto
            Modificación();
            
            Debug.Log($"Habilidad desbloqueada: {habilidad.name} en {objetoAModificar.name}");
        }
        else
        {
            Debug.LogWarning("No se puede desbloquear: la habilidad no está activa o no hay objeto a modificar");
        }
    }

    /// <summary>
    /// Método virtual para aplicar modificaciones al objeto
    /// Debe ser sobrescrito en clases derivadas
    /// </summary>
    public virtual void Modificación()
    {
        // Este método debe ser implementado en las clases hijas
        // con la lógica específica de modificación
        Debug.Log("Modificación base aplicada - Sobrescribe este método en la clase hija");
    }

    /// <summary>
    /// Activa la habilidad para poder ser desbloqueada
    /// </summary>
    public void ActivarHabilidad()
    {
        active = true;
        Debug.Log($"Habilidad activada: {habilidad.name}");
    }

    /// <summary>
    /// Desactiva la habilidad
    /// </summary>
    public void DesactivarHabilidad()
    {
        active = false;
        Debug.Log($"Habilidad desactivada: {habilidad.name}");
    }

    /// <summary>
    /// Establece el objeto que será modificado por esta habilidad
    /// </summary>
    public void SetObjetoAModificar(GameObject nuevoObjeto)
    {
        objetoAModificar = nuevoObjeto;
    }

    /// <summary>
    /// Establece la habilidad asociada
    /// </summary>
    public void SetHabilidad(GameObject nuevaHabilidad)
    {
        habilidad = nuevaHabilidad;
    }
}