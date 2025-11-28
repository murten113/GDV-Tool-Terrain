using UnityEngine;

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
        if (newProjectButton != null){
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

    }

    private void OnLoadProjectClicked()
    {

    }

}
