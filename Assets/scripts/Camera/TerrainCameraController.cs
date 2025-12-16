using UnityEngine;

public class TerrainCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 50f;

    [Header("Pan Limits")]
    [SerializeField] private bool usePanLimits = false;
    [SerializeField] private float panLimitX = 100f;
    [SerializeField] private float panLimitZ = 100f;

    private Vector3 lastMousePosition;
    private float currentZoom = 30f;
    private TerrainManager terrainManager;

    private void Start()
    {
        // Find terrain manager
        terrainManager = FindFirstObjectByType<TerrainManager>();

        // Calculate terrain center
        Vector3 terrainCenter = Vector3.zero;
        if (terrainManager != null && terrainManager.CurrentTerrainData != null)
        {
            TerrainData data = terrainManager.CurrentTerrainData;
            float centerX = (data.width * data.horizontalScale) / 2f;
            float centerZ = (data.height * data.horizontalScale) / 2f;
            terrainCenter = new Vector3(centerX, 0, centerZ);
        }

        // Initialize camera position (looking down at terrain at angle)
        currentZoom = 30f;
        transform.position = terrainCenter + new Vector3(0, currentZoom, -currentZoom * 0.5f);
        transform.rotation = Quaternion.Euler(45f, 0f, 0f); // 45 degree angle looking down

        // Update pan limits based on terrain size
        if (terrainManager != null && terrainManager.CurrentTerrainData != null)
        {
            TerrainData data = terrainManager.CurrentTerrainData;
            panLimitX = data.width * data.horizontalScale;
            panLimitZ = data.height * data.horizontalScale;
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
        // Right mouse button held = rotate around Y axis only
        if (Input.GetMouseButton(1))
        {
            float deltaX = Input.mousePosition.x - lastMousePosition.x;
            float rotationAmount = deltaX * rotationSpeed * Time.deltaTime;

            // Rotate only around Y axis (horizontal rotation)
            transform.Rotate(0, rotationAmount, 0, Space.World);
        }
    }

    private void HandlePan()
    {
        // Middle mouse button or Shift+Left mouse = pan (RTS style - horizontal only)
        bool isPanning = Input.GetMouseButton(2) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift));

        if (isPanning)
        {
            Vector3 delta = lastMousePosition - Input.mousePosition; // Reversed for natural panning

            // Get camera's forward and right directions (projected onto XZ plane - horizontal only)
            Vector3 forward = transform.forward;
            forward.y = 0; // Remove vertical component
            forward.Normalize();

            Vector3 right = transform.right;
            right.y = 0; // Remove vertical component
            right.Normalize();

            // Calculate pan movement (only horizontal)
            Vector3 panMovement = (right * delta.x + forward * delta.y) * panSpeed * Time.deltaTime;

            // Apply panning
            Vector3 newPosition = transform.position + panMovement;

            // Apply limits if enabled
            if (usePanLimits)
            {
                newPosition.x = Mathf.Clamp(newPosition.x, -panLimitX, panLimitX);
                newPosition.z = Mathf.Clamp(newPosition.z, -panLimitZ, panLimitZ);
            }

            transform.position = newPosition;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            // Zoom by moving camera forward/backward along its forward direction
            Vector3 zoomDirection = transform.forward;
            float zoomAmount = scroll * zoomSpeed;

            Vector3 newPosition = transform.position + zoomDirection * zoomAmount;

            // Clamp zoom distance
            float distanceFromOrigin = newPosition.magnitude;
            if (distanceFromOrigin >= minZoom && distanceFromOrigin <= maxZoom)
            {
                transform.position = newPosition;
                currentZoom = distanceFromOrigin;
            }
        }
    }

    private void LateUpdate()
    {
        lastMousePosition = Input.mousePosition;
    }
}