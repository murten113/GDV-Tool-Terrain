using UnityEngine;
using UnityEngine.Rendering.UI;

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

    private void Update()
    {
        HandleRotation();
        HandlePan();
        HandleZoom();
    }

    private void HandleRotation()
    {
        // Right mouse button = rotation
        if (Input.GetMouseButton(1)) 
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            float horizontal = delta.x * rotationSpeed * Time.deltaTime;
            float vertical = -delta.y * rotationSpeed * Time.deltaTime;

            transform.RotateAround(target.position, Vector3.up, horizontal);
            transform.RotateAround(target.position, transform.right, vertical);

            transform.LookAt(target);
        }
    }

    private void HandlePan()
    {
        // Middle mouse button = pan
        if (Input.GetMouseButton(2) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift)))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            Vector3 right = transform.right * delta.x * panSpeed * Time.deltaTime * 0.01f;
            Vector3 up = transform.up * delta.y * panSpeed * Time.deltaTime * 0.01f;

            Vector3 movement = right + up;
            transform.position -= movement;
            target.position -= movement;
        }
    }



    private void HandleZoom()
    {
        // Mouse scroll wheel = zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            // Update camera position based on zoom
            Vector3 direction = (transform.position - target.position).normalized;
            transform.position = target.position + direction * currentZoom;
        }
    }

    private void LateUpdate()
    {
        lastMousePosition = Input.mousePosition;
    }
}
