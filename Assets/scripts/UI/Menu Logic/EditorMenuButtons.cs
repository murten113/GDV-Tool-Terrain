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

    private void Start()
    {
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
        // MOCKUP: Just log what would happen
        Debug.Log("[MOCKUP] Save button clicked");
        Debug.Log("[MOCKUP] Would save current project to: [Current Project Path]");
        Debug.Log("[MOCKUP] No actual file saving - just mockup!");

        // TODO: In real implementation, call save function
        // TerrainSaveLoadManager.Instance.SaveProject(currentProjectPath);
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