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
        
    }
}
