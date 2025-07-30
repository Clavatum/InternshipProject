using System;
using Unity.XR.CoreUtils;
using UnityEngine;

public class OpenedWindowTrigger : MonoBehaviour
{
    public static event Action<OpenedWindowTrigger> OnWindowEnter;
    public static event Action OnWindowExit;

    [SerializeField] private Transform teleportTarget;
    public bool isInside = false;

    public Transform TeleportTarget => teleportTarget;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<XROrigin>() != null)
        {
            OnWindowEnter?.Invoke(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        OnWindowExit?.Invoke();
    }
}
