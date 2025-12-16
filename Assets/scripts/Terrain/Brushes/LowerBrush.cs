using UnityEngine;

public class LowerBrush : BaseBrush
{
    public override void ApplyBrush(Vector3 worldPosition)
    {
        ModifyHeightmap(worldPosition, brushStrength, (currentHeight, strength) =>
        {
            // Lower: subtract from current height
            return currentHeight - strength * Time.deltaTime;
        });
    }
}