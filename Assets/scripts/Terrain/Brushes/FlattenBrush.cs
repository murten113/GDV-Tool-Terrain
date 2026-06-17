using UnityEngine;

public class FlattenBrush : TerrainBrush
{
    protected override string GetDefaultDescription() =>
        "Levels terrain toward the height where you clicked to start painting. Drag to flatten an area.";

    private float targetHeight;
    private bool hasTarget;

    public override void BeginStroke(TerrainData data, int centerX, int centerY)
    {
        if (centerX < 0 || centerX >= data.width || centerY < 0 || centerY >= data.height)
            return;

        targetHeight = data.GetHeight(centerX, centerY);
        hasTarget = true;
    }

    public override void EndStroke()
    {
        hasTarget = false;
    }

    public override float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context)
    {
        if (!hasTarget)
            return currentHeight;

        float influence = Mathf.Clamp01(context.Falloff * context.BrushStrength * 8f * context.DeltaTime);
        return Mathf.Lerp(currentHeight, targetHeight, influence);
    }
}
