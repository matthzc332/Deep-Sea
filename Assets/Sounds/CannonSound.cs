using UnityEngine;

public class CannonSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoDisparo;


    public void disparo()
    {
        audioSource.PlayOneShot(sonidoDisparo);
    }
}

