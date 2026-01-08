using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Ship Player;

    private Image image;

    public Sprite spr5hp;
    public Sprite spr4hp;
    public Sprite spr3hp;
    public Sprite spr2hp;
    public Sprite spr1hp;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        // 1. Verificamos si tenemos la referencia al Barco
        if (Player == null)
        {
            // Si no lo tenemos, lo buscamos en la escena
            Player = FindObjectOfType<Ship>();

            // Si después de buscarlo sigue siendo null (aún no se ha instanciado),
            // detenemos la ejecución aquí para que no de error.
            if (Player == null)
            {
                return;
            }
        }

        // --- A partir de aquí ya sabemos que Player EXISTE seguro ---

        if (Player.vida >= 6) // Cambié == 6 por >= 6 por seguridad
        {
            // Opcional: Podrías poner un sprite de vida llena aquí si tuvieras
            return;
        }
        else if (Player.vida == 5)
        {
            image.sprite = spr5hp;
        }
        else if (Player.vida == 4)
        {
            image.sprite = spr4hp;
        }
        else if (Player.vida == 3)
        {
            image.sprite = spr3hp;
        }
        else if (Player.vida == 2)
        {
            image.sprite = spr2hp;
        }
        else if (Player.vida == 1)
        {
            image.sprite = spr1hp;
        }
        // Opcional: Qué pasa si vida es 0? Podrías querer desactivar la imagen o poner sprite vacío.
    }
}