using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public List<AchievementSO> baseDeDatosLogros;

    [Header("Referencias UI")]
    public GameObject panelPopup; // El objeto visual que aparece
    public TMPro.TMP_Text textoTitulo;
    public UnityEngine.UI.Image imagenIcono;

    private int totalEnemyKills = 0;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        CargarProgreso();
    }

    public void CheckLogro(string id)
    {
        AchievementSO logro = baseDeDatosLogros.Find(l => l.id == id);
        if (logro != null && !logro.desbloqueado)
        {
            Desbloquear(logro);
        }
    }

    private void Desbloquear(AchievementSO logro)
    {
        logro.desbloqueado = true;
        PlayerPrefs.SetInt("Logro_" + logro.id, 1);
        PlayerPrefs.Save();

        // Dar recompensa usando tu ShipData
        if (GameManager.instance != null && GameManager.instance.playerShipData != null)
        {
            GameManager.instance.playerShipData.dinero += logro.recompensaDinero;
        }

        MostrarNotificacion(logro);
    }

    private void MostrarNotificacion(AchievementSO logro)
    {
        textoTitulo.text = logro.titulo;
        imagenIcono.sprite = logro.icono;
        
        // Aquí usas DOTween (que ya tienes en el proyecto) para animar
        panelPopup.SetActive(true);
        panelPopup.transform.localScale = Vector3.zero;
        panelPopup.transform.DOScale(1, 0.5f).OnComplete(() => {
            Invoke("OcultarPanel", 3f);
        });
    }

    private void OcultarPanel() => panelPopup.SetActive(false);

    private void CargarProgreso()
    {
        foreach (var logro in baseDeDatosLogros)
        {
            logro.desbloqueado = PlayerPrefs.GetInt("Logro_" + logro.id, 0) == 1;
        }
        totalEnemyKills = PlayerPrefs.GetInt("stat_enemy_kills", 0);
    }
    public void RegisterEnemyKill()
{
    totalEnemyKills++;
    PlayerPrefs.SetInt("stat_enemy_kills", totalEnemyKills);
    PlayerPrefs.Save();

    CheckLogro("first_kill");

    if (totalEnemyKills >= 1)  CheckLogro("kill_1");
    if (totalEnemyKills >= 50)  CheckLogro("kill_50");
    if (totalEnemyKills >= 100) CheckLogro("kill_100");
}
}