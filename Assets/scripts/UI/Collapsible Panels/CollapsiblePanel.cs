using UnityEngine;

// This script is used to control the collapsible panel in the UI.

public class CollapsiblePanel : MonoBehaviour
{

    [Header("Panel Settings")]
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private float animationSpeed = 500f;

    [Header("States")]
    private bool isExpanded = true;
    private Vector2 expandedPosition;
    private Vector2 collapsedPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
