using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class LoadProjectPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField directoryPathInput;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Transform projectListContainer;
    [SerializeField] private GameObject projectListItemPrefab;

    private List<GameObject> projectListItems = new List<GameObject>();
    private TerrainSaveLoadManager saveLoadManager;

    private void Start()
    {
        // Find SaveLoadManager
        FindSaveLoadManager();

        // Set the default directory path
        if (directoryPathInput != null && saveLoadManager != null)
        {
            string defaultPath = Path.Combine(Application.persistentDataPath, "Projects");
            directoryPathInput.text = defaultPath;
        }

        // Button listeners
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);

        if (refreshButton != null)
            refreshButton.onClick.AddListener(RefreshProjectList);

        RefreshProjectList();
    }

    private void OnEnable()
    {
        // Find manager if not already found
        if (saveLoadManager == null)
            FindSaveLoadManager();

        RefreshProjectList();
    }

    private void FindSaveLoadManager()
    {
        if (saveLoadManager == null)
        {
            saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();

            if (saveLoadManager == null)
            {
                Debug.Log("Creating TerrainSaveLoadManager in Menu scene");
                GameObject go = new GameObject("TerrainSaveLoadManager");
                saveLoadManager = go.AddComponent<TerrainSaveLoadManager>();
            }
        }
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
            FindSaveLoadManager();

            if (saveLoadManager == null)
            {
                Debug.LogError("SaveLoadManager not found!");
                return;
            }
        }

        // Get all project files from SaveLoadManager
        string[] projectFiles = saveLoadManager.GetAllProjectFiles();

        Debug.Log($"Found {projectFiles.Length} project files");

        if (projectFiles.Length == 0)
        {
            Debug.Log("No saved projects found. Create a new project first!");
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

        // Store load data for transfer to main scene
        ProjectDataTransfer.Instance.SetLoadProjectData(filePath);

        // Load main scene
        MainMenuButtons mainMenu = FindFirstObjectByType<MainMenuButtons>();
        if (mainMenu != null)
        {
            mainMenu.LoadMainScene();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        }
    }

    private void ClearProjectList()
    {
        // Clear all items in list
        foreach (GameObject item in projectListItems)
        {
            if (item != null)
                Destroy(item);
        }
        projectListItems.Clear();
    }

    private void OnCancelClicked()
    {
        // Close popup
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ClearProjectList();
    }
}