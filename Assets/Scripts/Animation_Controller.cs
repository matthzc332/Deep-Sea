using UnityEngine;

[System.Serializable]
public class SpriteAnimation
{
    public Sprite[] frames;
    public float frameTime = 0.1f;
}

public class Animation_Controller : MonoBehaviour
{
    public SpriteRenderer sprite;

    [Header("Animations")]
    public SpriteAnimation[] animations;

    private int currentState = -1;
    private int frameIndex;
    private float timer;

    /// state = índice del array animations
    /// Ahora solo pide el índice del estado, la velocidad se maneja internamente
    public void Play(int state)
    {
        // 1. Validación de índice
        if (state < 0 || state >= animations.Length)
            return;

        // 2. Cambio de estado: Reseteamos si es una animación nueva
        if (state != currentState)
        {
            currentState = state;
            frameIndex = 0;
            timer = 0f;
        }

        SpriteAnimation anim = animations[state];

        // 3. Validación de frames
        if (anim.frames == null || anim.frames.Length == 0)
            return;

        // 4. Avance del temporizador
        // Simplemente usamos Time.deltaTime. 
        // El "paso" lo dictará el frameTime de la animación.
        timer += Time.deltaTime;

        if (timer >= anim.frameTime)
        {
            timer = 0f;
            frameIndex++;

            // 5. Bucle (Loop)
            if (frameIndex >= anim.frames.Length)
            {
                frameIndex = 0;
            }

            // Solo actualizamos el sprite cuando realmente cambia el frame
            sprite.sprite = anim.frames[frameIndex];
        }
    }
}
