using UnityEngine;

public class SmoothBrush : TerrainBrush
{
    protected override string GetDefaultDescription() =>
        "Softens bumps by blending each point with its neighbors. Good for polishing rough shapes.";

    public override float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context)
    {
        float averageHeight = GetAverageNeighborHeight(data, x, y);
        float influence = context.Falloff * context.BrushStrength * context.DeltaTime;
        return Mathf.Lerp(currentHeight, averageHeight, influence);
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
