using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIInputUtility
{
    private static readonly List<RaycastResult> RaycastResults = new List<RaycastResult>();

    public static bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        RaycastResults.Clear();
        EventSystem.current.RaycastAll(eventData, RaycastResults);

        return RaycastResults.Count > 0;
    }
}
