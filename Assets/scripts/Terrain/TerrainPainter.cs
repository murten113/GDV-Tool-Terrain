using System.Collections.Generic;
using UnityEngine;

public class TerrainPainter : MonoBehaviour
{
    private readonly List<(int x, int y, float height)> pendingUpdates = new List<(int x, int y, float height)>();

    private TerrainManager terrainManager;

    private void Awake()
    {
        terrainManager = FindFirstObjectByType<TerrainManager>();

        if (terrainManager == null)
            Debug.LogError("TerrainPainter: TerrainManager not found!");
    }

    public void ApplyBrush(ITerrainBrush brush, Vector3 worldPosition, float brushSize, float brushStrength)
    {
        if (brush == null || terrainManager == null || terrainManager.CurrentTerrainData == null)
            return;

        TerrainData data = terrainManager.CurrentTerrainData;

        int centerX = Mathf.FloorToInt(worldPosition.x / data.horizontalScale);
        int centerY = Mathf.FloorToInt(worldPosition.z / data.horizontalScale);
        int brushRadius = Mathf.CeilToInt(brushSize / data.horizontalScale);

        pendingUpdates.Clear();

        for (int y = -brushRadius; y <= brushRadius; y++)
        {
            for (int x = -brushRadius; x <= brushRadius; x++)
            {
                int heightmapX = centerX + x;
                int heightmapY = centerY + y;

                if (heightmapX < 0 || heightmapX >= data.width || heightmapY < 0 || heightmapY >= data.height)
                    continue;

                float worldDistance = Mathf.Sqrt(x * x + y * y) * data.horizontalScale;
                if (worldDistance > brushSize)
                    continue;

                float gridDistance = Mathf.Sqrt(x * x + y * y);
                if (gridDistance > brushRadius)
                    continue;

                float falloff = Mathf.Clamp01(1f - (gridDistance / brushRadius));
                float currentHeight = data.GetHeight(heightmapX, heightmapY);

                var context = new BrushStrokeContext
                {
                    CenterX = centerX,
                    CenterY = centerY,
                    BrushSize = brushSize,
                    BrushStrength = brushStrength,
                    Falloff = falloff,
                    DeltaTime = Time.deltaTime
                };

                float newHeight = brush.ComputeHeight(currentHeight, data, heightmapX, heightmapY, context);
                pendingUpdates.Add((heightmapX, heightmapY, newHeight));
            }
        }

        if (pendingUpdates.Count == 0)
            return;

        foreach (var (x, y, height) in pendingUpdates)
            data.SetHeight(x, y, height);

        terrainManager.UpdateMesh();
    }
}
