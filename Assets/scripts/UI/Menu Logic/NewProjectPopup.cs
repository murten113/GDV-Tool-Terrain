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

    [Header("Terrain Size Settings")]
    [SerializeField] private TMP_InputField terrainWidthInput;
    [SerializeField] private TMP_InputField terrainHeightInput;

    [Header("Default Settings")]
    [SerializeField] private string defaultProjectName = "New Project";
    [SerializeField] private string defaultSavePath = "C:/Projects/";
    [SerializeField] private int defaultTerrainSize = 100;

    private void Start()
    {
        // Set default values
        if (projectNameInput != null)
            projectNameInput.text = defaultProjectName;

        if (savePathInput != null)
            savePathInput.text = defaultSavePath;

        if (terrainWidthInput != null)
            terrainWidthInput.text = defaultTerrainSize.ToString();

        if (terrainHeightInput != null)
            terrainHeightInput.text = defaultTerrainSize.ToString();

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

        // Validate inputs
        if (string.IsNullOrWhiteSpace(projectName))
        {
            Debug.LogWarning("Project name cannot be empty!");
            return;
        }

        // Parse terrain size
        int terrainWidth = defaultTerrainSize;
        int terrainHeight = defaultTerrainSize;

        if (terrainWidthInput != null && int.TryParse(terrainWidthInput.text, out int width))
            terrainWidth = Mathf.Clamp(width, 10, 500);

        if (terrainHeightInput != null && int.TryParse(terrainHeightInput.text, out int height))
            terrainHeight = Mathf.Clamp(height, 10, 500);

        // Store project data for transfer to main scene
        ProjectDataTransfer.Instance.SetNewProjectData(projectName, terrainWidth, terrainHeight);

        Debug.Log($"Creating new project: {projectName} ({terrainWidth}x{terrainHeight})");

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

        if (terrainWidthInput != null)
            terrainWidthInput.text = defaultTerrainSize.ToString();

        if (terrainHeightInput != null)
            terrainHeightInput.text = defaultTerrainSize.ToString();
    }
}