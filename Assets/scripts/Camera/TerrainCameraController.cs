using UnityEngine;

public class TerrainCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float panSpeed = 0.5f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 50f;

    [Header("Target")]
    [SerializeField] private Transform target; // Center point to orbit around

    private Vector3 lastMousePosition;
    private float currentZoom = 20f;
    private TerrainManager terrainManager;
    private Vector3 terrainCenter; // Store terrain center position

    private void Start()
    {
        // Find terrain manager to get terrain center
        terrainManager = FindFirstObjectByType<TerrainManager>();

        // Calculate terrain center based on terrain data
        if (terrainManager != null && terrainManager.CurrentTerrainData != null)
        {
            TerrainData data = terrainManager.CurrentTerrainData;
            // Calculate center position
            float centerX = (data.width * data.horizontalScale) / 2f;
            float centerZ = (data.height * data.horizontalScale) / 2f;
            terrainCenter = new Vector3(centerX, 0, centerZ);
        }
        else
        {
            terrainCenter = Vector3.zero;
        }

        // Set target to terrain center if not assigned
        if (target == null)
        {
            GameObject targetObj = new GameObject("CameraTarget");
            target = targetObj.transform;
        }

        // Set target position to terrain center
        target.position = terrainCenter;

        // Initialize camera position
        currentZoom = 30f;
        transform.position = terrainCenter + new Vector3(0, 20, -20);
        transform.LookAt(terrainCenter);
        currentZoom = Vector3.Distance(transform.position, terrainCenter);
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
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

            // Rotate around terrain center
            transform.RotateAround(terrainCenter, Vector3.up, horizontal);
            transform.RotateAround(terrainCenter, transform.right, vertical);

            // Keep camera looking at terrain center
            transform.LookAt(terrainCenter);

            // Update zoom distance
            currentZoom = Vector3.Distance(transform.position, terrainCenter);
        }
    }

    private void HandlePan()
    {
        // Middle mouse button or Shift+Left mouse = pan
        bool isPanning = Input.GetMouseButton(2) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift));

        if (isPanning)
        {
            Vector3 delta = lastMousePosition - Input.mousePosition; // Reversed for natural panning

            // Calculate movement in camera's right and up directions (on XZ plane)
            Vector3 right = transform.right;
            right.y = 0; // Keep panning horizontal
            right.Normalize();

            Vector3 up = transform.up;
            up.y = 0; // Keep panning horizontal
            up.Normalize();

            Vector3 movement = (right * delta.x + up * delta.y) * panSpeed;

            // Move ONLY the terrain center (not the target GameObject)
            terrainCenter += movement;

            // Update camera position to maintain distance and look at new center
            Vector3 direction = (transform.position - terrainCenter).normalized;
            if (direction == Vector3.zero)
                direction = -transform.forward;
            transform.position = terrainCenter + direction * currentZoom;
            transform.LookAt(terrainCenter);

            // Also update target GameObject position for reference
            if (target != null)
                target.position = terrainCenter;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            // Move camera closer/further from terrain center
            Vector3 direction = (transform.position - terrainCenter).normalized;
            if (direction == Vector3.zero)
                direction = -transform.forward;

            transform.position = terrainCenter + direction * currentZoom;
            transform.LookAt(terrainCenter);
        }
    }

    private void LateUpdate()
    {
        lastMousePosition = Input.mousePosition;
    }
}