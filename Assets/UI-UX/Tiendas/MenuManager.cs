using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject modalOverlay; // Arrastra el panel oscuro aquí
    public GameObject currentActiveMenu;

    // Llama a esto cuando abras cualquier menú
    public void OpenMenu(GameObject menuToOpen)
    {
        modalOverlay.SetActive(true);
        menuToOpen.SetActive(true);
        currentActiveMenu = menuToOpen;
        
        // Si tu script Up-Down usa animaciones, 
        // aquí dispararías el Trigger de "Up"
    }

    // Llama a esto con el botón de cerrar del panel oscuro
    public void CloseAllMenus()
    {
        if (currentActiveMenu != null)
        {
            // Aquí disparas el Trigger de "Down" de tu script actual
            currentActiveMenu.SetActive(false); 
        }
        modalOverlay.SetActive(false);
        currentActiveMenu = null;
    }
}