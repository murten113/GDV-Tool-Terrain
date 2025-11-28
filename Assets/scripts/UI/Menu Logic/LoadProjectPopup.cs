using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LoadProjectPopup : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private TMP_InputField directoryPathInput;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Transform projectListContainer;
    [SerializeField] private GameObject projectListItemPrefab;

    [Header("Mockup Settings")]
    [SerializeField] private string defaultDirectoryPath = "C:/Projects/";
    
    // MOCKUP: Hardcoded project list - not reading from disk
    [SerializeField] private List<string> mockProjectNames = new List<string>
    {
        "City Project 1",
        "Mountain Terrain",
        "Desert Landscape",
        "Island Paradise",
        "Urban District"
    };
    
    private List<GameObject> projectListItems = new List<GameObject>();  // Fixed: List, GameObject, removed semicolon before =

    private void Start()
    {
        //set the default directory path
        if (directoryPathInput != null)
            directoryPathInput.text = defaultDirectoryPath;

        //Button listeners
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);

        if (refreshButton != null)
            refreshButton.onClick.AddListener(RefreshProjectList);

        RefreshProjectList();
    }

    public void RefreshProjectList()
    {
        ClearProjectList();

        //Get directory path from input field (not working yet)
        string directoryPath = directoryPathInput != null ? directoryPathInput.text : defaultDirectoryPath;
        
        Debug.Log($"[MOCKUP] scan direct: {directoryPath}");
        Debug.Log("[MOCKUP] not reading disk using hardcoded list");  // Fixed: Added semicolon

        if (projectListContainer == null || projectListItemPrefab == null)
        {
            Debug.Log("Proj list cont/pref not assigned");  // Fixed: Added semicolon
            return;
        }

        //create project item [MOCKUP]
        foreach (string projectName in mockProjectNames)
        {
            CreateProjectListItem(projectName);
        }

    }

    private void CreateProjectListItem(string projectName)
    {
        GameObject listItem = Instantiate(projectListItemPrefab, projectListContainer);
        projectListItems.Add(listItem);

        Button itemButton = listItem.GetComponent<Button>();  // Fixed: GetComponent (capital C)
        if (itemButton == null)
            itemButton = listItem.GetComponentInChildren<Button>();

        if (itemButton != null)
            itemButton.onClick.AddListener(() => OnProjectSelected(projectName));  // Fixed: renamed from OnNewProjectSelected

        TextMeshProUGUI nameText = listItem.GetComponentInChildren<TextMeshProUGUI>();
        if (nameText != null)
            nameText.text = projectName;
    }
    
    private void OnProjectSelected(string projectName)  // Fixed: OnProjectSelected, string (was strng)
    {
        string directoryPath = directoryPathInput != null ? directoryPathInput.text : defaultDirectoryPath;


        //MOCK
        Debug.Log($"[MOCKUP] Would load project: {projectName}");
        Debug.Log($"  From directory: {directoryPath}");
        Debug.Log("[MOCKUP] No actual file loading - just mockup!");

        //load Main scene
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
        //clear all items in list
        foreach (GameObject item in projectListItems)  // Fixed: item (was iten)
        {
            if (item != null)  // Fixed: item (was referencing undeclared variable)
                Destroy(item);
        }
        projectListItems.Clear();  // Fixed: Added parentheses ()
    }

    private void OnCancelClicked()
    {
        //close popup
        gameObject.SetActive(false);  // Fixed: gameObject (capital O)
    }

    private void OnDestroy()
    {
        ClearProjectList();
    }
}