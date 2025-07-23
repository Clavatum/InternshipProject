using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public GameEndState gameEndState;
    public int totalDirtyWindow;
    public int totalCleanedWindow;
    public int score = 0;
    public int prizeForEachCleanedWindow = 100;
    public int prizeForTimeLeft = 10;

    public void CalculateScore()
    {
        score = totalCleanedWindow * prizeForEachCleanedWindow + (int)(gameEndState.StartTimer() * prizeForTimeLeft);
    }

    public void SetScore() => PlayerPrefs.SetInt("Score", score);
    public int GetScore() => PlayerPrefs.GetInt("Score", 0);
}