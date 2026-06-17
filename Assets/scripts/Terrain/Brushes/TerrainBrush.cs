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

    public string BrushName => string.IsNullOrEmpty(brushName) ? GetType().Name : brushName;

    public abstract float ComputeHeight(float currentHeight, TerrainData data, int x, int y, BrushStrokeContext context);
}
