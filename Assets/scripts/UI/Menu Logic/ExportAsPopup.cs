using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ExportAsPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField exportPathInput;
    [SerializeField] private TMP_Dropdown formatDropdown;
    [SerializeField] private Button exportButton;
    [SerializeField] private Button cancelButton;

    [Header("Default Settings")]
    [SerializeField] private string defaultExportDirectory = "Exports";

    [Header("References")]
    [SerializeField] private TerrainSaveLoadManager saveLoadManager;
    [SerializeField] private TerrainManager terrainManager;

    private void Start()
    {
        // Find managers if not assigned
        if (saveLoadManager == null)
            saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();

        if (terrainManager == null)
            terrainManager = FindFirstObjectByType<TerrainManager>();

        // Set default export path
        if (exportPathInput != null)
        {
            string defaultPath = Path.Combine(Application.persistentDataPath, defaultExportDirectory);
            exportPathInput.text = defaultPath;
        }

        // Setup format dropdown options
        if (formatDropdown != null)
        {
            formatDropdown.ClearOptions();
            formatDropdown.AddOptions(new System.Collections.Generic.List<string>
            {
                "OBJ (3D Mesh)",
                "PNG (Heightmap)"
            });
            formatDropdown.value = 0;
        }

        // Wire up buttons
        if (exportButton != null)
            exportButton.onClick.AddListener(OnExportClicked);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);
    }

    private void OnExportClicked()
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

        // Get export directory
        string exportDirectory = exportPathInput != null ? exportPathInput.text :
            Path.Combine(Application.persistentDataPath, defaultExportDirectory);

        // Ensure directory exists
        if (!Directory.Exists(exportDirectory))
        {
            Directory.CreateDirectory(exportDirectory);
        }

        // Get selected format
        int formatIndex = formatDropdown != null ? formatDropdown.value : 0;

        // Generate filename
        string projectName = currentProject.projectName.Replace(" ", "_");
        string fileName = "";
        bool success = false;

        switch (formatIndex)
        {
            case 0: // OBJ
                fileName = Path.Combine(exportDirectory, $"{projectName}.obj");
                success = saveLoadManager.ExportAsOBJ(fileName, currentProject);
                break;

            case 1: // PNG Heightmap
                fileName = Path.Combine(exportDirectory, $"{projectName}_heightmap.png");
                success = saveLoadManager.ExportHeightmapPNG(fileName, currentProject);
                break;
        }

        if (success)
        {
            Debug.Log($"Export successful! File saved to: {fileName}");
        }
        else
        {
            Debug.LogError("Export failed!");
        }

        // Close popup
        gameObject.SetActive(false);
    }

    private void OnCancelClicked()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (exportPathInput != null)
        {
            string defaultPath = Path.Combine(Application.persistentDataPath, defaultExportDirectory);
            exportPathInput.text = defaultPath;
        }

        // Reset dropdown to first option
        if (formatDropdown != null)
            formatDropdown.value = 0;
    }
}