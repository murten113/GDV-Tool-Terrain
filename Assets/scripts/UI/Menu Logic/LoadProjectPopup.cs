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
    
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
