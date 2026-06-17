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
        if (brushManager == null)
            brushManager = FindFirstObjectByType<BrushManager>();

        if (raiseButton != null)
            raiseButton.onClick.AddListener(() => SelectBrush(0, raiseButton));

        if (lowerButton != null)
            lowerButton.onClick.AddListener(() => SelectBrush(1, lowerButton));

        if (smoothButton != null)
            smoothButton.onClick.AddListener(() => SelectBrush(2, smoothButton));

        if (brushManager != null && brushManager.Brushes.Count > 0)
            SetActiveButton(raiseButton);
    }

    private void SelectBrush(int index, Button button)
    {
        if (brushManager != null)
        {
            brushManager.SetActiveBrush(index);
            SetActiveButton(button);
        }
    }

    private void SetActiveButton(Button activeButton)
    {
        if (currentActiveButton != null)
        {
            ColorBlock colors = currentActiveButton.colors;
            colors.normalColor = Color.white;
            currentActiveButton.colors = colors;
        }

        if (activeButton != null)
        {
            currentActiveButton = activeButton;
            ColorBlock colors = activeButton.colors;
            colors.normalColor = Color.cyan;
            activeButton.colors = colors;
        }
    }
}
