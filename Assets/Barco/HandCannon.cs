using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class HandCannon : Cannon
{
    [Header("Refs")]
    public GameObject bulletPrefab;

    [Header("Tuning")]
    public float shootCooldown = 0.3f;
    public int timeShoot = 2;
    public float multiShotRadiusPx = 80f;

    private Camera cam;
    private float lastShootTime = -999f;

    static readonly List<RaycastResult> _uiHits = new List<RaycastResult>();

    // =============================
    // Unity
    // =============================

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

        // stats base
        speed = 5f;
        power = 1;
        ammunition = 9999;

        if (cam == null)
            Debug.LogError("No hay cámara con tag MainCamera.");
        if (bulletPrefab == null)
            Debug.LogError("bulletPrefab no asignado.");
    }

    void Update()
    {
        // Touch
        foreach (var t in Touch.activeTouches)
        {
            if (t.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Vector2 pos = t.screenPosition;
                if (!IsOverUI(pos))
                    TryShoot(pos);
                return;
            }
        }

        // Mouse
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            if (!IsOverUI(pos))
                TryShoot(pos);
        }
    }

    // =============================
    // Shooting
    // =============================

    private void TryShoot(Vector2 screenPos)
    {
        if (Time.time - lastShootTime < shootCooldown) return;
        if (cam == null || bulletPrefab == null) return;
        if (ammunition <= 0) return;

        DispararHaciaPantalla(screenPos);

        ammunition--;
        lastShootTime = Time.time;
    }

    private void DispararHaciaPantalla(Vector2 screenPos)
    {
        float distanceZ = cam.orthographic
            ? cam.nearClipPlane
            : Mathf.Abs(cam.transform.position.z - transform.position.z);

        if (distanceZ <= 0f) distanceZ = 1f;

        // Bala central
        Vector3 worldPos = cam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, distanceZ)
        );

        SpawnBullet(worldPos);

        // Balas extra (MULTISHOT PERMANENTE)
        var offsets = GetExtraScreenOffsets(multiShotLevel, multiShotRadiusPx);
        foreach (var o in offsets)
        {
            Vector2 extraScreen = screenPos + o;
            Vector3 extraWorld = cam.ScreenToWorldPoint(
                new Vector3(extraScreen.x, extraScreen.y, distanceZ)
            );
            SpawnBullet(extraWorld);
        }
    }

    private void SpawnBullet(Vector3 targetWorld)
    {
        var bullet = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        ).GetComponent<Bullet>();

        bullet.Initialize(
            targetWorld,
            initialSpeed: 18f,
            power: power + bonusPower,
            pierce: pierceLevel
        );
    }

    // =============================
    // Utils
    // =============================

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
    }

    private bool IsOverUI(Vector2 screenPos)
    {
        if (EventSystem.current == null) return false;

        var data = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        _uiHits.Clear();
        EventSystem.current.RaycastAll(data, _uiHits);

        foreach (var hit in _uiHits)
            if (hit.module is GraphicRaycaster)
                return true;

        return false;
    }
}
