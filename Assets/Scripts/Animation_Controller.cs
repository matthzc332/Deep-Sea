using UnityEngine;
using System.Collections;

// La clase debe estar fuera de Animation_Controller para que sea accesible
[System.Serializable]
public class SpriteAnimation
{
    public string name; // Útil para identificarla en el inspector
    public Sprite[] frames;
    public float frameTime = 0.1f;
    public bool loop = true;
}

public class Animation_Controller : MonoBehaviour
{
    public SpriteRenderer sprite;

    [Header("Animations")]
    public SpriteAnimation[] animations;

    private int currentState = -1;
    private Coroutine animationRoutine;
    private bool hasFinished = false; // Nueva variable de control

    public void Play(int state)
    {
        if (state < 0 || state >= animations.Length) return;
        if (state == currentState) return;

        currentState = state;
        hasFinished = false; // Reiniciamos el estado al empezar una nueva

        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        animationRoutine = StartCoroutine(AnimateRoutine(animations[state]));
    }

    private IEnumerator AnimateRoutine(SpriteAnimation anim)
    {
        int frameIndex = 0;
        if (anim.frames == null || anim.frames.Length == 0) yield break;

        while (true)
        {
            sprite.sprite = anim.frames[frameIndex];
            yield return new WaitForSeconds(anim.frameTime);

            frameIndex++;

            if (frameIndex >= anim.frames.Length)
            {
                if (anim.loop)
                {
                    frameIndex = 0;
                }
                else
                {
                    // La animación terminó realmente aquí
                    sprite.sprite = anim.frames[anim.frames.Length - 1];
                    hasFinished = true;
                    animationRoutine = null;
                    yield break;
                }
            }
        }
    }

    public bool IsFinished()
    {
        // Ahora es mucho más robusto
        return hasFinished;
    }
}