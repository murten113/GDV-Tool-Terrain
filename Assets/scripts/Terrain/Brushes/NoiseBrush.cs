using UnityEngine;

public class NoiseBrush : TerrainBrush
{
    protected override string GetDefaultDescription() =>
        "Adds subtle random height variation for a more natural, uneven surface.";

    [SerializeField] private float noiseScale = 0.15f;

    public override float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context)
    {
        float noise = Mathf.PerlinNoise(x * 0.2f + context.CenterX * 0.01f, y * 0.2f + context.CenterY * 0.01f);
        noise = (noise * 2f) - 1f;

        return currentHeight + noise * context.Falloff * context.BrushStrength * noiseScale * context.DeltaTime;
    }
}
