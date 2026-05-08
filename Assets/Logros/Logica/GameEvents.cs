using System;

public static class GameEvents
{
    public static event Action OnEnemyKilled;
    public static event Action OnPlayerDied;

    public static void EnemyKilled() => OnEnemyKilled?.Invoke();
    public static void PlayerDied()  => OnPlayerDied?.Invoke();
}