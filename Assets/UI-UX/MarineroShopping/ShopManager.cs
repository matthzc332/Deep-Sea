using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public Transform container;
    public GameObject cartaPrefab; // el diseñador arrastra el prefab aquí cuando esté listo
    public List<MarineroShopping> marinerosDisponibles = new List<MarineroShopping>();

    public int monedaJugador = 500; // ejemplo inicial

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Ejemplo: agregar marineros a la lista (puedes poblarla desde el inspector) no se si va
        //aca o en el panel de unity
        marinerosDisponibles.Clear();

        marinerosDisponibles.Add(new MarineroShopping
        {
            nombre = "Pistolero",
            ventajasGenerales = "Antiaéreo",
            descripcion = "Daña unidades voladoras",
            precio = 120
        });

        marinerosDisponibles.Add(new MarineroShopping
        {
            nombre = "Apostador",
            ventajasGenerales = "Economía",
            descripcion = "Gana monedas extra por ronda",
            precio = 200
        });

        marinerosDisponibles.Add(new MarineroShopping
        {
            nombre = "Capitán",
            ventajasGenerales = "Defensa",
            descripcion = "Otorga escudo a aliados cercanos",
            precio = 300
        });

        marinerosDisponibles.Add(new MarineroShopping
        {
            nombre = "Explorador",
            ventajasGenerales = "Visión",
            descripcion = "Revela zonas ocultas del mapa",
            precio = 180
        });

        LlenarShop();


    }


    //

    public void LlenarShop()
    {
        foreach (Transform child in container) Destroy(child.gameObject);
        foreach (var m in marinerosDisponibles)
        {
            var go = Instantiate(cartaPrefab, container);
            var ui = go.GetComponent<CartaMarineroUI>();
            ui.ConfigurarCarta(m);
        }
    }

    public void IntentarComprar(MarineroShopping m)
    {
        if (monedaJugador >= m.precio)
        {
            monedaJugador -= m.precio;
            ProcesarCompra(m);
            Debug.Log($"Comprado: {m.nombre} - Moneda restante: {monedaJugador}");
        }
        else
        {
            MostrarErrorNoSuficienteMoneda();
            Debug.LogWarning($"No alcanza la moneda para {m.nombre}. Tenés: {monedaJugador}, cuesta: {m.precio}");

        }
    }

    void ProcesarCompra(MarineroShopping m)
    {
        // lógica: añadir al inventario, desbloquear en juego, etc.
        Debug.Log($"Comprado: {m.nombre}");
    }

    void MostrarErrorNoSuficienteMoneda()
    {
        Debug.Log("Moneda insuficiente");
    }




 


}

