using System;
using System.Collections.Generic;
using UnityEngine;

public class BrushManager : MonoBehaviour
{
    [Header("Brush References")]
    [SerializeField] private RaiseBrush raiseBrush;
    [SerializeField] private LowerBrush lowerBrush;
    [SerializeField] private FlattenBrush flattenBrush;

    [Header("Settings")]
    [SerializeField] private float brushSize = 5f;
    [SerializeField] private float brushStrength = 1f;


    [Header("Hotkey Settings")]
    [SerializeField] private float sizeAdjustSpeed = 0.5f;
    [SerializeField] private float strengthAdjustSpeed = 0.1f;
    [SerializeField] private float minBrushSize = 0.5f;
    [SerializeField] private float maxBrushSize = 50f;
    [SerializeField] private float minBrushStrength = 0.1f;
    [SerializeField] private float maxBrushStrength = 10f;

    private bool isAdjustingSize = false;
    private bool isAdjustingStrength = false;

    // Events for UI updates
    public System.Action<float> OnBrushSizeChanged;
    public System.Action<float> OnBrushStrengthChanged;
    public System.Action<bool> OnSizeAdjustModeChanged; // true = adjusting size
    public System.Action<bool> OnStrengthAdjustModeChanged; // true = adjusting strength

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
        RegisterBrush(typeof(FlattenBrush), flattenBrush);
        
        // Set default brush
        if (brushes.Count > 0)
        {
            SetActiveBrush(typeof(RaiseBrush));
        }
        
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

    // Set active brush by Type
    public void SetActiveBrush(Type brushType)
    {
        if (!brushes.ContainsKey(brushType))
        {
            Debug.LogError($"Brush type {brushType} not registered!");
            return;
        }

        // Reset flatten brush if switching away
        if (currentBrush != null && currentBrush.GetType() == typeof(FlattenBrush))
        {
            FlattenBrush flatten = currentBrush as FlattenBrush;
            if (flatten != null)
                flatten.ResetFlattenHeight();
        }

        currentBrushType = brushType;
        currentBrush = brushes[brushType];
        UpdateBrushSettings();

        Debug.Log($"Active brush: {brushType.Name}");
    }

    // Get active brush type
    public Type GetActiveBrushType() => currentBrushType;

    private void Update()
    {
        // Handle hotkey adjustments
        HandleHotkeys();

        // Handle brush size adjustment
        if (isAdjustingSize)
        {
            float mouseDelta = Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y");
            float newSize = brushSize + (mouseDelta * sizeAdjustSpeed);
            SetBrushSize(Mathf.Clamp(newSize, minBrushSize, maxBrushSize));
        }

        // Handle brush strength adjustment
        if (isAdjustingStrength)
        {
            float mouseDelta = Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y");
            float newStrength = brushStrength + (mouseDelta * strengthAdjustSpeed);
            SetBrushStrength(Mathf.Clamp(newStrength, minBrushStrength, maxBrushStrength));
        }

        // Check if we should paint
        if (Input.GetMouseButton(0) && currentBrush != null && !isAdjustingSize && !isAdjustingStrength)
        {
            Paint();
        }

        // Reset flatten height when mouse is released
        if (Input.GetMouseButtonUp(0) && flattenBrush != null)
        {
            flattenBrush.ResetFlattenHeight();
        }
    }

    private void HandleHotkeys()
    {
        bool wasAdjustingSize = isAdjustingSize;
        bool wasAdjustingStrength = isAdjustingStrength;

        // F key: Toggle size adjustment
        if (Input.GetKeyDown(KeyCode.F) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            isAdjustingSize = !isAdjustingSize;
            isAdjustingStrength = false; // Cancel strength adjustment
            
            if (isAdjustingSize != wasAdjustingSize)
            {
                OnSizeAdjustModeChanged?.Invoke(isAdjustingSize);
                Debug.Log(isAdjustingSize ? "Brush Size Adjustment Mode: ON (Move mouse to adjust)" : "Brush Size Adjustment Mode: OFF");
            }
        }
        
        // Shift+F key: Toggle strength adjustment
        if (Input.GetKeyDown(KeyCode.F) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            isAdjustingStrength = !isAdjustingStrength;
            isAdjustingSize = false; // Cancel size adjustment
            
            if (isAdjustingStrength != wasAdjustingStrength)
            {
                OnStrengthAdjustModeChanged?.Invoke(isAdjustingStrength);
                Debug.Log(isAdjustingStrength ? "Brush Strength Adjustment Mode: ON (Move mouse to adjust)" : "Brush Strength Adjustment Mode: OFF");
            }
        }

        // Exit adjustment modes on mouse click or Escape
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isAdjustingSize || isAdjustingStrength)
            {
                bool wasSize = isAdjustingSize;
                bool wasStrength = isAdjustingStrength;
                isAdjustingSize = false;
                isAdjustingStrength = false;
                
                if (wasSize) OnSizeAdjustModeChanged?.Invoke(false);
                if (wasStrength) OnStrengthAdjustModeChanged?.Invoke(false);
            }
        }
    }

    // Paint at mouse position
    private void Paint()
    {
        if (raycaster == null || currentBrush == null)
            return;

        Vector3 hitPoint;
        Vector3 hitNormal;

        if (raycaster.RaycastTerrain(out hitPoint, out hitNormal))
        {
            currentBrush.ApplyBrush(hitPoint);
        }
    }

    // Update brush settings (size and strength)
    public void SetBrushSize(float size)
    {
        brushSize = size;
        UpdateBrushSettings();
        OnBrushSizeChanged?.Invoke(brushSize); // Notify UI
    }

    public void SetBrushStrength(float strength)
    {
        brushStrength = strength;
        UpdateBrushSettings();
        OnBrushStrengthChanged?.Invoke(brushStrength); // Notify UI
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

        if (flattenBrush != null)
        {
            flattenBrush.brushSize = brushSize;
            flattenBrush.brushStrength = brushStrength;
        }
    }

    public float GetBrushSize() => brushSize;
    public float GetBrushStrength() => brushStrength;
}