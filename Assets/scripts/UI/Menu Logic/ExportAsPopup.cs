using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExportAsPopup : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private TMP_InputField exportPathInput;
    [SerializeField] private TMP_Dropdown formatDropdown;
    [SerializeField] private Button exportButton;
    [SerializeField] private Button cancelButton;

    [Header("Mockup Settings")]
    [SerializeField] private string defaultExportPath = "C:/Exports/";

    private void Start()
    {
        // Set default export path
        if (exportPathInput != null)
            exportPathInput.text = defaultExportPath;

        // Wire up buttons
        if (exportButton != null)
            exportButton.onClick.AddListener(OnExportClicked);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);  
    }

    private void OnExportClicked()
    {
        string exportPath = exportPathInput != null ? exportPathInput.text : defaultExportPath;

        // MOCKUP: Just log what would happen
        Debug.Log("[MOCKUP] Export As clicked");
        Debug.Log($"  Export Path: {exportPath}");
        Debug.Log("[MOCKUP] No actual export - just mockup!");

        // TODO: In real implementation, export terrain here
        // TerrainSaveLoadManager.Instance.ExportTerrain(exportPath, format);

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
            exportPathInput.text = defaultExportPath;
    }

}
