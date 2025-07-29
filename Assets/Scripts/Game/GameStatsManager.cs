using System;
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
    private int totalScore;
    public int currentScore = 0;
    public int PrizeForEachCleanedState { get; private set; } = 100;

    #region - Getter/Setter -

    void Start()
    {
        totalScore = GetTotalScore();
        totalPlayedTime = GetTotalPlayedTime();
    }

    public void SetTotalScore()
    {
        PlayerPrefs.SetInt("TotalScore", totalScore);
    }

    public int GetTotalScore() => PlayerPrefs.GetInt("TotalScore");

    public void SetTotalPlayedTime()
    {
        PlayerPrefs.SetFloat("TotalPlayedTime", totalPlayedTime);
    }

    public float GetTotalPlayedTime() => PlayerPrefs.GetFloat("TotalPlayedTime");

    #endregion

    public void CalculateCurrentScore()
    {
        currentScore += totalCleanedState * PrizeForEachCleanedState + (int)(gameEndState.TotalTimeLeft * prizeForTimeLeft);
    }

    public void UpdateCurrentScore(int value)
    {
        currentScore += value;
    }

    public void UpdateTotalScore()
    {
        totalScore += currentScore;
        SetTotalScore();
    }
}