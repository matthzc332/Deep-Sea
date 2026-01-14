using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con el componente Button

public class BuySK : MonoBehaviour
{
    [Header("Referencia al Progreso")]
    public WeaponSkillStatus progresoHabilidades;
    
    private Button miBoton;

    private void Awake()
    {
        miBoton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        // Al activarse el panel o botón, comprobamos si debe estar bloqueado
        ValidarDisponibilidad();
    }

    public void ComprarDirecto()
    {
        Objeto objetoPadre = GetComponentInParent<Objeto>();
        if (objetoPadre == null) return;

        PlantillaObjeto datos = objetoPadre.GetDatos();
        ShopManager tienda = objetoPadre.GetManager();
        GameObject cartaOriginal = objetoPadre.GetCartaOriginal();

        if (datos == null || tienda == null || progresoHabilidades == null)
        {
            Debug.LogError("Faltan referencias en BuySK o el objeto no tiene datos.");
            return;
        }

        // Verificamos si ya la tenemos
        if (datos.esPermanente && progresoHabilidades.EstaDesbloqueada(datos.nombreId))
        {
            Debug.Log("Esta habilidad ya ha sido desbloqueada anteriormente.");
            return;
        }

        if (tienda.monedaJugador >= datos.precio)
        {
            // 1. Procesar la venta
            tienda.ConfirmarVenta(datos, cartaOriginal);

            // 2. Guardar en el ScriptableObject de Progreso
            if (datos.esPermanente)
            {
                if (!progresoHabilidades.habilidadesDesbloqueadas.Contains(datos.nombreId))
                {
                    progresoHabilidades.habilidadesDesbloqueadas.Add(datos.nombreId);
                    
                    #if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(progresoHabilidades);
                    #endif
                }
            }

            // 3. Bloquear inmediatamente después de comprar
            ValidarDisponibilidad();
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
        }
    }

    public void ValidarDisponibilidad()
    {
        Objeto objetoPadre = GetComponentInParent<Objeto>();
        if (objetoPadre == null || progresoHabilidades == null) return;

        PlantillaObjeto datos = objetoPadre.GetDatos();
        
        // Si es permanente y ya está en la lista, desactivamos el botón
        if (datos != null && datos.esPermanente && progresoHabilidades.EstaDesbloqueada(datos.nombreId))
        {
            if (miBoton != null) miBoton.interactable = false;
            // Opcional: Cambiar texto a "Vendido" si tienes una referencia al texto del botón
        }
    }
}