// using UnityEngine;
// using System.Collections.Generic;
// using DG.Tweening;
// using System.Collections;

// public class AchievementManager : MonoBehaviour
// {
//     public static AchievementManager instance;
//     public List<AchievementSO> baseDeDatosLogros;

//     [Header("Referencias UI")]
//     public AchievementNotifyUI notifyUI;

//     private int totalEnemyKills = 0;

//     private void Awake()
//     {
//         if (instance == null) instance = this;
//         else Destroy(gameObject);

//         DontDestroyOnLoad(gameObject);

//         // Asegurarse que el popup empiece oculto
//         if (notifyUI != null)
//             notifyUI.gameObject.SetActive(false);

//         CargarProgreso();
//     }

//     public void CheckLogro(string id)
//     {
//         AchievementSO logro = baseDeDatosLogros.Find(l => l.id == id);
//         if (logro != null && !logro.desbloqueado)
//             Desbloquear(logro);
//     }

//     private void Desbloquear(AchievementSO logro)
//     {
//         logro.desbloqueado = true;
//         PlayerPrefs.SetInt("Logro_" + logro.id, 1);
//         PlayerPrefs.Save();

//         if (GameManager.instance != null && GameManager.instance.playerShipData != null)
//             GameManager.instance.playerShipData.dinero += logro.recompensaDinero;

//         MostrarNotificacion(logro);
//     }

//     private void MostrarNotificacion(AchievementSO logro)
// {
//     if (notifyUI == null)
//     {
//         Debug.LogError("AchievementManager: notifyUI no está asignado");
//         return;
//     }

//     notifyUI.PrepararDatos(logro);
//     notifyUI.gameObject.SetActive(true);
//     Invoke(nameof(EjecutarAnimacion), 0.05f);
// }

// private void EjecutarAnimacion()
// {
//     notifyUI.Animar();
// }
// // private void MostrarNotificacion(AchievementSO logro)
// // {
// //     if (notifyUI == null)
// //     {
// //         Debug.LogError("AchievementManager: notifyUI no está asignado");
// //         return;
// //     }

// //     StopAllCoroutines();
// //     StartCoroutine(MostrarPopup(logro));
// // }

// private IEnumerator MostrarPopup(AchievementSO logro)
// {
//     notifyUI.PrepararDatos(logro);
//     notifyUI.gameObject.SetActive(true);

//     yield return null; // espera un frame

//     notifyUI.Animar();
// }
//     // private void MostrarNotificacion(AchievementSO logro)
//     // {
//     //     if (notifyUI == null)
//     //     {
//     //         Debug.LogError("AchievementManager: notifyUI no está asignado en el Inspector");
//     //         return;
//     //     }

//     //     notifyUI.gameObject.SetActive(true);
//     //     notifyUI.Show(logro);
//     // }

//     private void CargarProgreso()
//     {
//         foreach (var logro in baseDeDatosLogros)
//             logro.desbloqueado = PlayerPrefs.GetInt("Logro_" + logro.id, 0) == 1;

//         totalEnemyKills = PlayerPrefs.GetInt("stat_enemy_kills", 0);
//     }

//     public void RegisterEnemyKill()
//     {
//         totalEnemyKills++;
//         PlayerPrefs.SetInt("stat_enemy_kills", totalEnemyKills);
//         PlayerPrefs.Save();

//         if (totalEnemyKills == 1)   CheckLogro("first_kill");
//         if (totalEnemyKills >= 10)  CheckLogro("kill_10");
//         if (totalEnemyKills >= 50)  CheckLogro("kill_50");
//         if (totalEnemyKills >= 100) CheckLogro("kill_100");
//     }
// }

using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public List<AchievementSO> baseDeDatosLogros;

    [Header("Referencias UI")]
    public AchievementNotifyUI notifyUI;

    private int totalEnemyKills = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (notifyUI != null)
            notifyUI.gameObject.SetActive(false);

        CargarProgreso();
    }

    public void CheckLogro(string id)
    {
        Debug.Log("CheckLogro llamado con id: " + id);
        AchievementSO logro = baseDeDatosLogros.Find(l => l.id == id);

        if (logro == null)
            Debug.LogError("Logro no encontrado en la base de datos: " + id);
        else if (logro.desbloqueado)
            Debug.LogWarning("Logro ya desbloqueado: " + id);
        else
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
        if (notifyUI == null)
        {
            Debug.LogError("AchievementManager: notifyUI no está asignado");
            return;
        }

        notifyUI.PrepararDatos(logro);
        notifyUI.gameObject.SetActive(true);
        Invoke(nameof(EjecutarAnimacion), 0.05f);
    }

    private void EjecutarAnimacion()
    {
        notifyUI.Animar();
    }

    private void CargarProgreso()
    {
        foreach (var logro in baseDeDatosLogros)
            logro.desbloqueado = PlayerPrefs.GetInt("Logro_" + logro.id, 0) == 1;

        totalEnemyKills = PlayerPrefs.GetInt("stat_enemy_kills", 0);
    }

    public void RegisterEnemyKill()
    {
        totalEnemyKills++;
        PlayerPrefs.SetInt("stat_enemy_kills", totalEnemyKills);
        PlayerPrefs.Save();

        Debug.Log("RegisterEnemyKill llamado. Total kills: " + totalEnemyKills);

        if (totalEnemyKills == 1)   CheckLogro("first_kill");
        if (totalEnemyKills >= 10)  CheckLogro("kill_10");
        if (totalEnemyKills >= 50)  CheckLogro("kill_50");
        if (totalEnemyKills >= 100) CheckLogro("kill_100");
    }
}