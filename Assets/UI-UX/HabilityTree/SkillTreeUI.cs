using UnityEngine;

public class SkillTreeUIManager : MonoBehaviour
{
    [Header("Parent dentro del Canvas")]
    public Transform uiRoot;
    private GameObject currentUI;
    private CannonSkillsTree currentTree;


    public void ShowUI(Cannon cannon, GameObject uiPrefab)
    {
        if (uiRoot == null)
        {
            Debug.LogError($"{name}: uiRoot no asignado.");
            return;
        }
        if (uiPrefab == null)
        {
            Debug.LogError($"{name}: uiPrefab es null.");
            return;
        }

        Hide();
        currentUI = Instantiate(uiPrefab, uiRoot);
        currentTree = currentUI.GetComponent<CannonSkillsTree>();
        if (currentTree == null)
        {
            Debug.LogError($"{name}: el prefab '{uiPrefab.name}' no tiene WeaponsSkillsTree.");
            return;
        }

        var bindMethod = currentTree.GetType().GetMethod("Bind");
        if (bindMethod != null)
        {
            currentTree.Bind(cannon);
        }
        else
        {
            currentTree.cannon = cannon;
            currentTree.RefreshButtons();
        }
    }

    public void Hide()
    {
        if (currentUI != null)
        {
            Destroy(currentUI);
            currentUI = null;
            currentTree = null;
        }
    }

    public void Refresh()
    {
        if (currentTree != null)
            currentTree.RefreshButtons();
    }
}