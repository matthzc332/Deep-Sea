using UnityEngine;

public class Pierce1 : SkillTreeBase
{
    [SerializeField] private int cantidadPerforacion = 1; // Configurable en el Inspector
    public override void Modificación()
    {
        // Buscamos el script Bullet en este mismo GameObject
        if (TryGetComponent<Bullet>(out Bullet scriptBullet))
        {
            scriptBullet.pierce += cantidadPerforacion;
            //Debug.Log($"Pierce aumentado a: {scriptBullet.pierce}");
        }
        else
        {
            //Debug.LogError($"No se encontró el script Bullet en {gameObject.name}");
        }
    }
}