using UnityEngine;

public class FlattenBrush : BaseBrush
{
    private float targetHeight = 0f; // The height to flatten to
    private bool heightSet = false;

    public override void ApplyBrush(Vector3 worldPosition)
    {
        // On the first application, set the target height
        if (!heightSet)
        {
           if(terrainManager != null && terrainManager.CurrentTerrainData != null)
            {
                TerrainData data = terrainManager.CurrentTerrainData;
                int x, y;
                if (raycaster != null && raycaster.WorldToHeightmapCoords(worldPosition, out x, out y))
                {
                    targetHeight = data.GetHeight(x, y);
                    heightSet = true;
                }
            }
        }
        float finalTargetHeight = targetHeight;

        ModifyHeightmap(worldPosition, brushStrength, (currentHeight, strength) =>
        {
            // Flatten: move current height towards target height
            return Mathf.Lerp(currentHeight, finalTargetHeight, strength * Time.deltaTime);
        });
    }

    //reset tartget height when brush is deselected
    public void ResetFlattenHeight()
    {
        heightSet = false;
    }
}
