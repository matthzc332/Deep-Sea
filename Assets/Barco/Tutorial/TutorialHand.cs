using UnityEngine;

public class TutorialHand : MonoBehaviour
{
    [SerializeField] Animator anim;
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
