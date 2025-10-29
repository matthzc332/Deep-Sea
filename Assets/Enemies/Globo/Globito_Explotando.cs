using UnityEngine;

public class Globo_Explotando : State_Base
{

    public override void EnterState()
    {
        Debug.Log("�El globo explota!");

        Destroy(controlledObject); // destruir el globo
    }
}
