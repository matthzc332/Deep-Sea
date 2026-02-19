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

    void Start()
    {
        CalcularYMostrar();
        
        if (botonContinuar != null)
        {
            botonContinuar.onClick.AddListener(AlPulsarContinuar);
        }
    }

    void CalcularYMostrar()
    {
        if (shipData == null) return;

        // 1. Calculamos el total
        long total = (shipData.score * shipData.puntosDeVida) + shipData.dinero + shipData.balasGastadas;

        // 2. ¡IMPORTANTE! Guardar el resultado en el ScriptableObject
        // para que sea persistente entre escenas.
        shipData.score = (int)total;

        // 3. Mostrar en UI
        if (textoScoreBase) textoScoreBase.text = shipData.score.ToString();
        if (textoVida)      textoVida.text = shipData.puntosDeVida.ToString();
        if(textoDinero)    textoDinero.text = shipData.dinero.ToString();
        if(textoBalas)     textoBalas.text = shipData.balasGastadas.ToString();
        if(textoTotal)     textoTotal.text = total.ToString();
    }

    void AlPulsarContinuar()
    {
        // Si la vida es 0, es Game Over -> Reiniciar Juego
        if (shipData.puntosDeVida <= 0)
        {
            shipData.ResetRunData(); // Reiniciar contadores
            SceneManager.LoadScene("ISLA"); // O el nombre de tu escena de menú
        }
        else
        {
            // Si estamos vivos (fin de oleada) -> Cerrar cartel
            gameObject.SetActive(false);
        }
    }
}