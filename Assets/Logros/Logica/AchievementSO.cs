using UnityEngine;

[CreateAssetMenu(fileName = "NuevoLogro", menuName = "DeepSea/Logro")]
public class AchievementSO : ScriptableObject
{
    // IDs sugeridos: "first_kill", "kill_10", "kill_50", "kill_100"
    public string id;
    public string titulo;
    [TextArea] public string descripcion;
    public Sprite icono;
    public bool desbloqueado;

    [Header("Recompensas")]
    public int recompensaDinero;
    public bool desbloqueaItem;
    public string idItemADesbloquear;
}