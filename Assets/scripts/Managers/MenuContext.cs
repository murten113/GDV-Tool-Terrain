using System;
using UnityEngine;

public struct NewProjectRequest
{
    public string ProjectName;
    public int TerrainWidth;
    public int TerrainHeight;
}

public class MenuContext : MonoBehaviour
{
    public const string ProjectPathPrefKey = "TerrainEditor_ProjectPath";

    [Header("Scene")]
    [SerializeField] private string mainSceneName = "main";

    [Header("UI")]
    [SerializeField] private MainMenuButtons mainMenuButtons;
    [SerializeField] private NewProjectPopup newProjectPopup;
    [SerializeField] private GameObject newProjectPopupRoot;

    [Header("Systems")]
    [SerializeField] private TerrainSaveLoadManager saveLoadManager;

    private void Awake()
    {
        if (mainMenuButtons == null)
            mainMenuButtons = FindFirstObjectByType<MainMenuButtons>();

        if (newProjectPopup == null)
            newProjectPopup = FindFirstObjectByType<NewProjectPopup>();

        if (newProjectPopupRoot == null && newProjectPopup != null)
            newProjectPopupRoot = newProjectPopup.gameObject;

        if (saveLoadManager == null)
            saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();

        if (saveLoadManager == null)
        {
            var go = new GameObject("TerrainSaveLoadManager");
            saveLoadManager = go.AddComponent<TerrainSaveLoadManager>();
        }
    }

    private void Start()
    {
        if (newProjectPopupRoot != null)
            newProjectPopupRoot.SetActive(false);

        if (mainMenuButtons != null)
        {
            mainMenuButtons.NewProjectRequested += HandleNewProjectRequested;
            mainMenuButtons.LoadProjectRequested += HandleLoadProjectRequested;
            mainMenuButtons.ExitRequested += HandleExitRequested;
        }

        if (newProjectPopup != null)
        {
            newProjectPopup.ProjectCreateRequested += HandleProjectCreateRequested;
            newProjectPopup.CancelRequested += HandleNewProjectCancelled;
        }
    }

    private void OnDestroy()
    {
        if (mainMenuButtons != null)
        {
            mainMenuButtons.NewProjectRequested -= HandleNewProjectRequested;
            mainMenuButtons.LoadProjectRequested -= HandleLoadProjectRequested;
            mainMenuButtons.ExitRequested -= HandleExitRequested;
        }

        if (newProjectPopup != null)
        {
            newProjectPopup.ProjectCreateRequested -= HandleProjectCreateRequested;
            newProjectPopup.CancelRequested -= HandleNewProjectCancelled;
        }
    }

    private void HandleNewProjectRequested()
    {
        if (newProjectPopupRoot != null)
            newProjectPopupRoot.SetActive(true);
    }

    private void HandleLoadProjectRequested()
    {
        string filePath = saveLoadManager.ShowLoadDialog();
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Load cancelled by user");
            return;
        }

        PlayerPrefs.SetString(ProjectPathPrefKey, filePath);
        PlayerPrefs.Save();

        Debug.Log($"Loading project: {filePath}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainSceneName);
    }

    private void HandleProjectCreateRequested(NewProjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProjectName))
        {
            Debug.LogWarning("Project name cannot be empty!");
            return;
        }

        var terrainData = new TerrainData();
        terrainData.Initialize(request.TerrainWidth, request.TerrainHeight);

        var project = new TerrainProject();
        project.Initialize(request.ProjectName, terrainData);

        string filePath = saveLoadManager.ShowSaveDialog(request.ProjectName);
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Save cancelled by user");
            return;
        }

        if (!saveLoadManager.SaveProject(project, filePath))
        {
            Debug.LogError("Failed to save project!");
            return;
        }

        PlayerPrefs.SetString(ProjectPathPrefKey, filePath);
        PlayerPrefs.Save();

        Debug.Log($"Project created and saved: {filePath}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainSceneName);
    }

    private void HandleNewProjectCancelled()
    {
        if (newProjectPopupRoot != null)
            newProjectPopupRoot.SetActive(false);
    }

    private void HandleExitRequested()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
