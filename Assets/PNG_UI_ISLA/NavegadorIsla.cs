using UnityEngine;
using DG.Tweening; // Para el movimiento suave

public class NavegadorIsla : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform objetoQueSeMueve; // Aquí irá "Scene1"
    
    [Header("Posiciones X")]
    public float izquierda;
    public float centro = 0;
    public float derecha;

    [Header("Paneles")]
    public GameObject panelMarineros;
    public GameObject panelHabilidades;
    public GameObject panelBarco;

    public void IrAMarineros() {
        if(objetoQueSeMueve != null) objetoQueSeMueve.DOAnchorPosX(izquierda, 0.5f);
        Alternar(panelMarineros);
    }

    public void IrACentro() {
        if(objetoQueSeMueve != null) objetoQueSeMueve.DOAnchorPosX(centro, 0.5f);
        Alternar(panelHabilidades);
    }

    public void IrABarco() {
        if(objetoQueSeMueve != null) objetoQueSeMueve.DOAnchorPosX(derecha, 0.5f);
        Alternar(panelBarco);
    }

    private void Alternar(GameObject activo) {
        if(panelMarineros) panelMarineros.SetActive(panelMarineros == activo);
        if(panelHabilidades) panelHabilidades.SetActive(panelHabilidades == activo);
        if(panelBarco) panelBarco.SetActive(panelBarco == activo);
    }

    // Esta función es la que usarán tus botones de la "X"
    public void CerrarTodo()
    {
        if(panelMarineros) panelMarineros.SetActive(false);
        if(panelHabilidades) panelHabilidades.SetActive(false);
        if(panelBarco) panelBarco.SetActive(false);
        
        Debug.Log("Todos los paneles se han cerrado.");
    }
}