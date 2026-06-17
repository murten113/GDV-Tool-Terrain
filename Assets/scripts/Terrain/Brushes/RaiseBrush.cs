using UnityEngine;

public class RaiseBrush : TerrainBrush
{
    protected override string GetDefaultDescription() =>
        "Raises terrain where you paint. Use strength and size to control how quickly land lifts.";

    public override float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context)
    {
        return currentHeight + context.Falloff * context.BrushStrength * context.DeltaTime;
    }
}
