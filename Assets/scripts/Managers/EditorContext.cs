using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorContext : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string menuSceneName = "Menu";

    [Header("UI")]
    [SerializeField] private EditorMenuButtons editorMenuButtons;

    [Header("Systems")]
    [SerializeField] private TerrainSaveLoadManager saveLoadManager;
    [SerializeField] private TerrainManager terrainManager;

    private void Awake()
    {
        if (editorMenuButtons == null)
            editorMenuButtons = FindFirstObjectByType<EditorMenuButtons>();

        if (saveLoadManager == null)
            saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();

        if (terrainManager == null)
            terrainManager = FindFirstObjectByType<TerrainManager>();
    }

    private void Start()
    {
        if (editorMenuButtons == null)
            return;

        editorMenuButtons.SaveRequested += HandleSaveRequested;
        editorMenuButtons.LoadRequested += HandleLoadRequested;
        editorMenuButtons.ExportRequested += HandleExportRequested;
        editorMenuButtons.QuitToMenuRequested += HandleQuitToMenuRequested;
        editorMenuButtons.QuitRequested += HandleQuitRequested;
    }

    private void OnDestroy()
    {
        if (editorMenuButtons == null)
            return;

        editorMenuButtons.SaveRequested -= HandleSaveRequested;
        editorMenuButtons.LoadRequested -= HandleLoadRequested;
        editorMenuButtons.ExportRequested -= HandleExportRequested;
        editorMenuButtons.QuitToMenuRequested -= HandleQuitToMenuRequested;
        editorMenuButtons.QuitRequested -= HandleQuitRequested;
    }

    private void HandleSaveRequested()
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

        string filePath = saveLoadManager.ShowSaveDialog(currentProject.projectName);
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Save cancelled by user");
            return;
        }

        if (saveLoadManager.SaveProject(currentProject, filePath))
            Debug.Log($"Project saved successfully to: {filePath}");
        else
            Debug.LogError("Failed to save project!");
    }

    private void HandleLoadRequested()
    {
        if (saveLoadManager == null || terrainManager == null)
        {
            Debug.LogError("SaveLoadManager or TerrainManager not found!");
            return;
        }

        string filePath = saveLoadManager.ShowLoadDialog();
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Load cancelled by user");
            return;
        }

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

    private void HandleExportRequested()
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

        string defaultName = currentProject.projectName.Replace(" ", "_");
        string filePath = saveLoadManager.ShowExportDialogWithFormat(defaultName);
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Export cancelled by user");
            return;
        }

        string extension = Path.GetExtension(filePath).ToLower();
        bool success = extension switch
        {
            ".obj" => saveLoadManager.ExportAsOBJ(filePath, currentProject),
            ".png" => saveLoadManager.ExportHeightmapPNG(filePath, currentProject),
            _ => false
        };

        if (!success && extension != ".obj" && extension != ".png")
            Debug.LogError($"Unsupported export format: {extension}");
        else if (success)
            Debug.Log($"Export successful! File saved to: {filePath}");
        else
            Debug.LogError("Export failed!");
    }

    private void HandleQuitToMenuRequested()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    private void HandleQuitRequested()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
