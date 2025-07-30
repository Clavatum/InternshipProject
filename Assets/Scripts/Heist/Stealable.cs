using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class Stealable : MonoBehaviour
{
    [Header("Class References")]
    private DynamicMoveProvider dynamicMoveProvider;

    [Header("Features")]
    public int value;
    [SerializeField] private float slowPercentage;
    private float minSpeedLimit = 0.5f;
    private float originalSpeed;

    [HideInInspector] public bool isSlowApplied = false;

    public bool isHeld = false;


    void Awake()
    {
        dynamicMoveProvider = FindAnyObjectByType<DynamicMoveProvider>();
        originalSpeed = dynamicMoveProvider.moveSpeed;
    }

    public void ApplySlow()
    {
        originalSpeed -= originalSpeed * slowPercentage / 100f;
        dynamicMoveProvider.moveSpeed = originalSpeed;
        isSlowApplied = true;
    }

    public void RemoveSlow()
    {
        dynamicMoveProvider.moveSpeed = originalSpeed;
        isSlowApplied = false;
    }

    public bool IsSpeedLimitExceeded()
    {
        return dynamicMoveProvider.moveSpeed <= minSpeedLimit;
    }

    public void ToggleHeld()
    {
        isHeld = !isHeld;
    }
}