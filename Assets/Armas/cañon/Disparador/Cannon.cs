using UnityEngine;

public class Cannon : Guns
{
    [Header("Skills Tree Levels")]
    public int KnockBack = 0;
    public int Pierce = 0;
    public int Fire = 0;
    public int MultipleShoot = 0;
    public int BounceShoots = 0;

    [Header("UI")]
    public GameObject SkillTreeUI;

    private SkillTreeUIManager uiManager;

    void Start()
    {
        uiManager = FindFirstObjectByType<SkillTreeUIManager>();
    }

    public void OpenSkillTree()
    {
        if (uiManager == null || SkillTreeUI == null)
        {
            Debug.LogError("SkillTreeUIManager o SkillTreeUI no asignado");
            return;
        }

        uiManager.ShowUI(this, SkillTreeUI);
    }

    // ===== UPGRADES =====

    public void KnockbackUpgrade()
    {
        if (KnockBack < 4) KnockBack++;
    }

    public void PierceUpgrade()
    {
        if (Pierce < 4) Pierce++;
    }

    public void FireUpgrade()
    {
        if (Fire < 4) Fire++;
    }

    public void MultipleShootUpgrade()
    {
        if (MultipleShoot < 4) MultipleShoot++;
    }

    public void BounceShootsUpgrade()
    {
        if (BounceShoots < 4) BounceShoots++;
    }
}
