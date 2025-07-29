using UnityEngine;

public class Pocket : MonoBehaviour
{
    private InGameStatsUI inGameStatsUI;
    private Stealable stealable;

    void Awake()
    {
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Stealable>() != null)
        {
            stealable = other.gameObject.GetComponent<Stealable>();
            if (stealable.IsSpeedLimitExceeded())
            {
                inGameStatsUI.ErrorFeedback("Can't steal more!");
                return;
            }
            Destroy(other.gameObject);
            inGameStatsUI.SuccessFeedback($"+${stealable.value}");
        }
    }
}
