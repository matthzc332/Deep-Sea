using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonSkillsTree : MonoBehaviour
{
    [Header("Buttons")]
    public List<Button> KnockbackButtons;
    public List<Button> PierceButtons;
    public List<Button> FireButtons;
    public List<Button> DoubleShootButtons;
    public List<Button> BounceButtons;

    [Header("Skills Tree Levels")]
    public int KnockBack = 0;
    public int Pierce = 0;
    public int Fire = 0;
    public int MultipleShoot = 0;
    public int BounceShoots = 0;

    [Header("Runtime")]
    public Cannon cannon;

    [Header("CloseButton")]
    public Button closeButton;
    public SkillTreeUIManager uiManager;

    private bool _isBound;

    public void Bind(Cannon c)
    {
        cannon = c;
        _isBound = true;
        WireListeners();
        RefreshButtons();
    }
    void Start()
    {
        uiManager = FindFirstObjectByType<SkillTreeUIManager>();

        if (cannon == null)
            Debug.LogWarning($"{name}: Start() sin Bind(cannon). Llamá Bind después de instanciar la UI.");

        if (closeButton != null && uiManager != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() => uiManager.Hide());
        }


        cannon.SkillTreeScript = this;
    }

    void Update()
    {
        if (cannon == null) return;
        KnockBack = cannon.KnockBack;
        Pierce = cannon.Pierce;
        Fire = cannon.Fire;
        MultipleShoot = cannon.MultipleShoot;
        BounceShoots = cannon.BounceShoots;
    }

    void WireListeners()
    {
        ClearListeners(KnockbackButtons);
        ClearListeners(PierceButtons);
        ClearListeners(FireButtons);
        ClearListeners(DoubleShootButtons);
        ClearListeners(BounceButtons);

        // Todos los botones de cada categoría hacen la acción
        AddSameAction(KnockbackButtons, () => { if (cannon != null) { cannon.KnockbackUpgrade(); RefreshButtons(); } });
        AddSameAction(PierceButtons, () => { if (cannon != null) { cannon.PierceUpgrade(); RefreshButtons(); } });
        AddSameAction(FireButtons, () => { if (cannon != null) { cannon.FireUpgrade(); RefreshButtons(); } });
        AddSameAction(DoubleShootButtons, () => { if (cannon != null) { cannon.MultipleShootUpgrade(); RefreshButtons(); } });
        AddSameAction(BounceButtons, () => { if (cannon != null) { cannon.BounceShootsUpgrade(); RefreshButtons(); } });
    }

    void ClearListeners(List<Button> list)
    {
        if (list == null) return;
        foreach (var b in list) if (b != null) b.onClick.RemoveAllListeners();
    }

    void AddSameAction(List<Button> list, UnityEngine.Events.UnityAction action)
    {
        if (list == null) return;
        foreach (var b in list)
        {
            if (b == null) continue;
            b.onClick.AddListener(action);
        }
    }

    public void RefreshButtons()
    {
        if (cannon == null)
        {
            if (_isBound) Debug.LogWarning($"{name}: RefreshButtons() sin cannon (post-Bind).");
            return;
        }

        // DESACTIVAMOSS T0D0S LOS B OTONES
        SetInteractable(KnockbackButtons, false);
        SetInteractable(PierceButtons, false);
        SetInteractable(FireButtons, false);
        SetInteractable(DoubleShootButtons, false);
        SetInteractable(BounceButtons, false);

        // OBTENER EL VALOR DE LAS HABILIDADES
        KnockBack = cannon.KnockBack;
        Pierce = cannon.Pierce;
        Fire = cannon.Fire;
        MultipleShoot = cannon.MultipleShoot;
        BounceShoots = cannon.BounceShoots;

        // ACTIVAR EL SIGUIENTE BOTON
        SafeEnableAtIndex(KnockbackButtons, KnockBack);
        SafeEnableAtIndex(PierceButtons, Pierce);
        SafeEnableAtIndex(FireButtons, Fire);
        SafeEnableAtIndex(DoubleShootButtons, MultipleShoot);
        SafeEnableAtIndex(BounceButtons, BounceShoots);
    }

    void SetInteractable(List<Button> list, bool value)
    {
        if (list == null) return;
        foreach (var b in list) if (b != null) b.interactable = value;
    }

    void SafeEnableAtIndex(List<Button> list, int index)
    {
        if (list == null || list.Count == 0) return;

        //MAXEADO 
        if (index < 0) index = 0;
        if (index >= list.Count) return;
        var btn = list[index];
        if (btn != null) btn.interactable = true;
    }
}