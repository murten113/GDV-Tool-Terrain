using UnityEngine;

public class TerrainCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float panSpeed = 0.5f; // Increased for better panning
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 50f;

    [Header("Target")]
    [SerializeField] private Transform target; // Center point to orbit around

    private Vector3 lastMousePosition;
    private float currentZoom = 20f;
    private TerrainManager terrainManager;

    private void Start()
    {
        // Find terrain manager to get terrain center
        terrainManager = FindFirstObjectByType<TerrainManager>();

        // Set target to terrain center if not assigned
        if (target == null)
        {
            GameObject terrainObj = GameObject.Find("TerrainManager");
            if (terrainObj != null)
            {
                target = terrainObj.transform;
            }
            else
            {
                GameObject targetObj = new GameObject("CameraTarget");
                target = targetObj.transform;
                target.position = Vector3.zero;
            }
        }

        // Calculate terrain center based on terrain data
        if (terrainManager != null && terrainManager.CurrentTerrainData != null)
        {
            TerrainData data = terrainManager.CurrentTerrainData;
            // Calculate center position
            float centerX = (data.width * data.horizontalScale) / 2f;
            float centerZ = (data.height * data.horizontalScale) / 2f;
            target.position = new Vector3(centerX, 0, centerZ);
        }

        // Initialize camera position
        if (target != null)
        {
            // Position camera at a good angle
            currentZoom = 30f;
            transform.position = target.position + new Vector3(0, 20, -20);
            transform.LookAt(target);
            currentZoom = Vector3.Distance(transform.position, target.position);
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }
    }

    private void Update()
    {
        HandleRotation();
        HandlePan();
        HandleZoom();
    }

    private void HandleRotation()
    {
        // Right mouse button held = rotate
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            float horizontal = delta.x * rotationSpeed * Time.deltaTime;
            float vertical = -delta.y * rotationSpeed * Time.deltaTime;

            if (target != null)
            {
                transform.RotateAround(target.position, Vector3.up, horizontal);
                transform.RotateAround(target.position, transform.right, vertical);

                // Keep camera looking at target
                Vector3 direction = (transform.position - target.position).normalized;
                transform.LookAt(target);
            }
        }
    }

    private void HandlePan()
    {
        // Middle mouse button or Shift+Left mouse = pan
        bool isPanning = Input.GetMouseButton(2) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift));

        if (isPanning)
        {
            Vector3 delta = lastMousePosition - Input.mousePosition; // Reversed for natural panning

            // Calculate movement in camera's right and up directions
            Vector3 right = transform.right * delta.x * panSpeed;
            Vector3 up = transform.up * delta.y * panSpeed;

            Vector3 movement = right + up;

            // Move both camera and target
            if (target != null)
            {
                transform.position += movement;
                target.position += movement;
            }
            else
            {
                transform.position += movement;
            }
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f && target != null)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            // Move camera closer/further from target while maintaining direction
            Vector3 direction = (transform.position - target.position).normalized;
            if (direction == Vector3.zero)
                direction = -transform.forward;

            transform.position = target.position + direction * currentZoom;
        }
    }

    private void LateUpdate()
    {
        lastMousePosition = Input.mousePosition;
    }
}