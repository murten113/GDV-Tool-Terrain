using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewProjectPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField projectNameInput;
    [SerializeField] private TMP_InputField savePathInput;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button cancelButton;

    [Header("Mockup Settings")]
    [SerializeField] private string defaultProjectName = "New Project";
    [SerializeField] private string defaultSavePath = "C:/Projects/";

    private void Start()
    {
        // Set default values
        if (projectNameInput != null)
            projectNameInput.text = defaultProjectName;

        if (savePathInput != null)
            savePathInput.text = defaultSavePath;

        // Wire up buttons
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);

        // Enable/disable continue button based on input
        if (projectNameInput != null)
            projectNameInput.onValueChanged.AddListener(OnProjectNameChanged);
    }

    private void OnProjectNameChanged(string value)
    {
        if (continueButton != null)
            continueButton.interactable = !string.IsNullOrWhiteSpace(value);
    }

    private void OnContinueClicked()
    {
        string projectName = projectNameInput != null ? projectNameInput.text : defaultProjectName;
        string savePath = savePathInput != null ? savePathInput.text : defaultSavePath;

        // MOCKUP: Just validate inputs (don't actually create files)
        if (string.IsNullOrWhiteSpace(projectName))
        {
            Debug.LogWarning("[MOCKUP] Project name cannot be empty!");
            return;
        }

        // MOCKUP: Just log what would happen
        Debug.Log($"[MOCKUP] Would create new project:");
        Debug.Log($"  Project Name: {projectName}");
        Debug.Log($"  Save Path: {savePath}");
        Debug.Log("[MOCKUP] No actual file creation - just mockup!");

        // Load main scene (this is the only real action)
        MainMenuButtons mainMenu = FindObjectOfType<MainMenuButtons>();
        if (mainMenu != null)
        {
            mainMenu.LoadMainScene();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        }
    }

    private void OnCancelClicked()
    {
        // Close popup
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Reset form when popup opens
        if (projectNameInput != null)
            projectNameInput.text = defaultProjectName;
            
        if (savePathInput != null)
            savePathInput.text = defaultSavePath;
    }
}