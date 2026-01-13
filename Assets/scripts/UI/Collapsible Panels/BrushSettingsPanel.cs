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
    [SerializeField] private TextMeshProUGUI hotkeyFeedbackText; // For visual feedback

    [Header("References")]
    [SerializeField] private BrushManager brushManager;

    [Header("Settings")]
    [SerializeField] private float minBrushSize = 1f;
    [SerializeField] private float maxBrushSize = 20f;
    [SerializeField] private float minBrushStrength = 0.1f;
    [SerializeField] private float maxBrushStrength = 5f;

    private bool isUpdatingFromManager = false; // Prevent circular updates

    protected override void SetupCollapsedPosition()
    {
        base.SetupCollapsedPosition();
        collapsedPosition = new Vector2(expandedPosition.x, -panelHeight);
    }

    private void Start()
    {
        // Initialize UI elements
        if (brushManager == null)
            brushManager = FindFirstObjectByType<BrushManager>();

        if (brushManager != null)
        {
            // Subscribe to events
            brushManager.OnBrushSizeChanged += OnBrushSizeChangedFromManager;
            brushManager.OnBrushStrengthChanged += OnBrushStrengthChangedFromManager;
            brushManager.OnSizeAdjustModeChanged += OnSizeAdjustModeChanged;
            brushManager.OnStrengthAdjustModeChanged += OnStrengthAdjustModeChanged;
        }

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
        UpdateTextLabels();
        UpdateHotkeyFeedback(""); // Clear initial feedback
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (brushManager != null)
        {
            brushManager.OnBrushSizeChanged -= OnBrushSizeChangedFromManager;
            brushManager.OnBrushStrengthChanged -= OnBrushStrengthChangedFromManager;
            brushManager.OnSizeAdjustModeChanged -= OnSizeAdjustModeChanged;
            brushManager.OnStrengthAdjustModeChanged -= OnStrengthAdjustModeChanged;
        }
    }

    // Called when brush size changes from BrushManager (hotkeys)
    private void OnBrushSizeChangedFromManager(float size)
    {
        if (brushSizeSlider != null && !isUpdatingFromManager)
        {
            isUpdatingFromManager = true;
            brushSizeSlider.value = size;
            isUpdatingFromManager = false;
            UpdateTextLabels();
        }
    }

    // Called when brush strength changes from BrushManager (hotkeys)
    private void OnBrushStrengthChangedFromManager(float strength)
    {
        if (brushStrengthSlider != null && !isUpdatingFromManager)
        {
            isUpdatingFromManager = true;
            brushStrengthSlider.value = strength;
            isUpdatingFromManager = false;
            UpdateTextLabels();
        }
    }

    // Called when size adjustment mode changes
    private void OnSizeAdjustModeChanged(bool isActive)
    {
        if (isActive)
        {
            UpdateHotkeyFeedback("Adjusting Brush Size (F to exit)");
        }
        else
        {
            UpdateHotkeyFeedback("");
        }
    }

    // Called when strength adjustment mode changes
    private void OnStrengthAdjustModeChanged(bool isActive)
    {
        if (isActive)
        {
            UpdateHotkeyFeedback("Adjusting Brush Strength (Shift+F to exit)");
        }
        else
        {
            UpdateHotkeyFeedback("");
        }
    }

    private void OnBrushSizeChanged(float value)
    {
        if (!isUpdatingFromManager && brushManager != null)
        {
            brushManager.SetBrushSize(value);
        }
        UpdateTextLabels();
    }

    private void OnBrushStrengthChanged(float value)
    {
        if (!isUpdatingFromManager && brushManager != null)
        {
            brushManager.SetBrushStrength(value);
        }
        UpdateTextLabels();
    }

    private void UpdateTextLabels()
    {
        if (brushSizeText != null && brushSizeSlider != null)
        {
            brushSizeText.text = $"Brush size: {brushSizeSlider.value:F1}";
        }

        if (brushStrengthText != null && brushStrengthSlider != null)
        {
            brushStrengthText.text = $"Brush strength: {brushStrengthSlider.value:F1}";
        }
    }

    private void UpdateHotkeyFeedback(string message)
    {
        if (hotkeyFeedbackText != null)
        {
            hotkeyFeedbackText.text = message;
            // Optional: Change color or add animation here
        }
    }

    public void OnToggleButtonClicked()
    {
        Toggle();
    }
}