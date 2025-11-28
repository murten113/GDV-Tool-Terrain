using UnityEngine;

public class MainMenuPanel : CollapsiblePanel
{
    [Header("Main Menu Specific")]
    [SerializeField] private float panelWidth = 250f;

    protected override void SetupCollapsedPosition()
    {
        base.SetupCollapsedPosition();

        // Collapsed: hide panel to the left (off-screen)
        collapsedPosition = new Vector2(-panelWidth, expandedPosition.y);
    }

    public void OnToggleButtonClicked()
    {
        Toggle();
    }
}