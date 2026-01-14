using UnityEngine;
using TMPro;

public class GunAmmoUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Base_Gun gun;       // Referencia al arma
    [SerializeField] private TextMeshProUGUI ammoText; // Referencia al texto TMP

    private void Start()
    {
        // Solo buscamos el componente de texto al inicio (el UI ya existe en la escena)
        if (ammoText == null)
            ammoText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        // 1. Verificamos si la variable 'gun' está vacía
        if (gun == null)
        {
            // Intentamos encontrarla en la escena
            gun = FindFirstObjectByType<Base_Gun>();

            // Si después de buscarla sigue siendo null (el barco aún no se generó),
            // detenemos la ejecución de este frame con 'return'.
            if (gun == null)
            {
                return;
            }
        }

        // --- Si llegamos aquí, significa que 'gun' YA EXISTE ---

        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        if (gun != null && ammoText != null)
        {
            ammoText.text = gun.amount_ammunition.ToString();
        }
    }
}