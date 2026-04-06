using Unity.VisualScripting;
using UnityEngine;

public class MoverX : State_Base
{
    private Cofre cofre;
    private Animation_Controller anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void EnterState()
    {
        cofre = controlledObject.GetComponent<Cofre>();
        anim = controlledObject.GetComponent<Animation_Controller>();
        if (anim != null) anim.Play(0);
    }

    // Update is called once per frame
    public override void UpdateState()
    {

        cofre.transform.Translate(Vector2.right * cofre.getSpeed() * Time.deltaTime);
        
        if (transform.position.x > 15f || cofre.HP == 0)
        {
            ExitState("Destroy");
        }
    }

    public override void ExitState(string nextState)
    {
        if (nextState == "Destroy")
        {
            state_machine.SetState<Destroy>();
        }
    }
}
