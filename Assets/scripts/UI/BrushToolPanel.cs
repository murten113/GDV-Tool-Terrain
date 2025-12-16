using UnityEngine;
using UnityEngine.UI;

public class BrushToolPanel : MonoBehaviour
{
    [Header("Brush Buttons")]
    [SerializeField] private Button raiseButton;
    [SerializeField] private Button lowerButton;
    [SerializeField] private Button flattenButton;

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

        if (flattenButton != null)
            flattenButton.onClick.AddListener(() => OnFlattenBrushClicked());
    }

    private void OnRaiseBrushClicked()
    {
        if (brushManager != null)
        {
            brushManager.SetActiveBrush(BrushManager.BrushType.Raise);
            SetActiveButton(raiseButton);
        }
    }

    private void OnLowerBrushClicked()
    {
        if (brushManager != null)
        {
            brushManager.SetActiveBrush(BrushManager.BrushType.Lower);
            SetActiveButton(lowerButton);
        }
    }

    private void OnFlattenBrushClicked()
    {
        if (brushManager != null)
        {
            brushManager.SetActiveBrush(BrushManager.BrushType.Flatten);
            SetActiveButton(flattenButton);
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
