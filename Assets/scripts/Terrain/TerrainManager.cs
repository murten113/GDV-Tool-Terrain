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

    private void Start()
    {
        // Initialize terrain on start
        initializeTerrain();
    }

    public void initializeTerrain()
    {
        currentTerrainData = new TerrainData(terrainWidth, terrainHeight);
        currentTerrainData.horizontalScale = horizontalScale;
        currentTerrainData.verticalScale = verticalScale;

        currentProject = new TerrainProject("Default Project", currentTerrainData);

        GenerateTerrainMesh();
    }

    public void LoadTerrain(TerrainProject project)
    {
        if (project == null || project.terrainData == null)
        {
            Debug.LogError("cant load null project");
            return;
        }

        currentProject = project;
        currentTerrainData = project.terrainData;

        // Update terrain settings
        terrainWidth = currentTerrainData.width;
        terrainHeight = currentTerrainData.height;
        horizontalScale = currentTerrainData.horizontalScale;
        verticalScale = currentTerrainData.verticalScale;

        GenerateTerrainMesh();
    }

    public void CreateNewTerrain(string projectName, int width, int height)
    {
        terrainWidth = width;
        terrainHeight = height;

        currentTerrainData = new TerrainData(width, height);
        currentTerrainData.horizontalScale = horizontalScale;
        currentTerrainData.verticalScale = verticalScale;

        currentProject = new TerrainProject(projectName, currentTerrainData);

        GenerateTerrainMesh();
    }






    // Generate the mesh from current terrain data
    private void GenerateTerrainMesh()
    {
        if (meshGenerator != null && currentTerrainData != null)
        {
            meshGenerator.GenerateMesh(currentTerrainData);
        }
    }
}
