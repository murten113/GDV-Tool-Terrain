using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


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
        // Initialize UI elements
        if (brushManager == null)
            brushManager = FindFirstObjectByType<BrushManager>();

        // Initialize sliders
        if (brushSizeSlider != null)
        {
            brushSizeSlider.minValue = minBrushSize;
            brushSizeSlider.maxValue = maxBrushSize;
            brushSizeSlider.value = brushManager != null ? brushManager.GetBrushSize() : 5f;
            brushSizeSlider.onValueChanged.AddListener(OnBrushSizeChanged);
        }

        if (brushStrengthSlider != null)
        {
            brushStrengthSlider.minValue = minBrushStrength;
            brushStrengthSlider.maxValue = maxBrushStrength;
            brushStrengthSlider.value = brushManager != null ? brushManager.GetBrushStrength() : 1f;
            brushStrengthSlider.onValueChanged.AddListener(OnBrushStrengthChanged);
        }

        // Update text labels
        UpdateTextlabels();
    }

    private void OnBrushSizeChanged(float value)
    {
        if (brushManager != null)
        {
            brushManager.SetBrushSize(value);
        }
        UpdateTextlabels();
    }

    private void OnBrushStrengthChanged(float value)
    {
        if (brushManager != null)
        {
            brushManager.SetBrushStrength(value);
        }
        UpdateTextlabels();
    }

    private void UpdateTextlabels()
    {
        if(brushSieText != null && brushSizeSlider != null)
        {
            brushSieText.text = $"Brush size: {brushSizeSlider.value:F1}";
        }

        if (brushStrengthText != null && brushStrengthSlider != null)
        {
            brushStrengthText.text = $"Brush strength: {brushStrengthSlider.value:F1}";
        }
    }


    public void OnToggleButtonClicked()
    {
        Toggle();
    }
}