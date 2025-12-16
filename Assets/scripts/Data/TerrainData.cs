using System;
using UnityEngine;


[Serializable]
public class TerrainData : MonoBehaviour
{
    [Header("Dimensions")]
    public int width = 100;
    public int height = 100;

    [Header("Scale")]
    public float horizontalScale = 1f; // Distance between points in the x-axis
    public float verticalScale = 10f; //heightmap multiplier

    [Header("HeightMmap")]
    public float[] heightMap; //Flattened array: heightmap[x + y * witdth]


    public TerrainData(int w = 100, int h = 100)
    {
        width = w;
        height = h;
        horizontalScale = 1f;
        verticalScale = 10f;
        heightMap = new float[width * height];

        //initialize flat terrain
        InitializeFlat();
    }


    // Initializes the heightmap to a flat terrain (all zeros)
    public void InitializeFlat()
    {
        if (heightMap == null || heightMap.Length != width * height)
        {
            heightMap = new float[width * height];
        }


        //alredy zeroed by default, but just to be sure
        for (int i = 0; i < heightMap.Length; i++)
        {
            heightMap[i] = 0f;
        }
    }


    // Gets the height at the specified grid coordinates
    public float GetHeight(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return 0f;


        return heightMap[x + y * width];
    }


    // Sets the height at the specified grid coordinates
    public float GetHeightAtWorldPosition(float worldX, float worldZ)
    {
        int x = Mathf.RoundToInt(worldX / horizontalScale);
        int y = Mathf.RoundToInt(worldZ / horizontalScale);
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
