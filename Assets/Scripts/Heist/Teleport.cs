using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform playerTransformInElevator;

    public void TeleportTo(OpenedWindowTrigger openedWindowTrigger)
    {
        if (!openedWindowTrigger.isInside)
        {
            gameObject.transform.localPosition = openedWindowTrigger.TeleportTarget.localPosition;
            gameObject.transform.localRotation = openedWindowTrigger.TeleportTarget.localRotation;
            openedWindowTrigger.isInside = true;
            Debug.Log("teleported apartment");
            return;
        }
        gameObject.transform.localPosition = playerTransformInElevator.transform.localPosition;
        gameObject.transform.localRotation = playerTransformInElevator.transform.localRotation;
        openedWindowTrigger.isInside = false;
        Debug.Log("teleported elevator");
    }
}