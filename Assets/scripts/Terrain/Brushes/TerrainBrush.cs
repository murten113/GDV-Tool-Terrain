using UnityEngine;

public struct BrushStrokeContext
{
    public int CenterX;
    public int CenterY;
    public float BrushSize;
    public float BrushStrength;
    public float Falloff;
    public float DeltaTime;
}

public interface ITerrainBrush
{
    string BrushName { get; }

    float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context);
}

public abstract class TerrainBrush : MonoBehaviour, ITerrainBrush
{
    [SerializeField] private string brushName;
    [SerializeField] [TextArea] private string brushDescription;

    public string BrushName => string.IsNullOrEmpty(brushName) ? GetType().Name : brushName;

    public string BrushDescription =>
        string.IsNullOrWhiteSpace(brushDescription) ? GetDefaultDescription() : brushDescription;

    protected virtual string GetDefaultDescription() => $"{BrushName} brush.";

    public virtual void BeginStroke(TerrainData data, int centerX, int centerY) { }

    public virtual void EndStroke() { }

    public abstract float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context);
}
