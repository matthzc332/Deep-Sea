using UnityEngine;
using System.Collections;

public class BossBombState : State_Base
{
    public GameObject bombPrefab;
    public int bombCount = 4;
    public float throwInterval = 0.3f;

    public override void EnterState()
    {
        StartCoroutine(ThrowBombs());
        transform.rotation = Quaternion.Euler(0, 0, -90f);
    }

    IEnumerator ThrowBombs()
    {
        for (int i = 0; i < bombCount; i++)
        {
            GameObject bomb = Instantiate(bombPrefab, controlledObject.transform.position, Quaternion.identity);

            // L�gica simple para que la bomba se mueva a la derecha
            Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
            if (rb) rb.AddForce(new Vector2(10f, Random.Range(-2f, 2f)), ForceMode2D.Impulse);

            yield return new WaitForSeconds(throwInterval);
        }

        state_machine.SetState<BossPursueState>();
    }
}