using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public event Action NewProjectRequested;
    public event Action LoadProjectRequested;
    public event Action ExitRequested;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button newProjectButton;
    [SerializeField] private Button loadProjectButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        if (newProjectButton != null)
            newProjectButton.onClick.AddListener(() => NewProjectRequested?.Invoke());

        if (loadProjectButton != null)
            loadProjectButton.onClick.AddListener(() => LoadProjectRequested?.Invoke());

        if (exitButton != null)
            exitButton.onClick.AddListener(() => ExitRequested?.Invoke());
    }
}
