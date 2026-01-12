using UnityEngine;
using UnityEngine.UI;

public class ButtonActionHandler : MonoBehaviour
{
    // Definimos los tipos de acciones posibles
    public enum ActionType { StartGame, ResumeGame, ExitGame, LoadMenu }
    
    [Header("Configuración del Botón")]
    public ActionType accionAAsociar;

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(EjecutarAccion);
        }
    }

    void EjecutarAccion()
    {
        // Verificamos que el Manager persistente exista
        if (GameManager.instance == null) return;

        switch (accionAAsociar)
        {
            case ActionType.StartGame:
                GameManager.instance.StartGame();
                break;
            case ActionType.ResumeGame:
                // GameManager.instance.ResumeGame(); 
                break;
            case ActionType.ExitGame:
                Application.Quit();
                break;
        }
    }
}