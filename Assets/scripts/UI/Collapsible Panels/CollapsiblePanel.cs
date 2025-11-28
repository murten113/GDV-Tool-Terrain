using UnityEngine;

// This script is used to control the collapsible panel in the UI.

public class CollapsiblePanel : MonoBehaviour
{

    [Header("Panel Settings")]
    [SerializeField] private RectTransform panelRectTransform;

    [Header("States")]
    private bool isExpanded = true;
    protected Vector2 expandedPosition;
    protected Vector2 collapsedPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (panelRectTransform == null)
            panelRectTransform = GetComponent<RectTransform>();

        //saves the starting position as expanded position
        expandedPosition = panelRectTransform.anchoredPosition;

        //sets up the collapsed position
        SetupCollapsedPosition();
    }

    //sets up the collapsed position
    protected virtual void SetupCollapsedPosition()
    {
        collapsedPosition = expandedPosition;
    }

    //toggles the panel between expanded and collapsed state
    public void Toggle()
    {
        isExpanded = !isExpanded;
        MoveToTargetPosition();
    }

    //sets the panel to the expanded or collapsed state
    public void SetExpanded(bool expand)
    {
        if (isExpanded != expand)
        {
            isExpanded = expand;
            MoveToTargetPosition();
        }
    }

    private void MoveToTargetPosition()
    {
        Vector2 targetPosition = isExpanded ? expandedPosition : collapsedPosition;
        
        panelRectTransform.anchoredPosition = targetPosition;
    }
}
