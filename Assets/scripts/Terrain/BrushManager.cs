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


        // Check if we should paint
        if (Input.GetMouseButton(0) && currentBrush != null)
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
    }

    public void SetBrushStrength(float strength)
    {
        brushStrength = strength;
        UpdateBrushSettings();
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