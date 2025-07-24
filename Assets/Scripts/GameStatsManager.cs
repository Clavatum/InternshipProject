using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public GameEndState gameEndState;
    public int totalDirtyWindow;
    public int totalCleanedWindow;
    public int score = 0;
    public int prizeForEachCleanedWindow = 100;
    public int prizeForTimeLeft = 10;
    public float totalPlayedTime;

    public void CalculateScore()
    {
        score = totalCleanedWindow * prizeForEachCleanedWindow + (int)(gameEndState.StartTimer() * prizeForTimeLeft);
    }

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
    public void Save() => PlayerPrefs.Save();
}