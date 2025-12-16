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
}
