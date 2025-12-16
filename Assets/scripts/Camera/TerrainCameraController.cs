using UnityEngine;

public class TerrainCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float zoomSpeed = 500f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 50f;

    [Header("Target")]
    [SerializeField] private Transform target; //Center point to orbit around

    private Vector3 lastMousePosition;
    private float currentZoom = 20f;

    private void Start()
    {

        // Find or create target if not assigned
        if (target == null)
        {
            GameObject terrainObj = GameObject.Find("TerrainManager");
            if (terrainObj != null)
            {
                target = terrainObj.transform;
            }
            else
            {
                target = new GameObject("CameraTarget").transform;
                target.position = Vector3.zero;
            }
        }

        // Initialize camera position
        transform.LookAt(target);
        currentZoom = Vector3.Distance(transform.position, target.position);
    }

}
