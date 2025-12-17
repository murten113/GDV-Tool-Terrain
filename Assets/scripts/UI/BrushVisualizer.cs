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


}
