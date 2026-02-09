using UnityEngine;

[System.Serializable]
public class SpriteAnimation
{
    public Sprite[] frames;
    public float frameTime = 0.1f;
}

public class AnimationController : MonoBehaviour
{
    public SpriteRenderer sprite;

    [Header("Animations")]
    public SpriteAnimation[] animations;

    private int currentState = -1;
    private int frameIndex;
    private float timer;

    /// state = índice del array animations
    public void Play(int state, float speed)
    {
        if (state < 0 || state >= animations.Length)
            return;

        if (state != currentState)
        {
            currentState = state;
            frameIndex = 0;
            timer = 0f;
        }

        SpriteAnimation anim = animations[state];

        if (anim.frames == null || anim.frames.Length == 0)
            return;

        timer += Time.deltaTime * (speed > 0 ? speed : 1f);

        if (timer >= anim.frameTime)
        {
            timer = 0f;
            frameIndex++;
        }

        if (frameIndex >= anim.frames.Length)
            frameIndex = 0;

        sprite.sprite = anim.frames[frameIndex];
    }
}
