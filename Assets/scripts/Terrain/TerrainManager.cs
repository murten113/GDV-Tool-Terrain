using UnityEngine;
using System.IO;

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
        // Check if we're loading from menu with project path stored in PlayerPrefs
        string projectPath = PlayerPrefs.GetString(MenuContext.ProjectPathPrefKey, "");
        
        if (!string.IsNullOrEmpty(projectPath) && System.IO.File.Exists(projectPath))
        {
            // Load project from file
            TerrainSaveLoadManager saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();
            if (saveLoadManager == null)
            {
                GameObject go = new GameObject("TerrainSaveLoadManager");
                saveLoadManager = go.AddComponent<TerrainSaveLoadManager>();
            }
            
            if (saveLoadManager != null)
            {
                TerrainProject loadedProject = saveLoadManager.LoadProject(projectPath);
                if (loadedProject != null)
                {
                    LoadTerrain(loadedProject);
                    // Clear the PlayerPrefs after loading
                    PlayerPrefs.DeleteKey(MenuContext.ProjectPathPrefKey);
                    PlayerPrefs.Save();
                    return;
                }
            }
            
            // If loading failed, clear the pref and fall through to default
            PlayerPrefs.DeleteKey(MenuContext.ProjectPathPrefKey);
            PlayerPrefs.Save();
        }
        
        // Fallback: Check old ProjectDataTransfer (for backwards compatibility, can be removed later)
        ProjectDataTransfer transfer = ProjectDataTransfer.Instance;
        
        if (transfer != null && transfer.isNewProject)
        {
            // Create new project with transferred data
            CreateNewTerrain(transfer.ProjectName, transfer.terrainWidth, transfer.terrainHeight);
            transfer.ClearData(); // Clear after use
        }
        else if (transfer != null && !transfer.isNewProject && !string.IsNullOrEmpty(transfer.loadFilePath))
        {
            // Load existing project
            TerrainSaveLoadManager saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();
            if (saveLoadManager != null)
            {
                TerrainProject loadedProject = saveLoadManager.LoadProject(transfer.loadFilePath);
                if (loadedProject != null)
                {
                    LoadTerrain(loadedProject);
                }
            }
            transfer.ClearData(); // Clear after use
        }
        else
        {
            // Default: Initialize with flat terrain
            InitializeTerrain();
        }
    }

    // Initialize terrain with flat terrain
    public void InitializeTerrain()
    {
        currentTerrainData = new TerrainData();
        currentTerrainData.Initialize(terrainWidth, terrainHeight);
        currentTerrainData.horizontalScale = horizontalScale;
        currentTerrainData.verticalScale = verticalScale;

        currentProject = new TerrainProject();
        currentProject.Initialize("Default Project", currentTerrainData);

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


    // Create new terrain project
    public void CreateNewTerrain(string projectName, int width, int height)
    {
        terrainWidth = width;
        terrainHeight = height;

        currentTerrainData = new TerrainData();
        currentTerrainData.Initialize(width, height);
        currentTerrainData.horizontalScale = horizontalScale;
        currentTerrainData.verticalScale = verticalScale;

        currentProject = new TerrainProject();
        currentProject.Initialize(projectName, currentTerrainData);

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

    // Update the mesh when terrain data changes
    public void UpdateMesh()
    {
        if (meshGenerator != null && currentTerrainData != null)
        {
            meshGenerator.GenerateMesh(currentTerrainData);
        }
    }

    public float GetHeightAtWorldPosition(float worldX, float worldZ)
    {
        if (currentTerrainData != null)
        {
            return currentTerrainData.GetHeightAtWorldPosition(worldX, worldZ);
        }

        return 0f;
    }
}
