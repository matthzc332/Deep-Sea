// using UnityEngine;
// using UnityEngine.UI; // Necesario para interactuar con el componente Button

// public class BuySK : MonoBehaviour
// {
//     [Header("Referencia al Progreso")]
//     public WeaponSkillStatus progresoHabilidades;
    
//     private Button miBoton;

//     private void Awake()
//     {
//         miBoton = GetComponent<Button>();
//     }

//     private void OnEnable()
//     {
//         // Al activarse el panel o botón, comprobamos si debe estar bloqueado
//         ValidarDisponibilidad();
//     }

//     public void ComprarDirecto()
//     {
//         Objeto objetoPadre = GetComponentInParent<Objeto>();
//         if (objetoPadre == null) return;

//         PlantillaObjeto datos = objetoPadre.GetDatos();
//         ShopManager tienda = objetoPadre.GetManager();
//         GameObject cartaOriginal = objetoPadre.GetCartaOriginal();

//         if (datos == null || tienda == null || progresoHabilidades == null)
//         {
//             Debug.LogError("Faltan referencias en BuySK o el objeto no tiene datos.");
//             return;
//         }

//         // Verificamos si ya la tenemos
//         if (datos.esPermanente && progresoHabilidades.EstaDesbloqueada(datos.nombreId))
//         {
//             Debug.Log("Esta habilidad ya ha sido desbloqueada anteriormente.");
//             return;
//         }

//         if (tienda.monedaJugador >= datos.precio)
//         {
//             // 1. Procesar la venta
//             tienda.ConfirmarVenta(datos, cartaOriginal);

//             // 2. Guardar en el ScriptableObject de Progreso
//             if (datos.esPermanente)
//             {
//                 if (!progresoHabilidades.habilidadesDesbloqueadas.Contains(datos.nombreId))
//                 {
//                     progresoHabilidades.habilidadesDesbloqueadas.Add(datos.nombreId);
                    
//                     #if UNITY_EDITOR
//                     UnityEditor.EditorUtility.SetDirty(progresoHabilidades);
//                     #endif
//                 }
//             }

//             // 3. Bloquear inmediatamente después de comprar
//             ValidarDisponibilidad();
//         }
//         else
//         {
//             Debug.Log("No tienes suficiente dinero.");
//         }
//     }

//     public void ValidarDisponibilidad()
//     {
//         Objeto objetoPadre = GetComponentInParent<Objeto>();
//         if (objetoPadre == null || progresoHabilidades == null) return;

//         PlantillaObjeto datos = objetoPadre.GetDatos();
        
//       //  Si es permanente y ya está en la lista, desactivamos el botón
//         if (datos != null && datos.esPermanente && progresoHabilidades.EstaDesbloqueada(datos.nombreId))
//         {
//             if (miBoton != null) miBoton.interactable = false;
//             // Opcional: Cambiar texto a "Vendido" si tienes una referencia al texto del botón
//         }
//     }
// }

// using UnityEngine;
// using UnityEngine.UI;
// using DG.Tweening;
// using TMPro;

// public class buySK : MonoBehaviour
// {
//     [Header("Referencia al Progreso")]
//     public WeaponSkillStatus progresoHabilidades; 
    
//     [Header("Configuración de UI")]
//     [SerializeField] private TextMeshProUGUI textoBoton; 
//     [SerializeField] private Image fondoTarjeta;      
//     [SerializeField] private Color colorBloqueado = new Color(0.5f, 0.5f, 0.5f, 1f);

//     private Button miBoton;
//     private RectTransform rectTransform;

//     private void Awake()
//     {
//         miBoton = GetComponent<Button>();
//         rectTransform = GetComponent<RectTransform>();
//     }

//     private void OnEnable()
//     {
//         // Añadimos un pequeño retraso o verificación para evitar el Null al instanciar
//         Invoke(nameof(ValidarDisponibilidad), 0.05f);
//     }

//     public void ComprarDirecto()
//     {
//         Objeto objetoPadre = GetComponentInParent<Objeto>();
//         if (objetoPadre == null) return;

//         PlantillaObjeto datos = objetoPadre.GetDatos();
//         ShopManager tienda = objetoPadre.GetManager();

//         // Seguridad: Si no hay datos o tienda, no hacer nada
//         if (datos == null || tienda == null || progresoHabilidades == null) return;

//         if (datos.esPermanente && progresoHabilidades.EstaDesbloqueada(datos.nombreId))
//         {
//             FeedbackError();
//             return;
//         }

//         if (tienda.monedaJugador >= datos.precio)
//         {
//             tienda.ConfirmarVenta(datos, objetoPadre.GetCartaOriginal());

