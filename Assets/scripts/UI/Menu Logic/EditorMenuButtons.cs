using System;
using UnityEngine;
using UnityEngine.UI;

public class EditorMenuButtons : MonoBehaviour
{
    public event Action SaveRequested;
    public event Action LoadRequested;
    public event Action ExportRequested;
    public event Action QuitToMenuRequested;
    public event Action QuitRequested;

    [Header("Menu Buttons")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exportAsButton;
    [SerializeField] private Button quitToMenuButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        if (saveButton != null)
            saveButton.onClick.AddListener(() => SaveRequested?.Invoke());

        if (loadButton != null)
            loadButton.onClick.AddListener(() => LoadRequested?.Invoke());

        if (exportAsButton != null)
            exportAsButton.onClick.AddListener(() => ExportRequested?.Invoke());

        if (quitToMenuButton != null)
            quitToMenuButton.onClick.AddListener(() => QuitToMenuRequested?.Invoke());

        if (quitButton != null)
            quitButton.onClick.AddListener(() => QuitRequested?.Invoke());
    }
}
