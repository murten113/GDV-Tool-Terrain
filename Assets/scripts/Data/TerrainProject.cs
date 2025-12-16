using System;
using UnityEngine;


[Serializable]
public class TerrainProject : MonoBehaviour
{
    [Header("Project Metadata")]
    public string projectName = "New Project";
    public string version = "1.0";
    public string creationDate;
    public string lastModifiedDate;

    [Header("Terrain Data")]
    public TerrainData terrainData;

    public TerrainProject()
    {
        projectName = "New Project";
        version = "1.0";
        creationDate = System.DateTime.Now.ToString();
        lastModifiedDate = creationDate;
        terrainData = new TerrainData();
    }

    public TerrainProject(string name, TerrainData data)
    {
        projectName = name;
        version = "1.0";
        creationDate = System.DateTime.Now.ToString();
        lastModifiedDate = creationDate;
        terrainData = data;
    }

    public void UpdateLastModified()
    {
        lastModifiedDate = System.DateTime.Now.ToString();
    }
}
