using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    [Header("Class References")]
    public GameManager gameEndState;

    [Header("Stats")]
    public int totalDirtyWindow;
    public int totalCleanedWindow;
    public int score = 0;
    public int prizeForEachCleanedWindow = 100;
    public int prizeForTimeLeft = 10;
    public float totalPlayedTime;

    #region - Getter/Setter -

    public void SetScore()
    {
        PlayerPrefs.SetInt("Score", score);
        Save();
    }
    public int GetScore() => PlayerPrefs.GetInt("Score", 0);

    public void SetTotalPlayedTime()
    {
        PlayerPrefs.SetFloat("TotalPlayedTime", totalPlayedTime);
        Save();
    }

    public float GetTotalPlayedTime() => PlayerPrefs.GetFloat("TotalPlayedTime", 0);

    #endregion

    public void CalculateScore()
    {
        score = totalCleanedWindow * prizeForEachCleanedWindow + (int)(gameEndState.StartTimer() * prizeForTimeLeft);
    }

    public void Save() => PlayerPrefs.Save();
}