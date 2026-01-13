using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExportAsPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Dropdown formatDropdown;
    [SerializeField] private Button exportButton;
    [SerializeField] private Button cancelButton;

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

        // Get selected format
        int formatIndex = formatDropdown != null ? formatDropdown.value : 0;

        // Determine extension and default name
        string extension = formatIndex == 0 ? "obj" : "png";
        string defaultName = currentProject.projectName.Replace(" ", "_");
        if (formatIndex == 1)
            defaultName += "_heightmap";

        // Show file browser dialog
        string filePath = saveLoadManager.ShowExportDialog(defaultName, extension);

        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Export cancelled by user");
            return;
        }

        // Perform export
        bool success = false;
        if (formatIndex == 0) // OBJ
        {
            success = saveLoadManager.ExportAsOBJ(filePath, currentProject);
        }
        else // PNG
        {
            success = saveLoadManager.ExportHeightmapPNG(filePath, currentProject);
        }

        if (success)
        {
            Debug.Log($"Export successful! File saved to: {filePath}");
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
        // Reset dropdown to first option
        if (formatDropdown != null)
            formatDropdown.value = 0;
    }
}