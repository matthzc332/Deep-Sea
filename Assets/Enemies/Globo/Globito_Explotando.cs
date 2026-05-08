public class Globo_Explotando : State_Base
{
    private Animation_Controller anim;
    private bool initialized = false;

    public override void EnterState()
    {
        anim = controlledObject.GetComponent<Animation_Controller>();
        initialized = true;
        // Ya no destruimos aquí con un tiempo fijo
    }

    public override void UpdateState()
    {
        if (anim == null) return;

        // Ejecutar animación de explosión (índice 1)
        anim.Play(1);

        // Si la animación terminó, destruimos el objeto
        if (anim.IsFinished())
        {
            Destroy(controlledObject);
        }
    }
}