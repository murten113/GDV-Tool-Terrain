using TMPro;
using UnityEngine;

public class ProjectTitleDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Settings")]
    [SerializeField] private string defaultTitle = "Untitled Project";
    [SerializeField] private string titleFormat = "{0}";

    private TerrainManager terrainManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if(titleText == null)
            titleText = GetComponentInChildren<TextMeshProUGUI>();


        terrainManager = FindFirstObjectByType<TerrainManager>();

        UpdateTitle();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateTitle();
    }

    private void UpdateTitle()
    {
        if (titleText == null) return;

        string displayName = defaultTitle;

        if (terrainManager != null && terrainManager.CurrentProject != null)
        {
            string projectName = terrainManager.CurrentProject.projectName;
            if (!string.IsNullOrEmpty(projectName))
                displayName = projectName;
        }
        titleText.text = string.Format(titleFormat, displayName);
    }

    public void refreshTitle()
    {
        UpdateTitle();
    }
}
