using UnityEngine;
using TMPro;

public class puntaje : MonoBehaviour
{
    // Esto permite que otros scripts accedan a 'puntaje' fácilmente
    public static puntaje instancia;

    private float puntos;
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        // Configuramos la instancia
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update() // Cambiado de FixedUpdate a Update para UI
    {
        // Si quieres que el puntaje suba solo por tiempo, deja esto.
        // Si solo quieres puntos por cofres, borra la siguiente línea:
        puntos += Time.deltaTime;
        ActualizarTexto();
    }

    public void SumarPuntos(float puntosEntrada)
    {
        puntos += puntosEntrada;
        ActualizarTexto(); // Forzamos la actualización visual
    }

    private void ActualizarTexto()
    {
        if (textMesh != null)
            textMesh.text = puntos.ToString("0");
    }
}