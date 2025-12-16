using UnityEngine;

public class TerrainRaycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask terrainLayerMask = -1; //all layers

    private TerrainManager terrainManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        terrainManager = FindFirstObjectByType<TerrainManager>();

        if (terrainManager == null)
        {
            Debug.LogError("TerrainRaycaster: No TerrainManager found in the scene.");
        }
    }

    public bool RaycastTerrain(out Vector3 hitPoint, out Vector3 hitNormal)
    {
        hitPoint = Vector3.zero;
        hitNormal = Vector3.up;

        if (mainCamera == null || terrainManager == null)
        {
            return false;
        }

        Vector3 mousePosition = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayerMask))
        {
            hitPoint = hit.point;
            hitNormal = hit.normal;
            return true;
        }
        return false;
    }
}
