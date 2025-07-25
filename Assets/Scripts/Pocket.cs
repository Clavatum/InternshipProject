using UnityEngine;

public class Pocket : MonoBehaviour
{
    private InGameStatsUI inGameStatsUI;
    private Stealable stealable;

    void Awake()
    {
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
        stealable = FindAnyObjectByType<Stealable>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Stealable>() != null)
        {
            if (stealable.IsSpeedLimitExceeded())
            {
                inGameStatsUI.ErrorFeedback("Can't steal more!");
                return;
            }
            Destroy(other);
            inGameStatsUI.SuccessFeedback($"+${stealable.value}");
        }
    }
}
