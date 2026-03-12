using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Referencia al Oleaje")]
    // Arrastra aquí el objeto padre que tiene el script GlobalSeaWave
    public GlobalSeaWave seaWaveControl;

    [Header("Ajustes de Flotación")]
    public float heightOffset = 0.5f; // Ajuste manual para que no se hunda
    [Range(0, 1)]
    public float smoothness = 0.1f;   // Suavizado de movimiento

    private float currentY;

    void Update()
    {
        if (seaWaveControl == null) return;

        // 1. Usamos la misma fórmula exacta del script de oleaje
        // Necesitamos la posición X global del objeto
        float worldX = transform.position.x;
        float time = Time.time * seaWaveControl.speed;

        // Calculamos la altura de la ola en este punto X exacto
        float waveHeight = Mathf.Sin(worldX * seaWaveControl.frequency + time) * seaWaveControl.amplitude;

        // 2. Calculamos la posición objetivo
        // (Asumimos que el mar está en Y = 0, si no, sumamos seaWaveControl.transform.position.y)
        float targetY = seaWaveControl.transform.position.y + waveHeight + heightOffset;

        // 3. Aplicamos el movimiento (con un pequeño suavizado para que no sea rígido)
        Vector3 newPos = transform.position;
        newPos.y = Mathf.Lerp(newPos.y, targetY, smoothness);
        transform.position = newPos;

        // 4. Opcional: Rotación para que el objeto se incline con la ola
        float nextWorldX = worldX + 0.1f;
        float nextWaveHeight = Mathf.Sin(nextWorldX * seaWaveControl.frequency + time) * seaWaveControl.amplitude;
        float angle = Mathf.Atan2(nextWaveHeight - waveHeight, 0.1f) * Mathf.Rad2Deg;

        // Suavizamos la rotación para que el barco cabecee
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothness);
    }
}