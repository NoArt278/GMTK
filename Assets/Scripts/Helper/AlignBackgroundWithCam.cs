using NaughtyAttributes;
using UnityEngine;

public class AlignBackgroundWithCam : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] private Transform backgroundPlane;

    [ReadOnly] [SerializeField] private Vector3 capturedPlaneSize;
    [ReadOnly] [SerializeField] private float capturedOrthographicSize;

    // // DONT TOUCH THIS
    // [Button("Capture Background Size")]
    // private void CaptureBackgroundSize()
    // {
    //     capturedPlaneSize = backgroundPlane.transform.localScale;
    //     capturedOrthographicSize = Camera.main.orthographicSize;

    //     Debug.Log("Background size captured.");
    // }

    [Button("Align Background")]
    private void AlignBackground()
    {
        backgroundPlane.localScale = capturedPlaneSize * Camera.main.orthographicSize / capturedOrthographicSize;
        Debug.Log("Background aligned with camera.");
    }
    #endif
}