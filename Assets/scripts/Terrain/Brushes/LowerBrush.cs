using UnityEngine;

public class LowerBrush : TerrainBrush
{
    protected override string GetDefaultDescription() =>
        "Lowers terrain where you paint. Use strength and size to control how quickly land sinks.";

    public override float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context)
    {
        return currentHeight - context.Falloff * context.BrushStrength * context.DeltaTime;
    }
}
