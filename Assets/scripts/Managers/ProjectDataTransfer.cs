using UnityEngine;

public class ProjectDataTransfer : MonoBehaviour
{
    private static ProjectDataTransfer instance;
    public static ProjectDataTransfer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ProjectDataTransfer");
                instance = go.AddComponent<ProjectDataTransfer>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public string ProjectName = "New Project";
    public int terrainWidth = 100;
    public int terrainHeight = 100;
    public bool isNewProject = true;
    public string loadFilePath = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetNewProjectData(string name, int width = 100, int height = 100)
    {
        ProjectName = name;
        terrainWidth = width;
        terrainHeight = height;
        isNewProject = true;
        loadFilePath = "";
    }

    public void SetLoadProjectData(string filePath)
    {
        loadFilePath = filePath;
        isNewProject = false;
    }

    public void ClearData()
    {
        ProjectName = "New Project";
        terrainWidth = 100;
        terrainHeight = 100;
        isNewProject = true;
        loadFilePath = "";
    }
}
