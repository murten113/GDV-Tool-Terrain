using UnityEngine;

public class BrushVisualizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BrushManager brushManager;
    [SerializeField] private TerrainRaycaster raycaster;

    [Header("Visual Settings")]
    [SerializeField] private Color brushColor = new Color(0f, 1f, 0f, 0.5f);
    [SerializeField] private int circleSegments = 32;
    [SerializeField] private float circleHeight = 0.1f;

    private Vector3 currentBrushPosition;
    private bool showBrush = false;


    private void Start()
    {
        if (brushManager == null)
            brushManager = FindFirstObjectByType<BrushManager>();

        if (raycaster == null)
            raycaster = FindFirstObjectByType<TerrainRaycaster>();

    }

    private void Update()
    {
        if (raycaster != null && raycaster.RaycastTerrain(out Vector3 hitPoint, out Vector3 hitNormal))
        {
            currentBrushPosition = hitPoint + Vector3.up * circleHeight;
            showBrush = true;
        }
        else
        {
            showBrush = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showBrush || brushManager == null)
            return;

        float brushSize = brushManager.GetBrushSize();

        Gizmos.color = brushColor;

        DrawCircle(currentBrushPosition, brushSize, circleSegments);

        Gizmos.DrawSphere(currentBrushPosition, 0.2f);
    }

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0f, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }

    }
}
