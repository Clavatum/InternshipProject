using UnityEngine;

public class Pocket : MonoBehaviour
{
    private InGameStatsUI inGameStatsUI;
    private Stealable stealable;
    private GameStatsManager gameStatsManager;

    void Awake()
    {
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Stealable>() != null)
        {
            stealable = other.gameObject.GetComponent<Stealable>();
            if (stealable.isHeld) { return; }
            if (stealable.IsSpeedLimitExceeded())
            {
                inGameStatsUI.ErrorFeedback("Can't steal more!");
                return;
            }
            gameStatsManager.UpdateCurrentScore(stealable.value);
            inGameStatsUI.UpdateScoreText(gameStatsManager.currentScore);
            if (!stealable.isSlowApplied) { stealable.ApplySlow(); }
            Destroy(other.gameObject);
            inGameStatsUI.SuccessFeedback($"+${stealable.value}");
        }
    }

}
