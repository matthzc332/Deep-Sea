// using UnityEngine;
// using System.Collections.Generic;
//FUNCIONA
// public class AchievementManager : MonoBehaviour
// {
//     public static AchievementManager instance;
//     public List<AchievementSO> baseDeDatosLogros;

//     [Header("Referencias UI")]
//     public AchievementNotifyUI notifyUI;

//     private int totalEnemyKills = 0;

//     private void Awake()
// {
//     if (instance == null)
//     {
//         instance = this;
//         transform.SetParent(null); // Se asegura de estar en la raíz para DontDestroyOnLoad
//         DontDestroyOnLoad(gameObject);
//         CargarProgreso();
//     }
//     else if (instance != this)
//     {
//         Debug.Log("AchievementManager duplicado detectado, destruyendo el nuevo.");
//         Destroy(gameObject);
//         return;
//     }

//     // Solo desactivar la UI si NO es el mismo objeto que este script
//     if (notifyUI != null && notifyUI.gameObject != this.gameObject)
//     {
//         notifyUI.gameObject.SetActive(false);
//     }
// }

//     public void CheckLogro(string id)
//     {
//         AchievementSO logro = baseDeDatosLogros.Find(l => l.id == id);
//         if (logro != null && !logro.desbloqueado)
//         {
//             Desbloquear(logro);
//         }
//     }

//     private void Desbloquear(AchievementSO logro)
//     {
//         logro.desbloqueado = true;
//         PlayerPrefs.SetInt("Logro_" + logro.id, 1);
//         PlayerPrefs.Save();

//         // Recompensa económica
//         if (GameManager.instance != null && GameManager.instance.playerShipData != null)
//             GameManager.instance.playerShipData.dinero += logro.recompensaDinero;

//         MostrarNotificacion(logro);
//     }

//     private void MostrarNotificacion(AchievementSO logro)
//     {
//         if (notifyUI == null)
//         {
//             // Intenta encontrarlo en los hijos por si se perdió la referencia
//             notifyUI = GetComponentInChildren<AchievementNotifyUI>(true);
//         }

//         if (notifyUI != null)
//         {
//             notifyUI.Show(logro);
//         }
//         else
//         {
//             Debug.LogError("AchievementManager: No hay AchievementNotifyUI asignado.");
//         }
//     }
// private void CargarProgreso()
// {
//     foreach (var logro in baseDeDatosLogros)
//     {
//         // Forzamos el estado inicial basado en los datos guardados, 
//         // eliminando cualquier "tilde" que haya quedado puesto en el Editor.
//         int estadoGuardado = PlayerPrefs.GetInt("Logro_" + logro.id, 0);
//         logro.desbloqueado = (estadoGuardado == 1);
//     }

//     totalEnemyKills = PlayerPrefs.GetInt("stat_enemy_kills", 0);
// }
//     // private void CargarProgreso()
//     // {
//     //     foreach (var logro in baseDeDatosLogros)
//     //         logro.desbloqueado = PlayerPrefs.GetInt("Logro_" + logro.id, 0) == 1;

//     //     totalEnemyKills = PlayerPrefs.GetInt("stat_enemy_kills", 0);
//     // }

//     public void RegisterEnemyKill()
//     {
//         totalEnemyKills++;
//         PlayerPrefs.SetInt("stat_enemy_kills", totalEnemyKills);
//         PlayerPrefs.Save();

//         // Verificación de hitos
//         if (totalEnemyKills == 1)   CheckLogro("first_kill");
//         if (totalEnemyKills == 10)  CheckLogro("kill_10");
//         if (totalEnemyKills == 50)  CheckLogro("kill_50");
//         if (totalEnemyKills == 100) CheckLogro("kill_100");
//     }
// }

// // using UnityEngine;
// // using System.Collections.Generic;

// // public class AchievementManager : MonoBehaviour
// // {
// //     public static AchievementManager instance;
// //     public List<AchievementSO> baseDeDatosLogros;

// //     [Header("Referencias UI")]
// //     public AchievementNotifyUI notifyUI;

// //     private int totalEnemyKills = 0;

// //     private void Awake()
// //     {
// //         // Si ya existe una instancia válida, esta se destruye y listo
// //         if (instance != null && instance != this)
// //         {
// //             Destroy(gameObject);
// //             return;
// //         }

// //         instance = this;
// //         DontDestroyOnLoad(gameObject);

// //         if (notifyUI != null)
// //             notifyUI.gameObject.SetActive(false);

// //         CargarProgreso();
// //     }

// //     public void CheckLogro(string id)
// //     {
// //         AchievementSO logro = baseDeDatosLogros.Find(l => l.id == id);

// //         if (logro == null)
// //         {
// //             Debug.LogWarning($"[Logros] ID no encontrado: {id}");
// //             return;
// //         }

// //         if (!logro.desbloqueado)
// //             Desbloquear(logro);
// //     }

