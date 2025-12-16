using UnityEngine;

public class RaiseBrush : BaseBrush
{
    public override void ApplyBrush(Vector3 worldPosition)
    {
        ModifyHeightmap(worldPosition, brushStrength, (currentHeight, strength) =>
        {
            return currentHeight + strength * Time.deltaTime;
        });
    }
}
