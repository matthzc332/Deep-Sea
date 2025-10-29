using UnityEngine;
using TMPro;

public class GunAmmoUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Base_Gun gun;       // Referencia al arma
    [SerializeField] private TextMeshProUGUI ammoText; // Referencia al texto TMP

    private void Start()
    {
        // Si no se asignó desde el inspector, intentar buscar automáticamente
        if (gun == null)
            gun = FindObjectOfType<Base_Gun>();

        if (ammoText == null)
            ammoText = GetComponent<TextMeshProUGUI>();

        UpdateAmmoText(); // Mostrar valor inicial
    }

    private void Update()
    {
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