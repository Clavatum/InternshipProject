using System;
using Unity.XR.CoreUtils;
using UnityEngine;

public class OpenedWindowTrigger : MonoBehaviour
{
    public static event Action<OpenedWindowTrigger> OnWindowEnter;
    public static event Action OnWindowExit;

    [SerializeField] private Transform teleportTarget;
    [SerializeField] private bool isInside = false;

    public bool IsInside => isInside;
    public Transform TeleportTarget => teleportTarget;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<XROrigin>() != null)
        {
            if (!IsInside)
            {
                OnWindowEnter?.Invoke(this);
                return;
            }
            OnWindowExit?.Invoke();
        }
    }
}
