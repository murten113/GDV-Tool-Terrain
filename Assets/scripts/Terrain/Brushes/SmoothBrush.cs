using System.Collections.Generic;
using UnityEngine;

public class SmoothBrush : BaseBrush
{
    public override void ApplyBrush(Vector3 worldPosition)
    {
        if (terrainManager == null || terrainManager.CurrentTerrainData == null)
            return;

        TerrainData data = terrainManager.CurrentTerrainData;

        int centerX = Mathf.FloorToInt(worldPosition.x / data.horizontalScale);
        int centerY = Mathf.FloorToInt(worldPosition.z / data.horizontalScale);
        int brushRadius = Mathf.CeilToInt(brushSize / data.horizontalScale);

        var updates = new List<(int x, int y, float height)>();

        for (int y = -brushRadius; y <= brushRadius; y++)
        {
            for (int x = -brushRadius; x <= brushRadius; x++)
            {
                int heightmapX = centerX + x;
                int heightmapY = centerY + y;

                float worldDistance = Mathf.Sqrt(x * x + y * y) * data.horizontalScale;
                if (worldDistance > brushSize)
                    continue;

                float gridDistance = Mathf.Sqrt(x * x + y * y);
                if (gridDistance > brushRadius)
                    continue;

                float falloff = Mathf.Clamp01(1f - (gridDistance / brushRadius));
                float currentHeight = data.GetHeight(heightmapX, heightmapY);
                float averageHeight = GetAverageNeighborHeight(data, heightmapX, heightmapY);
                float influence = falloff * brushStrength * Time.deltaTime;
                float newHeight = Mathf.Lerp(currentHeight, averageHeight, influence);

                updates.Add((heightmapX, heightmapY, newHeight));
            }
        }

        if (updates.Count == 0)
            return;

        foreach (var (x, y, height) in updates)
            data.SetHeight(x, y, height);

        terrainManager.UpdateMesh();
    }

    private static float GetAverageNeighborHeight(TerrainData data, int x, int y)
    {
        float sum = 0f;
        int count = 0;

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                int neighborX = x + dx;
                int neighborY = y + dy;

                if (neighborX < 0 || neighborX >= data.width || neighborY < 0 || neighborY >= data.height)
                    continue;

                sum += data.GetHeight(neighborX, neighborY);
                count++;
            }
        }

        return count > 0 ? sum / count : data.GetHeight(x, y);
    }
}
