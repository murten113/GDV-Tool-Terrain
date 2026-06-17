using UnityEngine;
using UnityEngine.UI;

public class BrushToolPanel : MonoBehaviour
{
    [Header("Brush Buttons")]
    [SerializeField] private Button[] brushButtons;

    [Header("References")]
    [SerializeField] private BrushManager brushManager;

    [Header("Highlight")]
    [SerializeField] private Color activeButtonColor = new Color(0f, 1f, 1f, 1f);
    [SerializeField] private Color inactiveButtonColor = Color.white;

    private Button currentActiveButton;

    private void Start()
    {
        if (brushManager == null)
            brushManager = FindFirstObjectByType<BrushManager>();

        if (brushButtons != null)
        {
            for (int i = 0; i < brushButtons.Length; i++)
            {
                if (brushButtons[i] == null)
                    continue;

                int index = i;
                brushButtons[i].onClick.AddListener(() => SelectBrush(index));
            }
        }

        if (brushManager != null)
        {
            brushManager.OnActiveBrushChanged += OnActiveBrushChanged;
            TerrainBrush activeBrush = brushManager.GetActiveBrush();
            if (activeBrush != null)
                OnActiveBrushChanged(brushManager.ActiveBrushIndex, activeBrush);
        }
    }

    private void OnDestroy()
    {
        if (brushManager != null)
            brushManager.OnActiveBrushChanged -= OnActiveBrushChanged;
    }

    private void SelectBrush(int index)
    {
        if (brushManager != null)
            brushManager.SetActiveBrush(index);
    }

    private void OnActiveBrushChanged(int index, TerrainBrush brush)
    {
        if (brushButtons == null || index < 0 || index >= brushButtons.Length)
            return;

        SetActiveButton(brushButtons[index]);
    }

    private void SetActiveButton(Button activeButton)
    {
        if (brushButtons == null)
            return;

        foreach (Button button in brushButtons)
        {
            if (button == null)
                continue;

            ApplyButtonHighlight(button, button == activeButton);
        }

        currentActiveButton = activeButton;
    }

    private void ApplyButtonHighlight(Button button, bool active)
    {
        Color color = active ? activeButtonColor : inactiveButtonColor;

        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        button.colors = colors;

        if (button.targetGraphic != null)
            button.targetGraphic.color = color;
    }
}
