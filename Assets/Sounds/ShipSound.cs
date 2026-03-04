using UnityEngine;

public class ShipSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoAtacar;
    public AudioClip sonidoDaño;
    public AudioClip sonidoCofre;
    public AudioClip sonidoTimon;

    public void Atacar()
    {
        audioSource.PlayOneShot(sonidoAtacar);
    }
    public void Danio()
    {
        audioSource.PlayOneShot(sonidoDaño);
    }
    public void Cofre()
    {
        audioSource.PlayOneShot(sonidoCofre);
    }
    public void Timon()
    {
        audioSource.PlayOneShot(sonidoTimon);
    }
}

