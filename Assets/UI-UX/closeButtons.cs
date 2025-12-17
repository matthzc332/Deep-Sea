using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Necesario para List

public class closeButtons : MonoBehaviour
{
    [Header("Objetos a Desactivar")]
    public List<GameObject> objetosADesactivar = new List<GameObject>();
    
    [Header("Configuración")]
    public bool desactivarEsteObjetoTambien = false;
    
    private Button boton;
    
    void Start()
    {
        boton = GetComponent<Button>();
        
        if (boton != null)
        {
            boton.onClick.AddListener(DesactivarObjetos);
        }
        else
        {
            Debug.LogWarning($"No se encontró Button en {gameObject.name}");
        }
    }
    
    void Update()
    {
        // Atajo de teclado opcional
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DesactivarObjetos();
        }
    }
    
    public void DesactivarObjetos()
    {
        if (objetosADesactivar.Count == 0)
        {
            Debug.LogWarning($"Lista vacía en {gameObject.name}");
            return;
        }
        
        // Desactivar todos los objetos
        foreach (GameObject obj in objetosADesactivar)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        
        // Limpiar la lista después de desactivar (opcional)
        // objetosADesactivar.Clear();
        
        if (desactivarEsteObjetoTambien)
        {
            gameObject.SetActive(false);
        }
    }
    
    // Métodos para manipular la lista
    public void AgregarObjeto(GameObject obj)
    {
        if (!objetosADesactivar.Contains(obj))
        {
            objetosADesactivar.Add(obj);
        }
    }
    
    public void RemoverObjeto(GameObject obj)
    {
        objetosADesactivar.Remove(obj);
    }
    
    public void LimpiarLista()
    {
        objetosADesactivar.Clear();
    }
}