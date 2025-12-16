using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [Header("Terrain Settings")]
    [SerializeField] private int terrainWidth = 100;
    [SerializeField] private int terrainHeight = 100;
    [SerializeField] private float horizontalScale = 1f;
    [SerializeField] private float verticalScale = 10f;

    [Header("References")]
    [SerializeField] private TerrainMeshGenerator meshGenerator;

    private TerrainData currentTerrainData;
    private TerrainProject currentProject;

    public TerrainData CurrentTerrainData => currentTerrainData;
    public TerrainProject CurrentProject => currentProject;

    private void Awake()
    {
        // Get or add mesh generator component
        if (meshGenerator == null)
            meshGenerator = GetComponent<TerrainMeshGenerator>();

        if (meshGenerator == null)
            meshGenerator = gameObject.AddComponent<TerrainMeshGenerator>();
    }
}
