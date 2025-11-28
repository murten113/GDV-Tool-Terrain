using UnityEngine;

public class BrushSettingsPanel : CollapsiblePanel
{
    [Header("Brush Settings Specific")]
    [SerializeField] private float panelHeight = 180f;
    protected override void SetupCollapsedPosition()
    {
        base.SetupCollapsedPosition();

        // Collapsed: hide panel below (off-screen)
        collapsedPosition = new Vector2(expandedPosition.x, -panelHeight);
    }

    public void OnToggleButtonClicked()
    {
        Toggle();
    }
}