using UnityEngine;
using System;
using Unity.VisualScripting;

public class TutorialEnemy : Entity
{
    private Transform trf;
    public Vector3 inicio;
    public Vector3 destino;
    public float duracion = 2f;
    public float tiempo = 1;
    public bool isPlaying = false;
    public int counter = 1;

    public TutorialManager manager;

    private void Start()
    {
        trf = gameObject.GetComponent<Transform>();
        inicio = transform.position;
    }


    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if (tiempo < duracion)
            {
                tiempo += Time.deltaTime;
                float t = tiempo / duracion;
                transform.position = Vector3.Lerp(inicio, destino, t);
            }
        }

        if (transform.position == destino && counter != 0)
        {
            counter -= 1;
            manager.StartShootTutorial(transform.position);
        }
    }


}
