using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewProjectPopup : MonoBehaviour
{
    public event Action<NewProjectRequest> ProjectCreateRequested;
    public event Action CancelRequested;

    [Header("UI References")]
    [SerializeField] private TMP_InputField projectNameInput;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button cancelButton;

    [Header("Terrain Size Settings")]
    [SerializeField] private TMP_InputField terrainWidthInput;
    [SerializeField] private TMP_InputField terrainHeightInput;

    [Header("Default Settings")]
    [SerializeField] private string defaultProjectName = "New Project";
    [SerializeField] private int defaultTerrainSize = 100;

    private void Start()
    {
        ResetForm();

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);

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
        if (string.IsNullOrWhiteSpace(projectName))
        {
            Debug.LogWarning("Project name cannot be empty!");
            return;
        }

        int terrainWidth = defaultTerrainSize;
        int terrainHeight = defaultTerrainSize;

        if (terrainWidthInput != null && int.TryParse(terrainWidthInput.text, out int width))
            terrainWidth = Mathf.Clamp(width, 10, 500);

        if (terrainHeightInput != null && int.TryParse(terrainHeightInput.text, out int height))
            terrainHeight = Mathf.Clamp(height, 10, 500);

        var request = new NewProjectRequest
        {
            ProjectName = projectName,
            TerrainWidth = terrainWidth,
            TerrainHeight = terrainHeight
        };

        ProjectCreateRequested?.Invoke(request);
    }

    private void OnCancelClicked()
    {
        CancelRequested?.Invoke();
    }

    private void OnEnable()
    {
        ResetForm();
    }

    private void ResetForm()
    {
        if (projectNameInput != null)
            projectNameInput.text = defaultProjectName;

        if (terrainWidthInput != null)
            terrainWidthInput.text = defaultTerrainSize.ToString();

        if (terrainHeightInput != null)
            terrainHeightInput.text = defaultTerrainSize.ToString();
    }
}
