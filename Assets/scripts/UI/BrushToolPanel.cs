using UnityEngine;
using UnityEngine.UI;

public class BrushToolPanel : MonoBehaviour
{
    [Header("Brush Buttons")]
    [SerializeField] private Button raiseButton;
    [SerializeField] private Button lowerButton;
    [SerializeField] private Button smoothButton;

    [Header("References")]
    [SerializeField] private BrushManager brushManager;

    private Button currentActiveButton;

    private void Start()
    {
        if(brushManager == null)
            brushManager = FindFirstObjectByType<BrushManager>();

        if (raiseButton != null)
            raiseButton.onClick.AddListener(() => OnRaiseBrushClicked());

        if (lowerButton != null)
            lowerButton.onClick.AddListener(() => OnLowerBrushClicked());

        if (smoothButton != null)
            smoothButton.onClick.AddListener(() => OnSmoothBrushClicked());
    }

    private void OnRaiseBrushClicked()
    {
        if (brushManager != null)
        {
            brushManager.SetActiveBrush(typeof(RaiseBrush));
            SetActiveButton(raiseButton);
        }
    }

    private void OnLowerBrushClicked()
    {
        if (brushManager != null)
        {
            brushManager.SetActiveBrush(typeof(LowerBrush));
            SetActiveButton(lowerButton);
        }
    }

    private void OnSmoothBrushClicked()
    {
        if (brushManager != null)
        {
            brushManager.SetActiveBrush(typeof(SmoothBrush));
            SetActiveButton(smoothButton);
        }
    }

    // Visual feedback for active brush
    private void SetActiveButton(Button activeButton)
    {
        // Reset previous active button color
        if (currentActiveButton != null)
        {
            ColorBlock colors = currentActiveButton.colors;
            colors.normalColor = Color.white;
            currentActiveButton.colors = colors;
        }

        // Set new active button color
        if (activeButton != null)
        {
            currentActiveButton = activeButton;
            ColorBlock colors = activeButton.colors;
            colors.normalColor = Color.cyan; // Highlight active brush
            activeButton.colors = colors;
        }
    }
}