//             if (datos.esPermanente)
//             {
//                 if (!progresoHabilidades.habilidadesDesbloqueadas.Contains(datos.nombreId))
//                 {
//                     progresoHabilidades.habilidadesDesbloqueadas.Add(datos.nombreId);
//                 }
//             }
//             ValidarDisponibilidad();
//         }
//         else
//         {
//             FeedbackError();
//         }
//     }

//     public void ValidarDisponibilidad()
//     {
//         // 1. Buscamos el objeto padre (la carta o el inspector)
//         Objeto objetoPadre = GetComponentInParent<Objeto>();
        
//         // 2. VERIFICACIÓN DE SEGURIDAD (Esto evita el NullReferenceException)
//         if (objetoPadre == null || progresoHabilidades == null) return;

//         PlantillaObjeto datos = objetoPadre.GetDatos();
//         if (datos == null) return; // Si aún no tiene datos cargados, esperamos.

//         bool yaComprado = datos.esPermanente && progresoHabilidades.EstaDesbloqueada(datos.nombreId);

//         if (yaComprado)
//         {
//             if (textoBoton != null) textoBoton.text = "DESBLOQUEADO";
//             if (fondoTarjeta != null) fondoTarjeta.color = colorBloqueado;
//         }
//         else
//         {
//             if (textoBoton != null) textoBoton.text = "ADQUIRIR";
//             if (fondoTarjeta != null) fondoTarjeta.color = Color.white;
//         }
//     }

//     private void FeedbackError()
//     {
//         if (rectTransform != null)
//         {
//             rectTransform.DOComplete();
//             rectTransform.DOShakePosition(0.4f, 10f, 20);
//         }
//     }
// }


using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class buySK : MonoBehaviour
{
    [Header("Referencia al Progreso")]
    public WeaponSkillStatus progresoHabilidades; 
    
    [Header("Configuración de UI")]
    [SerializeField] private TextMeshProUGUI textoBoton; 
    [SerializeField] private Image fondoTarjeta;      
    [SerializeField] private Color colorBloqueado = new Color(0.5f, 0.5f, 0.5f, 1f);

    private Button miBoton;
    private RectTransform rectTransform;

    private void Awake()
    {
        miBoton = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Invoke(nameof(ValidarDisponibilidad), 0.05f);
    }

    public void ComprarDirecto()
    {
        Objeto objetoPadre = GetComponentInParent<Objeto>();
        if (objetoPadre == null) return;

        PlantillaObjeto datos = objetoPadre.GetDatos();
        ShopManager tienda = objetoPadre.GetManager();

        if (datos == null || tienda == null || progresoHabilidades == null) return;

        // Si ya está comprado, salimos
        if (datos.esPermanente && progresoHabilidades.EstaDesbloqueada(datos.nombreId))
        {
            FeedbackError();
            return;
        }

        if (tienda.monedaJugador >= datos.precio)
        {
            tienda.ConfirmarVenta(datos, objetoPadre.GetCartaOriginal());

            if (datos.esPermanente)
            {
                if (!progresoHabilidades.habilidadesDesbloqueadas.Contains(datos.nombreId))
                {
                    progresoHabilidades.habilidadesDesbloqueadas.Add(datos.nombreId);
                }
            }
            // Actualizamos el texto y color inmediatamente
            ValidarDisponibilidad(); 
        }
        else
        {
            FeedbackError();
        }
    }

    public void ValidarDisponibilidad()
    {
        Objeto objetoPadre = GetComponentInParent<Objeto>();
        if (objetoPadre == null || progresoHabilidades == null) return;

        PlantillaObjeto datos = objetoPadre.GetDatos();
        if (datos == null) return;

        bool yaComprado = datos.esPermanente && progresoHabilidades.EstaDesbloqueada(datos.nombreId);

        if (yaComprado)
        {
            if (textoBoton != null) textoBoton.text = "Desbloqueado"; // Cambia el texto
            if (fondoTarjeta != null) fondoTarjeta.color = colorBloqueado; // Cambia el color a gris
            if (miBoton != null) miBoton.interactable = false; // Opcional: bloquea el clic
        }
        else
        {
            if (textoBoton != null) textoBoton.text = "Desbloquear";
            if (fondoTarjeta != null) fondoTarjeta.color = Color.white;
            if (miBoton != null) miBoton.interactable = true;
        }
    }

    private void FeedbackError()
    {
        if (rectTransform != null)
        {
            rectTransform.DOComplete();
            rectTransform.DOShakePosition(0.4f, 10f, 20).SetUpdate(true);
        }
    }
}