using System;
using System.Collections.Generic;
using UnityEngine;

public class BrushManager : MonoBehaviour
{
    [Header("Brushes")]
    [SerializeField] private List<TerrainBrush> brushes = new List<TerrainBrush>();

    [Header("References")]
    [SerializeField] private TerrainPainter terrainPainter;

    [Header("Settings")]
    [SerializeField] private float brushSize = 5f;
    [SerializeField] private float brushStrength = 1f;

    [Header("Scroll Hotkeys")]
    [SerializeField] private float sizeScrollSpeed = 2f;
    [SerializeField] private float strengthScrollSpeed = 0.25f;
    [SerializeField] private float minBrushSize = 0.5f;
    [SerializeField] private float maxBrushSize = 50f;
    [SerializeField] private float minBrushStrength = 0.1f;
    [SerializeField] private float maxBrushStrength = 10f;

    public Action<float> OnBrushSizeChanged;
    public Action<float> OnBrushStrengthChanged;
    public Action<int, TerrainBrush> OnActiveBrushChanged;

    private int activeBrushIndex;
    private TerrainRaycaster raycaster;
    private TerrainManager terrainManager;

    public IReadOnlyList<TerrainBrush> Brushes => brushes;
    public int ActiveBrushIndex => activeBrushIndex;

    private void Awake()
    {
        if (terrainPainter == null)
            terrainPainter = GetComponent<TerrainPainter>();

        if (terrainPainter == null)
            terrainPainter = FindFirstObjectByType<TerrainPainter>();
    }

    private void Start()
    {
        raycaster = FindFirstObjectByType<TerrainRaycaster>();
        terrainManager = FindFirstObjectByType<TerrainManager>();

        if (raycaster == null)
            Debug.LogError("BrushManager: TerrainRaycaster not found!");

        if (terrainPainter == null)
            Debug.LogError("BrushManager: TerrainPainter not found!");

        if (brushes.Count > 0)
            SetActiveBrush(0);
    }

    public void SetActiveBrush(int index)
    {
        if (index < 0 || index >= brushes.Count)
        {
            Debug.LogError($"Brush index {index} is out of range!");
            return;
        }

        activeBrushIndex = index;
        Debug.Log($"Active brush: {brushes[index].BrushName}");
        OnActiveBrushChanged?.Invoke(activeBrushIndex, brushes[activeBrushIndex]);
    }

    public TerrainBrush GetActiveBrush()
    {
        if (brushes.Count == 0 || activeBrushIndex < 0 || activeBrushIndex >= brushes.Count)
            return null;

        return brushes[activeBrushIndex];
    }

    public void SetActiveBrush(TerrainBrush brush)
    {
        int index = brushes.IndexOf(brush);
        if (index < 0)
        {
            Debug.LogError($"Brush {brush.name} is not registered in the brush list!");
            return;
        }

        SetActiveBrush(index);
    }

    private void Update()
    {
        HandleBrushHotkeys();
        HandleScrollWheelAdjustments();

        if (Input.GetMouseButtonDown(0) && !UIInputUtility.IsPointerOverUI())
            BeginActiveStroke();

        if (Input.GetMouseButton(0) && !UIInputUtility.IsPointerOverUI())
            Paint();

        if (Input.GetMouseButtonUp(0))
            EndActiveStroke();
    }

    private void HandleBrushHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && brushes.Count > 0) SetActiveBrush(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && brushes.Count > 1) SetActiveBrush(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && brushes.Count > 2) SetActiveBrush(2);
        if (Input.GetKeyDown(KeyCode.Alpha4) && brushes.Count > 3) SetActiveBrush(3);
        if (Input.GetKeyDown(KeyCode.Alpha5) && brushes.Count > 4) SetActiveBrush(4);
    }

    private void BeginActiveStroke()
    {
        if (raycaster == null || terrainManager == null || brushes.Count == 0)
            return;

        if (!raycaster.RaycastTerrain(out Vector3 hitPoint, out _))
            return;

        TerrainData data = terrainManager.CurrentTerrainData;
        if (data == null)
            return;

        int centerX = Mathf.FloorToInt(hitPoint.x / data.horizontalScale);
        int centerY = Mathf.FloorToInt(hitPoint.z / data.horizontalScale);
        brushes[activeBrushIndex].BeginStroke(data, centerX, centerY);
    }

    private void EndActiveStroke()
    {
        if (brushes.Count == 0)
            return;

        brushes[activeBrushIndex].EndStroke();
    }

    private void HandleScrollWheelAdjustments()
    {
        if (UIInputUtility.IsPointerOverUI())
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.01f)
            return;

        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool ctrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (shiftHeld && !ctrlHeld)
        {
            float newSize = brushSize + scroll * sizeScrollSpeed;
            SetBrushSize(Mathf.Clamp(newSize, minBrushSize, maxBrushSize));
        }
        else if (ctrlHeld && !shiftHeld)
        {
            float newStrength = brushStrength + scroll * strengthScrollSpeed;
            SetBrushStrength(Mathf.Clamp(newStrength, minBrushStrength, maxBrushStrength));
        }
    }

    private void Paint()
    {
        if (raycaster == null || terrainPainter == null || brushes.Count == 0)
            return;

        ITerrainBrush activeBrush = brushes[activeBrushIndex];
        if (activeBrush == null)
            return;

        if (raycaster.RaycastTerrain(out Vector3 hitPoint, out _))
            terrainPainter.ApplyBrush(activeBrush, hitPoint, brushSize, brushStrength);
    }

    public void SetBrushSize(float size)
    {
        brushSize = size;
        OnBrushSizeChanged?.Invoke(brushSize);
    }

    public void SetBrushStrength(float strength)
    {
        brushStrength = strength;
        OnBrushStrengthChanged?.Invoke(strength);
    }

    public float GetBrushSize() => brushSize;
    public float GetBrushStrength() => brushStrength;
}
