using UnityEngine;
using UnityEngine.UI;

public class CannonSkillsTree : MonoBehaviour
{
    [HideInInspector] public Cannon cannon;

    [Header("Buttons")]
    public Button knockbackBtn;
    public Button pierceBtn;
    public Button fireBtn;
    public Button multipleBtn;
    public Button bounceBtn;

    public void Bind(Cannon c)
    {
        cannon = c;
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        if (cannon == null) return;

        knockbackBtn.interactable = cannon.KnockBack < 4;
        pierceBtn.interactable = cannon.Pierce < 4;
        fireBtn.interactable = cannon.Fire < 4;
        multipleBtn.interactable = cannon.MultipleShoot < 4;
        bounceBtn.interactable = cannon.BounceShoots < 4;
    }

    // ===== BUTTON EVENTS =====

    public void UpgradeKnockback()
    {
        cannon.KnockbackUpgrade();
        RefreshButtons();
    }

    public void UpgradePierce()
    {
        cannon.PierceUpgrade();
        RefreshButtons();
    }

    public void UpgradeFire()
    {
        cannon.FireUpgrade();
        RefreshButtons();
    }

    public void UpgradeMultiple()
    {
        cannon.MultipleShootUpgrade();
        RefreshButtons();
    }

    public void UpgradeBounce()
    {
        cannon.BounceShootsUpgrade();
        RefreshButtons();
    }
}