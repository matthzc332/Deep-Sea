using UnityEngine;
using UnityEngine.UI;
public class HP : MonoBehaviour
{
    public Ship Player;

    private Image image;

    public Sprite spr5hp;
    public Sprite spr4hp;
    public Sprite spr3hp;
    public Sprite spr2hp;
    public Sprite spr1hp;

    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.vida == 6)
        {
            return;
        }
        else if (Player.vida == 5)
        {
            image.sprite = spr5hp;
        }
        else if (Player.vida == 4)
        {
            image.sprite = spr4hp;
        }
        else if (Player.vida == 3)
        {
            image.sprite = spr3hp;
        }
        else if (Player.vida == 2)
        {
            image.sprite = spr2hp;
        }
        else if (Player.vida == 1)
        {
            image.sprite = spr1hp;
        }

    }
}
