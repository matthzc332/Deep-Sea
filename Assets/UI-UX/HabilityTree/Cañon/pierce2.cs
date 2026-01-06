using UnityEngine;

public class Pierce2 : SkillTreeBase
{
    public override void Modificación()
    {
        // Buscamos el script Bullet en este mismo GameObject
        if (TryGetComponent<Bullet>(out Bullet scriptBullet))
        {
            scriptBullet.pierce += 1;
            Debug.Log($"Pierce aumentado a: {scriptBullet.pierce}");
        }
        else
        {
            Debug.LogError($"No se encontró el script Bullet en {gameObject.name}");
        }
    }
}