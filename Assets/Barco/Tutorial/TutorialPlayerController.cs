using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TutorialPlayerController : Cannon
{
    [Header("Refs")]
    public GameObject bulletPrefab;
    public TutorialManager tutorialManager;

    [Header("Tuning")]
    public float shootCooldown = 0.3f;   
    public int timeShoot = 2;            
    public float multiShotRadiusPx = 80f;  

    private Camera cam;
    private float lastShootTime = -999f;

    static readonly List<RaycastResult> _uiHits = new List<RaycastResult>();

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }

    void OnDisable()
    {
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        cam = Camera.main;
        speed = 5f;
        power = 1;

        if (cam == null)
            Debug.LogError("No hay c�mara con tag MainCamera en la escena.");
        if (bulletPrefab == null)
            Debug.LogError("bulletPrefab no asignado.");
    }

    void Update()
    {
        // TOUCH
        foreach (var t in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (t.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Vector2 pos = t.screenPosition;
                if (!IsOverUI(pos))
                    TryShoot(pos);
                return;
            }
        }

        // MOUSE (Editor/PC)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 pos = Mouse.current.position.ReadValue();

            // si ten�s una variable 'mitadPantallaY' en otra parte, reemplaz� por ella:
            if (!IsOverUI(pos))
                TryShoot(pos);
        }
    }

    private void TryShoot(Vector2 screenPos)
    {
        if (Time.time - lastShootTime < shootCooldown) return;
        if (cam == null || bulletPrefab == null) return;
        if (ammunition <= 0) return;

        DispararHaciaPantalla(screenPos);

        // *** MUNICI?N: se descuenta 1 por disparo (igual que en tu versi�n) ***
        ammunition -= 1;
        if (ammunition < 0) ammunition = 0;

        lastShootTime = Time.time;
        Debug.Log($"Disparo! Ammo restante: {ammunition}");
    }

    private void DispararHaciaPantalla(Vector2 screenPos)
    {
        Time.timeScale = 1f;
        tutorialManager.AfterShoot();

        float distanceZ = Mathf.Abs(cam.transform.position.z - transform.position.z);
        // bala central
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distanceZ));
        SpawnBullet(worldPos);

        // balas extra seg�n MultipleShoot usando offsets en p�xeles (patr�n circular)
        var extraOffsets = GetExtraScreenOffsets(MultipleShoot, multiShotRadiusPx);
        for (int i = 0; i < extraOffsets.Count; i++)
        {
            Vector2 extraScreen = screenPos + extraOffsets[i];
            Vector3 extraWorld = cam.ScreenToWorldPoint(new Vector3(extraScreen.x, extraScreen.y, distanceZ));
            SpawnBullet(extraWorld);
        }
    }

    private void SpawnBullet(Vector3 targetWorld)
    {
        var b = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        b.Initialize(targetWorld, initialSpeed: 18f, power: 2); 
        
    }

    private List<Vector2> GetExtraScreenOffsets(int count, float radiusPx)
    {
        var list = new List<Vector2>();
        if (count <= 0 || radiusPx <= 0f) return list;

        for (int i = 0; i < count; i++)
        {
            float rad = (360f / count) * i * Mathf.Deg2Rad;
            list.Add(new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radiusPx);
        }
        return list;
        // Si quer�s �fan� en lugar de c�rculo:
        // - triple: �ngulos [-10�, 0�, 10�] con offsets en X/Y calculados por seno/coseno
        // - doble horizontal: {(-radiusPx,0), (radiusPx,0)}
        // - cruz: {(�r,0),(0,�r)}
    }

    private bool IsOverUI(Vector2 screenPos)
    {
        if (EventSystem.current == null) return false;

        var data = new PointerEventData(EventSystem.current) { position = screenPos };
        _uiHits.Clear();
        EventSystem.current.RaycastAll(data, _uiHits);

        // Solo consideramos UI de Canvas (GraphicRaycaster). Ignoramos PhysicsRaycaster/2D
        for (int i = 0; i < _uiHits.Count; i++)
            if (_uiHits[i].module is GraphicRaycaster)
                return true;

        return false;
    }
}