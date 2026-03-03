using UnityEngine;

public class ShipSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoAtacar;
    public AudioClip sonidoDanio;
    public AudioClip sonidoCofre;


    public void Atacar()
    {
        audioSource.PlayOneShot(sonidoAtacar);
    }
    public void Danio()
    {
        audioSource.PlayOneShot(sonidoDanio);
    }
    public void Cofre()
    {
        audioSource.PlayOneShot(sonidoCofre);
    }
}

