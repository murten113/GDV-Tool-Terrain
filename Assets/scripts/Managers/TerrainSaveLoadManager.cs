using System.IO;
using UnityEngine;

public class TerrainSaveLoadManager : MonoBehaviour
{
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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Get the full path for saving projects
    private string GetSaveDirectory()
    {
        string path = Path.Combine(Application.persistentDataPath, defaultSaveDirectory);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    // Get full file path for a project
    private string GetProjectFilePath(string projectName)
    {
        string fileName = $"{projectName}.terrain";
        return Path.Combine(GetSaveDirectory(), fileName);
    }

    // Save project to file
    public bool SaveProject(TerrainProject project, string filePath = null)
    {
        if (project == null || project.terrainData == null)
        {
            Debug.LogError("Cannot save null project or terrain data!");
            return false;
        }

        // Update last modified date
        project.UpdateLastModified();

        // Use provided path or generate from project name
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = GetProjectFilePath(project.projectName);
        }

        try
        {
            // Convert to JSON
            string json = JsonUtility.ToJson(project, true);

            // Write to file
            File.WriteAllText(filePath, json);

            Debug.Log($"Project saved successfully to: {filePath}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save project: {e.Message}");
            return false;
        }
    }

    // Load project from file
    public TerrainProject LoadProject(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Project file not found: {filePath}");
            return null;
        }

        try
        {
            // Read JSON from file
            string json = File.ReadAllText(filePath);

            // Parse JSON to TerrainProject
            TerrainProject project = JsonUtility.FromJson<TerrainProject>(json);

            // Initialize terrain data if needed
            if (project.terrainData != null)
            {
                // Ensure heightmap is initialized
                if (project.terrainData.heightMap == null ||
                    project.terrainData.heightMap.Length != project.terrainData.width * project.terrainData.height)
                {
                    project.terrainData.InitializeFlat();
                }
            }

            Debug.Log($"Project loaded successfully from: {filePath}");
            return project;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load project: {e.Message}");
            return null;
        }
    }

    // Get all project files in save directory
    public string[] GetAllProjectFiles()
    {
        string directory = GetSaveDirectory();
        if (!Directory.Exists(directory))
        {
            return new string[0];
        }

        return Directory.GetFiles(directory, "*.terrain");
    }

    // Get project name from file path
    public string GetProjectNameFromPath(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        return fileName;
    }
}