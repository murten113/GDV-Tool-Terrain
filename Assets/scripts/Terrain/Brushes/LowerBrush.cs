using UnityEngine;

public class LowerBrush : TerrainBrush
{
    public override float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context)
    {
        return currentHeight - context.Falloff * context.BrushStrength * context.DeltaTime;
    }
}
