using UnityEngine;

public class ShipInit : MonoBehaviour
{
    // Este campo aparecerá en el Inspector para que arrastres tu ShipData
    [Header("Configuración de Datos")]
    public ShipData datosDelBarco;

    void Start()
    {
        if (datosDelBarco == null)
        {
            Debug.LogError("No has asignado el Scriptable Object 'ShipData' en el inspector de " + gameObject.name);
            return;
        }

        InicializarBarco();
    }

    private void InicializarBarco()
    {
        // 1. Actualizar vida del script Ship
        Ship shipScript = GetComponent<Ship>();
        if (shipScript != null)
        {
            shipScript.HP = datosDelBarco.puntosDeVida;
            shipScript.vida = datosDelBarco.puntosDeVida; // Sincroniza la variable que usas en Update
        }

        // 2. Buscar el objeto "Arma" y actualizar su munición
        Transform armaObj = transform.Find("Arma");
        if (armaObj != null)
        {
            Base_Gun gunScript = armaObj.GetComponent<Base_Gun>();
            if (gunScript != null)
            {
                gunScript.amount_ammunition = datosDelBarco.municion;
            }
        }

        // 3. Configurar Posiciones e instanciar Prefabs
        ConfigurarPuntoDePosicion("Posicion1", datosDelBarco.posicion1);
        ConfigurarPuntoDePosicion("Posicion2", datosDelBarco.posicion2);
    }

    private void ConfigurarPuntoDePosicion(string nombreHijo, GameObject prefab)
    {
        if (prefab == null) return;

        Transform punto = transform.Find(nombreHijo);
        if (punto != null)
        {
            // Instancia el prefab como hijo de la posición para que se mueva con el barco
            Instantiate(prefab, punto.position, punto.rotation, punto);
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto hijo: " + nombreHijo);
        }
    }
}