// //     private void Desbloquear(AchievementSO logro)
// //     {
// //         logro.desbloqueado = true;
// //         PlayerPrefs.SetInt("Logro_" + logro.id, 1);
// //         PlayerPrefs.Save();

// //         if (GameManager.instance != null && GameManager.instance.playerShipData != null)
// //             GameManager.instance.playerShipData.dinero += logro.recompensaDinero;

// //         Debug.Log($"[Logros] Desbloqueado: {logro.titulo}");
// //         MostrarNotificacion(logro);
// //     }

// //     private void MostrarNotificacion(AchievementSO logro)
// //     {
// //         if (notifyUI == null)
// //         {
// //             Debug.LogError("[Logros] notifyUI no asignado en el Inspector");
// //             return;
// //         }

// //         notifyUI.gameObject.SetActive(true);
// //         notifyUI.Show(logro);
// //     }

// //     private void CargarProgreso()
// //     {
// //         foreach (var logro in baseDeDatosLogros)
// //             logro.desbloqueado = PlayerPrefs.GetInt("Logro_" + logro.id, 0) == 1;

// //         totalEnemyKills = PlayerPrefs.GetInt("stat_enemy_kills", 0);
// //         Debug.Log($"[Logros] Progreso cargado. Kills: {totalEnemyKills}");
// //     }

// //     public void RegisterEnemyKill()
// //     {
// //         totalEnemyKills++;
// //         PlayerPrefs.SetInt("stat_enemy_kills", totalEnemyKills);
// //         PlayerPrefs.Save();

// //         Debug.Log($"[Logros] Kill registrado. Total: {totalEnemyKills}");

// //         if (totalEnemyKills == 1)   CheckLogro("first_kill");
// //         if (totalEnemyKills >= 10)  CheckLogro("kill_10");
// //         if (totalEnemyKills >= 50)  CheckLogro("kill_50");
// //         if (totalEnemyKills >= 100) CheckLogro("kill_100");
// //     }
// // }

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public List<AchievementSO> baseDeDatosLogros;

    [Header("Referencias UI")]
    public AchievementNotifyUI notifyUI;

    private int totalEnemyKills = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            if (notifyUI != null)
                notifyUI.gameObject.SetActive(false);

            CargarProgreso();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += RegisterEnemyKill;
        Debug.Log("AchievementManager: suscrito a OnEnemyKilled");
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= RegisterEnemyKill;
    }

    public void CheckLogro(string id)
    {
        AchievementSO logro = baseDeDatosLogros.Find(l => l.id == id);
        if (logro != null && !logro.desbloqueado)
            Desbloquear(logro);
    }

    private void Desbloquear(AchievementSO logro)
    {
        logro.desbloqueado = true;
        PlayerPrefs.SetInt("Logro_" + logro.id, 1);
        PlayerPrefs.Save();

        if (GameManager.instance != null && GameManager.instance.playerShipData != null)
            GameManager.instance.playerShipData.dinero += logro.recompensaDinero;

        MostrarNotificacion(logro);
    }

    private void MostrarNotificacion(AchievementSO logro)
    {
        Debug.Log("AchievementManager: MostrarNotificacion llamado para " + logro.titulo);
         Debug.Log("MostrarNotificacion llamado. notifyUI es null: " + (notifyUI == null));
        if (notifyUI == null)
        {
            Debug.LogError("AchievementManager: notifyUI no está asignado en el Inspector");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(MostrarPopup(logro));
    }

    private IEnumerator MostrarPopup(AchievementSO logro)
    {
        notifyUI.gameObject.SetActive(true);
        yield return null;
        notifyUI.Show(logro);
    }

    private void RegisterEnemyKill()
    {
        Debug.Log("AchievementManager: RegisterEnemyKill llamado, kills = " + totalEnemyKills);

        totalEnemyKills++;
        PlayerPrefs.SetInt("stat_enemy_kills", totalEnemyKills);
        PlayerPrefs.Save();

        if (totalEnemyKills == 1) CheckLogro("first_kill");
        // if (totalEnemyKills == 10)  CheckLogro("kill_10");
        // if (totalEnemyKills == 50)  CheckLogro("kill_50");
        // if (totalEnemyKills == 100) CheckLogro("kill_100");
    }

    private void CargarProgreso()
    {
        foreach (var logro in baseDeDatosLogros)
            logro.desbloqueado = PlayerPrefs.GetInt("Logro_" + logro.id, 0) == 1;

        totalEnemyKills = PlayerPrefs.GetInt("stat_enemy_kills", 0);
    }

#if UNITY_EDITOR
    [ContextMenu("Resetear todos los logros (solo Editor)")]
    public void ResetearLogrosEditor()
    {
        foreach (var logro in baseDeDatosLogros)
        {
            logro.desbloqueado = false;
            PlayerPrefs.DeleteKey("Logro_" + logro.id);
        }
        PlayerPrefs.DeleteKey("stat_enemy_kills");
        totalEnemyKills = 0;
        PlayerPrefs.Save();
        Debug.Log("Logros reseteados.");
    }
#endif
}