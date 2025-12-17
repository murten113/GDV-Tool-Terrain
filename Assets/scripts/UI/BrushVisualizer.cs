using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BrushVisualizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BrushManager brushManager;
    [SerializeField] private TerrainRaycaster raycaster;

    [Header("Visual Settings")]
    [SerializeField] private Color brushColor = new Color(0f, 1f, 0f, 0.8f);
    [SerializeField] private int circleSegments = 64;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private float heightOffset = 0.2f;

    private LineRenderer lineRenderer;
    private Vector3 currentBrushPosition;
    private bool showBrush = false;

    private void Awake()
    {
        // Setup LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        ConfigureLineRenderer();
    }

    private void Start()
    {
        if (brushManager == null)
            brushManager = FindFirstObjectByType<BrushManager>();

        if (raycaster == null)
            raycaster = FindFirstObjectByType<TerrainRaycaster>();
    }

    private void ConfigureLineRenderer()
    {
        // Basic settings
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Material setup
        Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineMaterial.color = brushColor;
        lineRenderer.material = lineMaterial;

        // Number of points in circle
        lineRenderer.positionCount = circleSegments;

        // Disable shadows
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
    }

    private void Update()
    {
        // Track mouse position
        if (raycaster != null && raycaster.RaycastTerrain(out Vector3 hitPoint, out Vector3 hitNormal))
        {
            currentBrushPosition = hitPoint + Vector3.up * heightOffset;
            showBrush = true;
            UpdateBrushCircle();
        }
        else
        {
            showBrush = false;
        }

        // Show/hide line renderer
        lineRenderer.enabled = showBrush;
    }

    private void UpdateBrushCircle()
    {
        if (brushManager == null)
            return;

        float brushSize = brushManager.GetBrushSize();
        float angleStep = 360f / circleSegments;

        for (int i = 0; i < circleSegments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 position = currentBrushPosition + new Vector3(
                Mathf.Cos(angle) * brushSize,
                0,
                Mathf.Sin(angle) * brushSize
            );

            lineRenderer.SetPosition(i, position);
        }
    }

    // Optional: Keep gizmo for Scene view as well
    private void OnDrawGizmos()
    {
        if (!showBrush || brushManager == null)
            return;

        Gizmos.color = new Color(brushColor.r, brushColor.g, brushColor.b, 0.3f);
        Gizmos.DrawSphere(currentBrushPosition, 0.2f);
    }
}