using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EditorMenuButtons : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exportAsButton;
    [SerializeField] private Button quitToMenuButton;
    [SerializeField] private Button quitButton;

    [Header("Popup Panels")]
    [SerializeField] private GameObject loadProjectPopup;
    [SerializeField] private GameObject exportAsPopup;

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

        // Hide popups initially
        if (loadProjectPopup != null)
            loadProjectPopup.SetActive(false);

        if (exportAsPopup != null)
            exportAsPopup.SetActive(false);
    }

    private void OnSaveClicked()
    {
        Debug.Log("Save button clicked");

        if (saveLoadManager == null || terrainManager == null)
        {
            Debug.LogError("SaveLoadManager or TerrainManager not found!");
            return;
        }

        // Use default save path for now
        string defaultPath = Application.dataPath + "/../SavedProjects/current_project.json";

        // FIXED: project first, then filePath
        bool success = saveLoadManager.SaveProject(terrainManager.CurrentProject, defaultPath);

        if (success)
            Debug.Log($"Project saved to: {defaultPath}");
        else
            Debug.LogError("Failed to save project!");
    }

    private void OnLoadClicked()
    {
        if (loadProjectPopup != null)
        {
            loadProjectPopup.SetActive(true);

            LoadProjectPopup loadPopup = loadProjectPopup.GetComponent<LoadProjectPopup>();
            if (loadPopup != null)
            {
                loadPopup.RefreshProjectList();
            }
        }
    }

    // Public method to load a project file (called by LoadProjectPopup or other UI)
    public void LoadProjectFile(string filePath)
    {
        if (saveLoadManager == null || terrainManager == null)
        {
            Debug.LogError("SaveLoadManager or TerrainManager not found!");
            return;
        }

        TerrainProject loadedProject = saveLoadManager.LoadProject(filePath);

        if (loadedProject != null)
        {
            // FIXED: use LoadTerrain instead of LoadProject
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
        if (exportAsPopup != null)
        {
            exportAsPopup.SetActive(true);

            // Refresh export popup if needed
            ExportAsPopup exportPopup = exportAsPopup.GetComponent<ExportAsPopup>();
            if (exportPopup != null)
            {
                // Any initialization can go here
            }
        }
    }

    private void OnQuitToMenuClicked()
    {
        // Load menu scene
        SceneManager.LoadScene(menuSceneName);
    }

    private void OnQuitClicked()
    {
        Application.Quit();

        // For testing in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}