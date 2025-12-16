using System.IO;
using UnityEngine;

public class TerrainSaveLoadManager : MonoBehaviour
{

    // Singleton instance
    private static TerrainSaveLoadManager instance;
    public static TerrainSaveLoadManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<TerrainSaveLoadManager>();
            return instance;
        }
    }

    [Header("Settings")]
    [SerializeField] private string defaultSaveDirectory = "Projects";

    // Ensure singleton instance
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Get or create the save directory path
    public string GetSaveDirectory()
    {
        string path = Path.Combine(Application.persistentDataPath, defaultSaveDirectory);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }

    // Get full file path for a project
    private string GetProjectFilePath(string projectName)
    {
        string fileName = $"{projectName}.terrain";
        return Path.Combine(GetSaveDirectory(), fileName);
    }


    // Save a TerrainProject to a file
    public bool SaveProject(TerrainProject project, string filePath = null)
    {
        if (project == null || project.terrainData == null)
        {
            Debug.LogError("Cannot save project: Project or terrain data is null.");
            return false;
        }

        project.UpdateLastModified();

        if (string.IsNullOrEmpty(filePath))
        {
            filePath = GetProjectFilePath(project.projectName);
        }

        try
        {
            string json = JsonUtility.ToJson(project, true);

            File.WriteAllText(filePath, json);

            Debug.Log($"Project '{project.projectName}' saved successfully to {filePath}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save project '{project.projectName}': {e.Message}");
            return false;
        }
    }

    // Load a TerrainProject from a file
    public TerrainProject LoadProject(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Cannot load project: File does not exist at {filePath}");
            return null;
        }
        try
        {
            string json = File.ReadAllText(filePath);

            TerrainProject project = JsonUtility.FromJson<TerrainProject>(json);
            if (project.terrainData != null)
            {
                if (project.terrainData.heightMap == null || project.terrainData.heightMap.Length != project.terrainData.width * project.terrainData.height)
                {
                    project.terrainData.InitializeFlat();
                }
            }

            Debug.Log($"Project '{project.projectName}' loaded successfully from {filePath}");
            return project;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load project from '{filePath}': {e.Message}");
            return null;
        }
    }

    public string[] GetAllProjectFiles()
    {
        string saveDir = GetSaveDirectory();
        if (!Directory.Exists(saveDir))
        {
            return new string[0];
        }
        return Directory.GetFiles(saveDir, "*.terrain");
    }

    public string GetProjectNameFromPath(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        return fileName;
    }
}
