using NaughtyAttributes;
using UnityEngine;

public class CenterParentToChildren : MonoBehaviour
{
    #if UNITY_EDITOR
    [Button("Center Pivot")]
    public void CenterPivot()
    {
        if (transform.childCount == 0)
        {
            Debug.LogWarning("No children found to center the parent to.");
            return;
        }

        Vector3 centerPoint = Vector3.zero;
        
        // Calculate the average position of all children
        foreach (Transform child in transform)
        {
            centerPoint += child.position;
        }
        centerPoint /= transform.childCount;

        // Store the parent's original position
        Vector3 parentOriginalPosition = transform.position;

        // Move the parent to the center point
        transform.position = centerPoint;

        // Move all children to maintain their world positions
        foreach (Transform child in transform)
        {
            child.position += parentOriginalPosition - centerPoint;
        }

        Debug.Log("Parent centered to children.");
    }
    #endif
}