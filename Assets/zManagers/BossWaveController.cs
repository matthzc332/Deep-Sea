using UnityEngine;
using UnityEngine.UI;

public class BossWaveController : MonoBehaviour
{
    [Header("Referencias de UI")]
    public GameObject healthBarPanel;
    public Slider hpSlider;

    [Header("Configuración")]
    public BossController bossScript;
    public GameManager gameManager;

    // Variable para saber si ya encontramos al boss y evitar buscarlo todo el tiempo
    private bool bossFound = false;

    private void OnEnable()
    {
        // Al iniciar la oleada, reseteamos el estado
        bossFound = false;

        // Ocultamos la barra al principio hasta que aparezca el jefe
        if (healthBarPanel != null)
            healthBarPanel.SetActive(false);

        // Si el boss ya estaba asignado manualmente, lo marcamos como encontrado
        if (bossScript != null)
        {
            SetupBossUI();
        }
    }

    private void Update()
    {
        // CASO 1: Aún no hemos encontrado al Boss
        if (!bossFound)
        {
            SearchForBoss();
            return; // No hacemos nada más hasta encontrarlo
        }

        // CASO 2: Ya tenemos Boss, pero ha desaparecido (ej. destruido antes de tiempo)
        if (bossScript == null)
        {
            // Opcional: Si desaparece el objeto, asumimos que murió
            OnBossDefeated();
            return;
        }

        // CASO 3: El Boss existe, actualizamos su vida
        hpSlider.value = bossScript.HP;

        if (bossScript.HP <= 0)
        {
            OnBossDefeated();
        }
    }

    private void SearchForBoss()
    {
        // Intentamos buscar el script en la escena
        bossScript = FindAnyObjectByType<BossController>();

        // Si lo encontramos AHORA, configuramos todo
        if (bossScript != null)
        {
            SetupBossUI();
        }
    }

    private void SetupBossUI()
    {
        bossFound = true;
        Debug.Log("Boss encontrado. Activando barra de vida.");

        if (healthBarPanel != null)
            healthBarPanel.SetActive(true);

        if (hpSlider != null)
        {
            hpSlider.maxValue = bossScript.HP;
            hpSlider.value = bossScript.HP;
        }
    }

    private void OnBossDefeated()
    {
        Debug.Log("El Boss ha sido derrotado.");

        // Aquí tu lógica de victoria (cambiar escena, desactivar barra, etc.)
        if (healthBarPanel != null)
            healthBarPanel.SetActive(false);
        gameManager.EndWave();
        // Desactivamos este script para que deje de procesar
        this.enabled = false;
    }
}