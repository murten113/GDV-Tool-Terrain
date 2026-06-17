using UnityEngine;

public class RaiseBrush : TerrainBrush
{
    public override float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context)
    {
        return currentHeight + context.Falloff * context.BrushStrength * context.DeltaTime;
    }
}
