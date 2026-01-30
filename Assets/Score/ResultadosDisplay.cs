using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultadosDisplay : MonoBehaviour
{
    [Header("Datos")]
    public ShipData shipData;

    [Header("UI References")]
    public TextMeshProUGUI textoScoreBase;
    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoDinero;
    public TextMeshProUGUI textoBalas;
    public TextMeshProUGUI textoTotal;

    [Header("Botones")]
    public Button botonContinuar;
    void OnEnable()
    {
        CalcularYMostrar();
    }
    void Start()
    {
        // Configuramos el botón solo una vez
        if (botonContinuar != null)
        {
            botonContinuar.onClick.RemoveAllListeners(); // Evita clics duplicados
            botonContinuar.onClick.AddListener(AlPulsarContinuar);
        }
    }

    void CalcularYMostrar()
    {
        if (shipData == null)
        {
            Debug.LogError("Falta asignar el ShipData en el Prefab");
            return;
        }

        Debug.Log("Actualizando Scoreboard..."); // Para ver si funciona en la consola

        long total = (shipData.score * shipData.puntosDeVida) + shipData.dinero + shipData.balasGastadas;

        // El signo ? evita error si olvidaste asignar algun texto
        if (textoScoreBase) textoScoreBase.text = shipData.score.ToString();
        if (textoVida) textoVida.text = shipData.puntosDeVida.ToString();
        if (textoDinero) textoDinero.text = shipData.dinero.ToString();
        if (textoBalas) textoBalas.text = shipData.balasGastadas.ToString();
        if (textoTotal) textoTotal.text = total.ToString();
    }

    void AlPulsarContinuar()
    {
        if (shipData.puntosDeVida <= 0)
        {
            shipData.ResetRunData();
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            // Si estamos en la isla, al cerrar el cartel desactivamos este objeto
            gameObject.SetActive(false);
        }
    }
}