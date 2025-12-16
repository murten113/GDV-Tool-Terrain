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
            filePath = GetProjectFilePath;
        }
    }
}
