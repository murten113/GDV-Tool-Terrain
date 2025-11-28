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

}
