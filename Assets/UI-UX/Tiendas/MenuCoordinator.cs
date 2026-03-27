using UnityEngine;
using UnityEngine.UI;

public class MenuCoordinator : MonoBehaviour
{
    [Header("Configuración de Interfaz")]
    public GameObject modalOverlay; // Arrastra el panel oscuro aquí
    public Button closeButton;      // El botón de cerrar (dentro del overlay)

    private GameObject currentActiveMenu;

    void Start()
    {
        // El botón del fondo oscuro también cerrará el menú
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseCurrentMenu);
        
        modalOverlay.SetActive(false);
    }

    // Esta función la llamarás desde tus botones de la isla
    public void OpenMenu(GameObject menuPanel)
    {
        currentActiveMenu = menuPanel;
        modalOverlay.SetActive(true);
        
        // Aquí activamos la lógica de "subir" de tu script original
        // Suponiendo que tu panel tiene el script de movimiento
        menuPanel.SetActive(true); 
    }

    public void CloseCurrentMenu()
    {
        if (currentActiveMenu != null)
        {
            // Aquí podrías llamar a la función de "Bajar" de tu script up-down
            currentActiveMenu.SetActive(false);
        }
        
        modalOverlay.SetActive(false);
        currentActiveMenu = null;
    }
}