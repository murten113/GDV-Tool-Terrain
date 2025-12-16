using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class EditorLoadProjectPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button cancelButton;
    [SerializeField] private Transform projectListContainer;
    [SerializeField] private GameObject projectListItemPrefab;

    private List<GameObject> projectListItems = new List<GameObject>();
    private TerrainSaveLoadManager saveLoadManager;
    private EditorMenuButtons editorMenuButtons;

    private void Start()
    {
        // Find managers
        FindManagers();

        // Button listeners
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);

        RefreshProjectList();
    }

    private void OnEnable()
    {
        // Find managers if not already found
        if (saveLoadManager == null || editorMenuButtons == null)
            FindManagers();

        RefreshProjectList();
    }

    private void FindManagers()
    {
        if (saveLoadManager == null)
            saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();

        if (editorMenuButtons == null)
            editorMenuButtons = FindFirstObjectByType<EditorMenuButtons>();
    }

    public void RefreshProjectList()
    {
        ClearProjectList();

        if (projectListContainer == null || projectListItemPrefab == null)
        {
            Debug.LogWarning("Project list container or prefab not assigned");
            return;
        }

        // Ensure we have the manager
        if (saveLoadManager == null)
        {
            saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();

            if (saveLoadManager == null)
            {
                Debug.LogError("SaveLoadManager not found in scene!");
                return;
            }
        }

        // Get all project files
        string[] projectFiles = saveLoadManager.GetAllProjectFiles();

        Debug.Log($"Found {projectFiles.Length} project files");

        if (projectFiles.Length == 0)
        {
            Debug.Log("No saved projects found.");
            return;
        }

        // Create UI items for each project
        foreach (string filePath in projectFiles)
        {
            string projectName = saveLoadManager.GetProjectNameFromPath(filePath);
            CreateProjectListItem(projectName, filePath);
        }
    }

    private void CreateProjectListItem(string projectName, string filePath)
    {
        GameObject listItem = Instantiate(projectListItemPrefab, projectListContainer);
        listItem.name = !string.IsNullOrWhiteSpace(projectName) ? projectName : "ProjectItem";
        projectListItems.Add(listItem);

        Button itemButton = listItem.GetComponent<Button>();
        if (itemButton == null)
            itemButton = listItem.GetComponentInChildren<Button>();

        if (itemButton != null)
            itemButton.onClick.AddListener(() => OnProjectSelected(projectName, filePath));

        TextMeshProUGUI nameText = listItem.GetComponentInChildren<TextMeshProUGUI>();
        if (nameText != null)
            nameText.text = projectName;
    }

    private void OnProjectSelected(string projectName, string filePath)
    {
        Debug.Log($"Loading project: {projectName} from {filePath}");

        // Call EditorMenuButtons to load the project
        if (editorMenuButtons != null)
        {
            editorMenuButtons.LoadProjectFile(filePath);
        }
        else
        {
            Debug.LogError("EditorMenuButtons not found!");
        }

        // Close popup
        gameObject.SetActive(false);
    }

    private void ClearProjectList()
    {
        foreach (GameObject item in projectListItems)
        {
            if (item != null)
                Destroy(item);
        }
        projectListItems.Clear();
    }

    private void OnCancelClicked()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ClearProjectList();
    }
}