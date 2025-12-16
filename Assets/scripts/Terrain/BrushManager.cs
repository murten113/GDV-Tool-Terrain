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

    private BaseBrush currentBrush;
    private TerrainRaycaster raycaster;

    public enum BrushType
    {
        Raise,
        Lower,
        Flatten
    }

    private void Start()
    {
        raycaster = FindFirstObjectByType<TerrainRaycaster>();

        if (raycaster == null)
            Debug.LogError("BrushManager: TerrainRaycaster not found!");

        // Set default brush settings
        UpdateBrushSettings();
    }

    private void Update()
    {
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

    // Set the active brush
    public void SetActiveBrush(BrushType brushType)
    {
        // Reset flatten brush if switching away
        if (flattenBrush != null && currentBrush != null && currentBrush.GetType() == typeof(FlattenBrush))
        {
            flattenBrush.ResetFlattenHeight();
        }

        switch (brushType)
        {
            case BrushType.Raise:
                currentBrush = raiseBrush;
                break;
            case BrushType.Lower:
                currentBrush = lowerBrush;
                break;
            case BrushType.Flatten:
                currentBrush = flattenBrush;
                break;
        }

        Debug.Log($"Active brush: {brushType}");
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