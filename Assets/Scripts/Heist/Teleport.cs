using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform playerTransformInElevator;

    public void TeleportTo(OpenedWindowTrigger openedWindowTrigger)
    {
        CharacterController characterController = GetComponent<CharacterController>();
        characterController.enabled = false;
        if (!openedWindowTrigger.isInside)
        {
            gameObject.transform.position = openedWindowTrigger.TeleportTarget.position;
            characterController.enabled = true;
            openedWindowTrigger.isInside = true;
            return;
        }
        gameObject.transform.position = playerTransformInElevator.transform.position;
        characterController.enabled = true;
        openedWindowTrigger.isInside = false;
    }
}