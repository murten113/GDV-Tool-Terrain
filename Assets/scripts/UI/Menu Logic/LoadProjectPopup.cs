using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadProjectPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button loadButton;
    [SerializeField] private Button cancelButton;

    private TerrainSaveLoadManager saveLoadManager;

    private void Start()
    {
        // Find SaveLoadManager
        FindSaveLoadManager();

        // Button listeners
        if (loadButton != null)
            loadButton.onClick.AddListener(OnLoadClicked);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);
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

    private void OnLoadClicked()
    {
        if (saveLoadManager == null)
        {
            FindSaveLoadManager();
            if (saveLoadManager == null)
            {
                Debug.LogError("SaveLoadManager not found!");
                return;
            }
        }

        // Show file browser dialog
        string filePath = saveLoadManager.ShowLoadDialog();

        if (!string.IsNullOrEmpty(filePath))
        {
            // Store file path in PlayerPrefs for tool scene
            PlayerPrefs.SetString("TerrainEditor_ProjectPath", filePath);
            PlayerPrefs.Save();

            Debug.Log($"Loading project: {filePath}");

            // Load main scene
            SceneManager.LoadScene("main");
        }
        else
        {
            Debug.Log("Load cancelled by user");
        }
    }

    private void OnCancelClicked()
    {
        gameObject.SetActive(false);
    }
}