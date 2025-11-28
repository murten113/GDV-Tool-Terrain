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
    [SerializeField] private GameObject newProjectPopup;
    [SerializeField] private GameObject loadProjectPopup;

    [Header("Scene Settings")]
    [SerializeField] private string mainSceneName = "main";


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

        if (loadProjectPopup != null)
            loadProjectPopup.SetActive(false);
    }


    private void OnNewProjectClicked()
    {
        if (newProjectPopup != null)
            newProjectPopup.SetActive(true);
    }


    private void OnLoadProjectClicked()
    {
        if (loadProjectPopup != null)  // Removed the extra { here
        {
            loadProjectPopup.SetActive(true);
            // Refresh project list when opening (mockup for now)
            LoadProjectPopup loadPopup = loadProjectPopup.GetComponent<LoadProjectPopup>();
            if (loadPopup != null)
            {
                loadPopup.RefreshProjectList();
            }
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