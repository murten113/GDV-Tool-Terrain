using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    [SerializeField] private Button newProjectButton;
    [SerializeField] private Button loadProjectButton;
    [SerializeField] private Button exitButton;

    [Header("Popup Panels")]
    [SerializeField] private GameObject newProjectPopup; // Keep this for terrain size input

    [Header("Scene Settings")]
    [SerializeField] private string mainSceneName = "main";

    [Header("References")]
    private TerrainSaveLoadManager saveLoadManager;

    private void Start()
    {
        // Add listeners to buttons
        if (newProjectButton != null)
            newProjectButton.onClick.AddListener(OnNewProjectClicked);

        if (loadProjectButton != null)
            loadProjectButton.onClick.AddListener(OnLoadProjectClicked);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitClicked);

        // Hide popups
        if (newProjectPopup != null)
            newProjectPopup.SetActive(false);
    }

    private void OnNewProjectClicked()
    {
        if (newProjectPopup != null)
            newProjectPopup.SetActive(true);
    }

    private void OnLoadProjectClicked()
    {
        // Find SaveLoadManager
        if (saveLoadManager == null)
            saveLoadManager = FindFirstObjectByType<TerrainSaveLoadManager>();

        if (saveLoadManager == null)
        {
            GameObject go = new GameObject("TerrainSaveLoadManager");
            saveLoadManager = go.AddComponent<TerrainSaveLoadManager>();
        }

        // Show load dialog directly
        string filePath = saveLoadManager.ShowLoadDialog();

        if (!string.IsNullOrEmpty(filePath))
        {
            // Store file path in PlayerPrefs for tool scene
            PlayerPrefs.SetString("TerrainEditor_ProjectPath", filePath);
            PlayerPrefs.Save();

            Debug.Log($"Loading project: {filePath}");

            // Load main scene
            SceneManager.LoadScene(mainSceneName);
        }
        else
        {
            Debug.Log("Load cancelled by user");
        }
    }

    private void OnExitClicked()
    {
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}