using System;
using System.Collections.Generic;
using UnityEngine;

public class BrushManager : MonoBehaviour
{
    [Header("Brush References")]
    [SerializeField] private RaiseBrush raiseBrush;
    [SerializeField] private LowerBrush lowerBrush;
    [SerializeField] private SmoothBrush smoothBrush;

    [Header("Settings")]
    [SerializeField] private float brushSize = 5f;
    [SerializeField] private float brushStrength = 1f;

    [Header("Scroll Hotkeys")]
    [SerializeField] private float sizeScrollSpeed = 2f;
    [SerializeField] private float strengthScrollSpeed = 0.25f;
    [SerializeField] private float minBrushSize = 0.5f;
    [SerializeField] private float maxBrushSize = 50f;
    [SerializeField] private float minBrushStrength = 0.1f;
    [SerializeField] private float maxBrushStrength = 10f;

    public Action<float> OnBrushSizeChanged;
    public Action<float> OnBrushStrengthChanged;

    private Dictionary<Type, BaseBrush> brushes = new Dictionary<Type, BaseBrush>();
    private Type currentBrushType;
    private BaseBrush currentBrush;
    private TerrainRaycaster raycaster;

    private void Start()
    {
        raycaster = FindFirstObjectByType<TerrainRaycaster>();

        if (raycaster == null)
            Debug.LogError("BrushManager: TerrainRaycaster not found!");

        RegisterBrush(typeof(RaiseBrush), raiseBrush);
        RegisterBrush(typeof(LowerBrush), lowerBrush);
        RegisterBrush(typeof(SmoothBrush), smoothBrush);

        if (brushes.Count > 0)
            SetActiveBrush(typeof(RaiseBrush));

        UpdateBrushSettings();
    }

    public void RegisterBrush(Type brushType, BaseBrush brushInstance)
    {
        if (brushInstance == null)
        {
            Debug.LogWarning($"cannot register null brush instance for type {brushType}");
            return;
        }

        if (!brushType.IsSubclassOf(typeof(BaseBrush)) && brushType != typeof(BaseBrush))
        {
            Debug.LogError($"Type {brushType} is not a subclass of BaseBrush");
            return;
        }

        brushes[brushType] = brushInstance;
        Debug.Log($"Registered brush: {brushType.Name}");
    }

    public void SetActiveBrush(Type brushType)
    {
        if (!brushes.ContainsKey(brushType))
        {
            Debug.LogError($"Brush type {brushType} not registered!");
            return;
        }

        currentBrushType = brushType;
        currentBrush = brushes[brushType];
        UpdateBrushSettings();

        Debug.Log($"Active brush: {brushType.Name}");
    }

    public Type GetActiveBrushType() => currentBrushType;

    private void Update()
    {
        HandleScrollWheelAdjustments();

        if (Input.GetMouseButton(0) && currentBrush != null && !UIInputUtility.IsPointerOverUI())
            Paint();
    }

    private void HandleScrollWheelAdjustments()
    {
        if (UIInputUtility.IsPointerOverUI())
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.01f)
            return;

        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool ctrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (shiftHeld && !ctrlHeld)
        {
            float newSize = brushSize + scroll * sizeScrollSpeed;
            SetBrushSize(Mathf.Clamp(newSize, minBrushSize, maxBrushSize));
        }
        else if (ctrlHeld && !shiftHeld)
        {
            float newStrength = brushStrength + scroll * strengthScrollSpeed;
            SetBrushStrength(Mathf.Clamp(newStrength, minBrushStrength, maxBrushStrength));
        }
    }

    private void Paint()
    {
        if (raycaster == null || currentBrush == null)
            return;

        if (raycaster.RaycastTerrain(out Vector3 hitPoint, out Vector3 hitNormal))
            currentBrush.ApplyBrush(hitPoint);
    }

    public void SetBrushSize(float size)
    {
        brushSize = size;
        UpdateBrushSettings();
        OnBrushSizeChanged?.Invoke(brushSize);
    }

    public void SetBrushStrength(float strength)
    {
        brushStrength = strength;
        UpdateBrushSettings();
        OnBrushStrengthChanged?.Invoke(brushStrength);
    }

    private void UpdateBrushSettings()
    {
        if (raiseBrush != null)
        {
            raiseBrush.brushSize = brushSize;
            raiseBrush.brushStrength = brushStrength;
        }

        if (lowerBrush != null)
        {
            lowerBrush.brushSize = brushSize;
            lowerBrush.brushStrength = brushStrength;
        }

        if (smoothBrush != null)
        {
            smoothBrush.brushSize = brushSize;
            smoothBrush.brushStrength = brushStrength;
        }
    }

    public float GetBrushSize() => brushSize;
    public float GetBrushStrength() => brushStrength;
}
