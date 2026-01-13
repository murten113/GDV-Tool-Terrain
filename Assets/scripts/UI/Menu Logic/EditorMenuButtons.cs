using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorMenuButtons : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exportAsButton;
    [SerializeField] private Button quitToMenuButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Settings")]
    [SerializeField] private string menuSceneName = "Menu";

    [Header("References")]
    [SerializeField] private TerrainSaveLoadManager saveLoadManager;
    [SerializeField] private TerrainManager terrainManager;

    private void Start()
    {
        // Find references if not assigned
        if (saveLoadManager == null)
            saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();

        if (terrainManager == null)
            terrainManager = FindFirstObjectByType<TerrainManager>();

        // Button listeners
        if (saveButton != null)
            saveButton.onClick.AddListener(OnSaveClicked);

        if (loadButton != null)
            loadButton.onClick.AddListener(OnLoadClicked);

        if (exportAsButton != null)
            exportAsButton.onClick.AddListener(OnExportAsClicked);

        if (quitToMenuButton != null)
            quitToMenuButton.onClick.AddListener(OnQuitToMenuClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnSaveClicked()
    {
        if (saveLoadManager == null || terrainManager == null)
        {
            Debug.LogError("SaveLoadManager or TerrainManager not found!");
            return;
        }

        TerrainProject currentProject = terrainManager.CurrentProject;
        if (currentProject == null)
        {
            Debug.LogError("No project to save!");
            return;
        }

        // Show save dialog directly
        string filePath = saveLoadManager.ShowSaveDialog(currentProject.projectName);

        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Save cancelled by user");
            return;
        }

        // Save project
        bool success = saveLoadManager.SaveProject(currentProject, filePath);

        if (success)
            Debug.Log($"Project saved successfully to: {filePath}");
        else
            Debug.LogError("Failed to save project!");
    }

    private void OnLoadClicked()
    {
        if (saveLoadManager == null || terrainManager == null)
        {
            Debug.LogError("SaveLoadManager or TerrainManager not found!");
            return;
        }

        // Show load dialog directly
        string filePath = saveLoadManager.ShowLoadDialog();

        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Load cancelled by user");
            return;
        }

        // Load project
        TerrainProject loadedProject = saveLoadManager.LoadProject(filePath);

        if (loadedProject != null)
        {
            terrainManager.LoadTerrain(loadedProject);
            Debug.Log($"Project loaded from: {filePath}");
        }
        else
        {
            Debug.LogError("Failed to load project!");
        }
    }

    private void OnExportAsClicked()
    {
        if (saveLoadManager == null || terrainManager == null)
        {
            Debug.LogError("SaveLoadManager or TerrainManager not found!");
            return;
        }

        TerrainProject currentProject = terrainManager.CurrentProject;
        if (currentProject == null)
        {
            Debug.LogError("No terrain project to export!");
            return;
        }

        // Show export dialog with both formats
        string defaultName = currentProject.projectName.Replace(" ", "_");
        string filePath = saveLoadManager.ShowExportDialogWithFormat(defaultName);

        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Export cancelled by user");
            return;
        }

        // Determine format from file extension
        string extension = System.IO.Path.GetExtension(filePath).ToLower();
        bool success = false;

        if (extension == ".obj")
        {
            success = saveLoadManager.ExportAsOBJ(filePath, currentProject);
        }
        else if (extension == ".png")
        {
            success = saveLoadManager.ExportHeightmapPNG(filePath, currentProject);
        }
        else
        {
            Debug.LogError($"Unsupported export format: {extension}");
            return;
        }

        if (success)
            Debug.Log($"Export successful! File saved to: {filePath}");
        else
            Debug.LogError("Export failed!");
    }

    private void OnQuitToMenuClicked()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    private void OnQuitClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}