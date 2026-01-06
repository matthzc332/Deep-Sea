using UnityEngine;

public class Pierce : SkillTreeBase
{
    [Range(1, 3)]
    [SerializeField] private int nivel = 1;

    public override void Apply()
    {
        if (cannon == null) return;

        cannon.pierceLevel += nivel;
        Debug.Log($"Pierce aplicado. Nivel actual: {cannon.pierceLevel}");
    }
}
