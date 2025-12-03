using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject skillTreeMenu;
    public GameObject pauseMenu;
    public GameObject hud;

    void Awake()
    {
        // Si no está asignado, lo busca en la escena automáticamente
        if (hud == null)
            hud = GameObject.Find("HUD");

        if (pauseMenu == null)
            pauseMenu = GameObject.Find("PauseMenu");

        if (skillTreeMenu == null)
            skillTreeMenu = GameObject.Find("SKILLTREE MENU");
    }

    // ------------------- GENERAL -----------------------

    public void StartGame()
    {
        if (hud != null) hud.SetActive(true);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (skillTreeMenu != null) skillTreeMenu.SetActive(false);
    }

    public void PauseGame()
    {
        if (pauseMenu != null) pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
    }

    // ---------------- ÁRBOL DE HABILIDADES --------------------

    public void OpenSkillTree()
    {
        if (skillTreeMenu != null) skillTreeMenu.SetActive(true);
        if (hud != null) hud.SetActive(false);
        Time.timeScale = 0f;
    }

    public void CloseSkillTree()
    {
        if (skillTreeMenu != null) skillTreeMenu.SetActive(false);
        if (hud != null) hud.SetActive(true);
        Time.timeScale = 1f;
    }

    public void ToggleSkillTree()
    {
        if (skillTreeMenu == null) return;

        if (skillTreeMenu.activeSelf)
            CloseSkillTree();
        else
            OpenSkillTree();
    }

    // Llamado desde GameManager
    public void CloseSkillTreeButton()
    {
        CloseSkillTree();
    }
}
