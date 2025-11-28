using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LoadProjectPopup : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private TMP_InputField directoryPathInput;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Transform projectListContainer;
    [SerializeField] private GameObject projectListItemPrefab;

    [Header("Mockup Settings")]
    [SerializeField] private string defaultDirectoryPath = "C:/Projects/";
    
    // MOCKUP: Hardcoded project list - not reading from disk
    [SerializeField] private List<string> mockProjectNames = new List<string>
    {
        "City Project 1",
        "Mountain Terrain",
        "Desert Landscape",
        "Island Paradise",
        "Urban District"
    };
    
    private list<gameobject> projectListItems; = new list<gameobject>();

    private void Start()
    {
        //set the default directory path
        if (directoryPathInput != null)
            directoryPathInput.text = defaultDirectoryPath;

        //Button listeners
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClicked);

        if (refreshButton != null)
            refreshButton.onClick.AddListener(RefreshProjectList);

        RefreshProjectList();
    }

    public void RefreshProjectList()
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
