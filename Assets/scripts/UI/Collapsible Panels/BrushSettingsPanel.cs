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
    [SerializeField] private TextMeshProUGUI brushSizeText;
    [SerializeField] private TextMeshProUGUI brushStrengthText;
    [SerializeField] private TextMeshProUGUI hotkeyFeedbackText;
    [SerializeField] private TextMeshProUGUI tooltipText;

    [Header("References")]
    [SerializeField] private BrushManager brushManager;

    [Header("Settings")]
    [SerializeField] private float minBrushSize = 1f;
    [SerializeField] private float maxBrushSize = 20f;
    [SerializeField] private float minBrushStrength = 0.1f;
    [SerializeField] private float maxBrushStrength = 5f;

    private bool isUpdatingFromManager = false;

    protected override void SetupCollapsedPosition()
    {
        base.SetupCollapsedPosition();
        collapsedPosition = new Vector2(expandedPosition.x, -panelHeight);
    }

    private void Awake()
    {
        if (tooltipText == null && transform.parent != null)
        {
            Transform tooltipTransform = transform.parent.Find("Tooltip");
            if (tooltipTransform != null)
                tooltipText = tooltipTransform.GetComponent<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        if (brushManager == null)
            brushManager = FindFirstObjectByType<BrushManager>();

        if (brushManager != null)
        {
            brushManager.OnBrushSizeChanged += OnBrushSizeChangedFromManager;
            brushManager.OnBrushStrengthChanged += OnBrushStrengthChangedFromManager;
            brushManager.OnActiveBrushChanged += OnActiveBrushChanged;

            TerrainBrush activeBrush = brushManager.GetActiveBrush();
            if (activeBrush != null)
                OnActiveBrushChanged(brushManager.ActiveBrushIndex, activeBrush);
        }

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

        UpdateTextLabels();
        UpdateHotkeyFeedback("Shift + scroll: size\nCtrl + scroll: strength");
    }

    private void OnDestroy()
    {
        if (brushManager != null)
        {
            brushManager.OnBrushSizeChanged -= OnBrushSizeChangedFromManager;
            brushManager.OnBrushStrengthChanged -= OnBrushStrengthChangedFromManager;
            brushManager.OnActiveBrushChanged -= OnActiveBrushChanged;
        }
    }

    private void OnActiveBrushChanged(int index, TerrainBrush brush)
    {
        if (tooltipText == null || brush == null)
            return;

        tooltipText.text = brush.BrushDescription;
    }

    private void OnBrushSizeChangedFromManager(float size)
    {
        if (brushSizeSlider != null && !isUpdatingFromManager)
        {
            isUpdatingFromManager = true;
            brushSizeSlider.value = Mathf.Clamp(size, brushSizeSlider.minValue, brushSizeSlider.maxValue);
            isUpdatingFromManager = false;
            UpdateTextLabels();
        }
    }

    private void OnBrushStrengthChangedFromManager(float strength)
    {
        if (brushStrengthSlider != null && !isUpdatingFromManager)
        {
            isUpdatingFromManager = true;
            brushStrengthSlider.value = Mathf.Clamp(strength, brushStrengthSlider.minValue, brushStrengthSlider.maxValue);
            isUpdatingFromManager = false;
            UpdateTextLabels();
        }
    }

    private void OnBrushSizeChanged(float value)
    {
        if (!isUpdatingFromManager && brushManager != null)
            brushManager.SetBrushSize(value);

        UpdateTextLabels();
    }

    private void OnBrushStrengthChanged(float value)
    {
        if (!isUpdatingFromManager && brushManager != null)
            brushManager.SetBrushStrength(value);

        UpdateTextLabels();
    }

    private void UpdateTextLabels()
    {
        if (brushSizeText != null && brushManager != null)
            brushSizeText.text = $"Brush size: {brushManager.GetBrushSize():F1}";

        if (brushStrengthText != null && brushManager != null)
            brushStrengthText.text = $"Brush strength: {brushManager.GetBrushStrength():F1}";
    }

    private void UpdateHotkeyFeedback(string message)
    {
        if (hotkeyFeedbackText != null)
            hotkeyFeedbackText.text = message;
    }

    public void OnToggleButtonClicked()
    {
        Toggle();
    }
}
