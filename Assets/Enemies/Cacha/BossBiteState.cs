using UnityEngine;
using DG.Tweening;

public class BossBiteState : State_Base
{
    public override void EnterState()
    {

        // Buscamos el Animator en el jefe y activamos el trigger
        controlledObject.GetComponent<Animator>().SetTrigger("Bite");


        Vector3 originalPos = controlledObject.transform.position;

        // 1. Ir al centro
        controlledObject.transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.InExpo)
            .OnComplete(() => {
                // 2. Aquí podrías activar una animación de mordida o un collider
                Debug.Log("¡BOSS MUERDE!");

                // 3. Volver a su posición de persecución
                controlledObject.transform.DOMove(originalPos, 1f).SetDelay(0.5f)
                    .OnComplete(() => state_machine.SetState<BossPursueState>());
            });
    }
}