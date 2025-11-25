using UnityEngine;

public class Cannon : Guns
{
    [Header("Skills Tree Levels")]
    public int KnockBack = 0;
    public int Pierce = 0;
    public int Fire = 0;
    public int MultipleShoot = 0;
    public int BounceShoots = 0;

    public GameObject SkillTreeUI;
    public CannonSkillsTree SkillTreeScript;
    public SkillTreeUIManager SkillTreeUIManager;

    void Start()
    {
        SkillTreeUIManager = FindFirstObjectByType<SkillTreeUIManager>();
    }

    void Update()
    {

    }


    //OPEN UI

    public void OpenSkillTree()
    {
        SkillTreeUIManager.ShowUI(this, SkillTreeUI);
    }


    //Skill Upgrade System
    public void KnockbackUpgrade()
    {
        if (KnockBack != 4)
        {
            KnockBack += 1;
            SkillTreeScript.RefreshButtons();
        }
    }

    public void PierceUpgrade()
    {
        if (Pierce != 4)
        {
            Pierce += 1;
            SkillTreeScript.RefreshButtons();
        }
    }

    public void FireUpgrade()
    {
        if (Fire != 4)
        {
            Fire += 1;
            SkillTreeScript.RefreshButtons();
        }
    }

    public void MultipleShootUpgrade()
    {
        if (MultipleShoot != 4)
        {
            MultipleShoot += 1;
            SkillTreeScript.RefreshButtons();
        }
    }

    public void BounceShootsUpgrade()
    {
        if (BounceShoots != 4)
        {
            BounceShoots += 1;
            SkillTreeScript.RefreshButtons();
        }
    }
}