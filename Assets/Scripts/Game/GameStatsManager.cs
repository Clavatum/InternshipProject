using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    [Header("Class References")]
    public GameManager gameEndState;

    [Header("Stats")]
    [HideInInspector] public int totalDirtyWindow;
    [HideInInspector] public int totalCleanedState;
    [HideInInspector] public float totalPlayedTime;
    [SerializeField] private int prizeForTimeLeft = 10;
    private int score = 0;
    public int prizeForEachCleanedState { get; private set; } = 100;

    #region - Getter/Setter -

    public void SetScore()
    {
        PlayerPrefs.SetInt("Score", score);
    }

    public int GetScore() => PlayerPrefs.GetInt("Score", 0);

    public void SetTotalPlayedTime()
    {
        PlayerPrefs.SetFloat("TotalPlayedTime", totalPlayedTime);
    }

    public float GetTotalPlayedTime() => PlayerPrefs.GetFloat("TotalPlayedTime", 0);

    #endregion

    public void CalculateScore()
    {
        score = totalCleanedState * prizeForEachCleanedState + (int)(gameEndState.TotalTimeLeft * prizeForTimeLeft);
    }

    public void UpdateScore(int value)
    {
        score += value;
        SetScore();
    }
}