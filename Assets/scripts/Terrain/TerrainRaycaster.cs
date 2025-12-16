using UnityEngine;

public class TerrainRaycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask terrainLayerMask = -1; // All layers

    private TerrainManager terrainManager;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        terrainManager = FindFirstObjectByType<TerrainManager>();

        if (terrainManager == null)
            Debug.LogError("TerrainRaycaster: TerrainManager not found!");
    }

    // Raycast from mouse position to terrain
    public bool RaycastTerrain(out Vector3 hitPoint, out Vector3 hitNormal)
    {
        hitPoint = Vector3.zero;
        hitNormal = Vector3.up;

        if (mainCamera == null || terrainManager == null)
            return false;

        // Get mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Create ray from camera through mouse position
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // Raycast to terrain mesh
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayerMask))
        {
            hitPoint = hit.point;
            hitNormal = hit.normal;
            return true;
        }

        return false;
    }

    // Get terrain heightmap coordinates from world position
    public bool WorldToHeightmapCoords(Vector3 worldPos, out int x, out int y)
    {
        x = 0;
        y = 0;

        if (terrainManager == null || terrainManager.CurrentTerrainData == null)
            return false;

        TerrainData terrainData = terrainManager.CurrentTerrainData;

        // Convert world position to heightmap coordinates
        x = Mathf.FloorToInt(worldPos.x / terrainData.horizontalScale);
        y = Mathf.FloorToInt(worldPos.z / terrainData.horizontalScale);

        // Check bounds
        if (x < 0 || x >= terrainData.width || y < 0 || y >= terrainData.height)
            return false;

        return true;
    }
}