using UnityEngine;
using System;

public class EnemyDeathNotifier : MonoBehaviour
{
    public Action OnDeath;

    private void OnDestroy()
    {
        // Si el objeto se destruye (muere), avisamos al Spawner
        OnDeath?.Invoke();
    }
}