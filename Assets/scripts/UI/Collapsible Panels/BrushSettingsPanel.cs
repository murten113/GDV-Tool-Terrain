using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BrushSettingsPanel : CollapsiblePanel
{
    [Header("Brush Settings Specific")]
    [SerializeField] private float panelHeight = 180f;

    [Header("UI References")]
    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] private Slider brushStrengthSlider;
    [SerializeField] private TextMeshProUGUI brushSieText;
    [SerializeField] private TextMeshProUGUI brushStrengthText;

    [Header("References")]
    [SerializeField] private BrushManager brushManager;

    [Header("Settings")]
    [SerializeField] private float minBrushSize = 1f;
    [SerializeField] private float maxBrushSize = 20f;
    [SerializeField] private float minBrushStrength = 0.1f;
    [SerializeField] private float maxBrushStrength = 5f;

    protected override void SetupCollapsedPosition()
    {
        base.SetupCollapsedPosition();

        // Collapsed: hide panel below (off-screen)
        collapsedPosition = new Vector2(expandedPosition.x, -panelHeight);
    }

    private void Start()
    {
        if (brushManager == null)
            brushManager = FindFirstObjectByType<BrushManager>();

        if (brushSizeSlider != null)
        {
            brushSizeSlider.minValue = minBrushSize;
            brushSizeSlider.maxValue = maxBrushSize;
            brushSizeSlider.value = brushManager != null ? brushManager.GetBrushSize() : 5f;
            brushSizeSlider.onValueChanged.AddListener(OnBrushSizeChanged);
        }
    }

    public void OnToggleButtonClicked()
    {
        Toggle();
    }
}