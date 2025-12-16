using UnityEngine;

public abstract class BaseBrush : MonoBehaviour
{
    [Header("Brush Settings")]
    public float brushSize = 5f;
    public float brushStrength = 1f;

    protected TerrainManager terrainManager;
    protected TerrainRaycaster raycaster;

    protected virtual void Start()
    {
        terrainManager = FindFirstObjectByType<TerrainManager>();
        raycaster = FindFirstObjectByType<TerrainRaycaster>();
        if (terrainManager == null)
            Debug.LogError("BaseBrush: TerrainManager not found!");
        if (raycaster == null)
            Debug.LogError("BaseBrush: TerrainRaycaster not found!");
    }

    public abstract void ApplyBrush(Vector3 worldPosition);

    protected void ModifyHeightmap(Vector3 centerWorldPos, float strength, System.Func<float, float, float> heightModifier)
    {
        if (terrainManager == null || terrainManager.CurrentTerrainData == null)
            return;

        TerrainData data = terrainManager.CurrentTerrainData;

        // Convert world position to heightmap coordinates
        int centerX = Mathf.FloorToInt(centerWorldPos.x / data.horizontalScale);
        int centerY = Mathf.FloorToInt(centerWorldPos.z / data.horizontalScale);

        // Calculate brush radius in heightmap units
        int brushRadius = Mathf.CeilToInt(brushSize / data.horizontalScale);

        bool hasChanges = false;
        for (int y = -brushRadius; y <= brushRadius; y++)
        {
            for (int x = -brushRadius; x <= brushRadius; x++)
            {
                // Calculate heightmap coordinates
                int heightmapX = centerX + x;
                int heightmapY = centerY + y;

                // Check bounds
                float dist = Mathf.Sqrt(x * x + y * y) * data.horizontalScale;
                if (dist > brushSize)
                    continue;

                float distance = Mathf.Sqrt(x * x + y * y);
                if (distance > brushRadius)
                    continue;

                float normalizedDistance = distance / brushRadius;
                float falloff = 1f - normalizedDistance; // Linear falloff
                falloff = Mathf.Clamp01(falloff);

                // Get current height
                float currentHeight = data.GetHeight(heightmapX, heightmapY);

                // Apply modifier
                float newHeight = heightModifier(currentHeight, falloff * strength * brushStrength);

                // Set new height
                data.SetHeight(heightmapX, heightmapY, newHeight);
                hasChanges = true;
            }
        }
    }
}
