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


    #region ExportOBJ
    /// <summary>
    /// Export terrain mesh as OBJ file
    /// </summary>
    public bool ExportAsOBJ(string filePath, TerrainProject project)
    {
        if (project == null || project.terrainData == null)
        {
            Debug.LogError("Cannot export: Project or terrain data is null!");
            return false;
        }

        try
        {
            // Ensure directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Ensure .obj extension
            if (!filePath.EndsWith(".obj", System.StringComparison.OrdinalIgnoreCase))
            {
                filePath += ".obj";
            }

            // Generate OBJ content
            string objContent = GenerateOBJContent(project.terrainData);

            // Write to file
            File.WriteAllText(filePath, objContent);

            Debug.Log($"Terrain exported as OBJ to: {filePath}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to export terrain as OBJ: {e.Message}");
            return false;
        }
    }
    #endregion

    #region ExportHeightmapPNG
    /// <summary>
    /// Export heightmap as PNG image
    /// </summary>
    public bool ExportHeightmapPNG(string filePath, TerrainProject project)
    {
        if (project == null || project.terrainData == null)
        {
            Debug.LogError("Cannot export: Project or terrain data is null!");
            return false;
        }

        try
        {
            // Ensure directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Ensure .png extension
            if (!filePath.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase))
            {
                filePath += ".png";
            }

            TerrainData data = project.terrainData;

            // Create texture from heightmap
            Texture2D heightmapTexture = new Texture2D(data.width, data.height);

            for (int y = 0; y < data.height; y++)
            {
                for (int x = 0; x < data.width; x++)
                {
                    float height = data.GetHeight(x, y);
                    // Normalize height to 0-1 range for grayscale
                    float normalizedHeight = Mathf.Clamp01(height);
                    Color pixel = new Color(normalizedHeight, normalizedHeight, normalizedHeight, 1f);
                    heightmapTexture.SetPixel(x, y, pixel);
                }
            }

            heightmapTexture.Apply();

            // Encode to PNG
            byte[] pngData = heightmapTexture.EncodeToPNG();

            // Clean up texture
            Object.Destroy(heightmapTexture);

            // Write to file
            File.WriteAllBytes(filePath, pngData);

            Debug.Log($"Heightmap exported as PNG to: {filePath}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to export heightmap as PNG: {e.Message}");
            return false;
        }
    }
    #endregion

    #region OBJGeneration
    /// <summary>
    /// Generate OBJ file content from terrain data
    /// </summary>
    private string GenerateOBJContent(TerrainData terrainData)
    {
        System.Text.StringBuilder obj = new System.Text.StringBuilder();

        // Header
        obj.AppendLine("# Terrain Editor Export");
        obj.AppendLine($"# Size: {terrainData.width}x{terrainData.height}");
        obj.AppendLine();

        // Vertices
        obj.AppendLine("# Vertices");
        for (int y = 0; y <= terrainData.height; y++)
        {
            for (int x = 0; x <= terrainData.width; x++)
            {
                int heightmapX = Mathf.Min(x, terrainData.width - 1);
                int heightmapY = Mathf.Min(y, terrainData.height - 1);
                float heightValue = terrainData.GetHeight(heightmapX, heightmapY) * terrainData.verticalScale;

                float worldX = x * terrainData.horizontalScale;
                float worldY = heightValue;
                float worldZ = y * terrainData.horizontalScale;

                obj.AppendLine($"v {worldX} {worldY} {worldZ}");
            }
        }
        obj.AppendLine();

        // Texture coordinates (UVs)
        obj.AppendLine("# Texture Coordinates");
        for (int y = 0; y <= terrainData.height; y++)
        {
            for (int x = 0; x <= terrainData.width; x++)
            {
                float u = (float)x / terrainData.width;
                float v = (float)y / terrainData.height;
                obj.AppendLine($"vt {u} {v}");
            }
        }
        obj.AppendLine();

        // Faces (triangles)
        obj.AppendLine("# Faces");
        int vertexWidth = terrainData.width + 1;

        for (int y = 0; y < terrainData.height; y++)
        {
            for (int x = 0; x < terrainData.width; x++)
            {
                // OBJ indices start at 1, not 0
                int bottomLeft = (x + y * vertexWidth) + 1;
                int bottomRight = ((x + 1) + y * vertexWidth) + 1;
                int topLeft = (x + (y + 1) * vertexWidth) + 1;
                int topRight = ((x + 1) + (y + 1) * vertexWidth) + 1;

                // First triangle
                obj.AppendLine($"f {bottomLeft}/{bottomLeft} {topLeft}/{topLeft} {topRight}/{topRight}");

                // Second triangle
                obj.AppendLine($"f {bottomLeft}/{bottomLeft} {topRight}/{topRight} {bottomRight}/{bottomRight}");
            }
        }

        return obj.ToString();
    }
    #endregion
}