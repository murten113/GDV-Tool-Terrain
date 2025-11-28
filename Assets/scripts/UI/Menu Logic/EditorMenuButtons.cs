using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EditorMenutbuttons : MonoBehaviour
{
    [header("MenuButtons")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exportAsButton;
    [SerializeField] private Button quitToMenuButton;
    [SerializeField] private Button quitButton;

    [Header("Popup Panels")]
    [SerializeField] private GameObject loadProjectPopup;
    [SerializeField] private GameObject exportAsPopup;
    
    [Header("Scene Settings")]
    [SerializeField] private string menuSceneName = "Menu";

    private void Start()
    {
        
    }

}