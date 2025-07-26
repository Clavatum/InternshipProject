using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform playerTransformInElevator;

    public void TeleportTo(OpenedWindowTrigger openedWindowTrigger)
    {
        if (!openedWindowTrigger.IsInside)
        {
            gameObject.transform.localPosition = openedWindowTrigger.TeleportTarget.localPosition;
            gameObject.transform.localRotation = openedWindowTrigger.TeleportTarget.localRotation;
        }
        gameObject.transform.localPosition = playerTransformInElevator.transform.localPosition;
        gameObject.transform.localRotation = playerTransformInElevator.transform.localRotation;
    }
}