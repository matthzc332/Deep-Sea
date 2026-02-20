using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public SpriteRenderer sprite;

    [Header("Animations")]
    public Sprite[] volando;
    public float volandoFrameTime = 0.0f;

    public Sprite[] picada;
    public float picadaFrameTime = 0.0f;

    public Sprite[] explotando;
    public float explotandoFrameTime = 0.0f;

    private int currentState = -1;
    private int frameIndex;
    private float timer;

    // 0 = volando, 1 = picada, 2 = explotando
    public void Play(int state, float speed)
    {
        if (state != currentState)
        {
            currentState = state;
            frameIndex = 0;
            timer = 0f;
        }

        Sprite[] anim = GetAnimation(state);
        float frameTime = GetFrameTime(state);

        if (anim == null || anim.Length == 0)
            return;

        timer += Time.deltaTime * (speed > 0 ? speed : 1f);

        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex++;
        }

        if (frameIndex >= anim.Length)
            frameIndex = 0;

        sprite.sprite = anim[frameIndex];
    }

    Sprite[] GetAnimation(int state)
    {
        if (state == 0) return volando;
        if (state == 1) return picada;
        if (state == 2) return explotando;
        return null;
    }

    float GetFrameTime(int state)
    {
        if (state == 0) return volandoFrameTime;
        if (state == 1) return picadaFrameTime;
        if (state == 2) return explotandoFrameTime;
        return 0.1f;
    }
}
