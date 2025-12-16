using UnityEngine;

public class TerrainCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 50f;

    [Header("Target")]
    [SerializeField] private Transform target; // Center point to orbit around

    private Vector3 lastMousePosition;
    private float currentZoom = 20f;

    private void Start()
    {
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

        // Initialize camera position
        if (target != null)
        {
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
                transform.LookAt(target);
            }
        }
    }

    private void HandlePan()
    {
        // Middle mouse button or Shift+Left mouse = pan
        bool isPanning = Input.GetMouseButton(2) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift));

        if (isPanning && target != null)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Calculate movement in camera space
            Vector3 right = transform.right * delta.x * panSpeed * Time.deltaTime * 0.1f;
            Vector3 forward = transform.forward;
            forward.y = 0; // Keep panning horizontal
            forward.Normalize();
            Vector3 upMovement = transform.up * delta.y * panSpeed * Time.deltaTime * 0.1f;

            Vector3 movement = right + upMovement;
            transform.position -= movement;
            target.position -= movement;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f && target != null)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            // Move camera closer/further from target
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