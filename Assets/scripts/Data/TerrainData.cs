using System;
using UnityEngine;

[Serializable]
public class TerrainData
{
    [Header("Dimensions")]
    public int width = 100;
    public int height = 100;

    [Header("Scale")]
    public float horizontalScale = 1f;  // Distance between vertices
    public float verticalScale = 10f;    // Height multiplier

    [Header("Heightmap")]
    public float[] heightMap;  // Flattened array: heightMap[x + y * width]

    // No constructor - use Initialize method instead
    public void Initialize(int w = 100, int h = 100)
    {
        width = w;
        height = h;
        horizontalScale = 1f;
        verticalScale = 10f;
        heightMap = new float[width * height];

        // Initialize with flat terrain (all zeros)
        InitializeFlat();
    }

    // Initialize heightmap with flat terrain (all zeros)
    public void InitializeFlat()
    {
        if (heightMap == null || heightMap.Length != width * height)
        {
            heightMap = new float[width * height];
        }

        // Already zeros, but explicit for clarity
        for (int i = 0; i < heightMap.Length; i++)
        {
            heightMap[i] = 0f;
        }
    }

    // Get height at specific x, y coordinates
    public float GetHeight(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return 0f;

        return heightMap[x + y * width];
    }

    // Set height at specific x, y coordinates
    public void SetHeight(int x, int y, float value)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        heightMap[x + y * width] = value;
    }

    // Get height at world position (converts world X/Z to heightmap coordinates)
    public float GetHeightAtWorldPosition(float worldX, float worldZ)
    {
        int x = Mathf.FloorToInt(worldX / horizontalScale);
        int y = Mathf.FloorToInt(worldZ / horizontalScale);
        return GetHeight(x, y) * verticalScale;
    }

    // Resize terrain (creates new heightmap)
    public void Resize(int newWidth, int newHeight)
    {
        float[] newHeightMap = new float[newWidth * newHeight];

        // Copy old data (with clamping)
        int minWidth = Mathf.Min(width, newWidth);
        int minHeight = Mathf.Min(height, newHeight);

        for (int y = 0; y < minHeight; y++)
        {
            for (int x = 0; x < minWidth; x++)
            {
                newHeightMap[x + y * newWidth] = heightMap[x + y * width];
            }
        }

        width = newWidth;
        height = newHeight;
        heightMap = newHeightMap;
    }
}