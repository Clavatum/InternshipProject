using System;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;
    [Header("Class References")]
    public GameManager gameEndState;

    [Header("Stats")]
    [HideInInspector] public int totalDirtyWindow;
    [HideInInspector] public int totalCleanedState;
    [HideInInspector] public float totalPlayedTime;
    [SerializeField] private int prizeForTimeLeft = 10;
    private int score;
    public int PrizeForEachCleanedState { get; private set; } = 100;

    #region - Getter/Setter -
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetScore()
    {
        PlayerPrefs.SetInt("Score", score);
    }

    public int GetScore() => PlayerPrefs.GetInt("Score");

    public void SetTotalPlayedTime()
    {
        PlayerPrefs.SetFloat("TotalPlayedTime", totalPlayedTime);
    }

    public float GetTotalPlayedTime() => PlayerPrefs.GetFloat("TotalPlayedTime");

    #endregion

    public void CalculateScore()
    {
        score += totalCleanedState * PrizeForEachCleanedState + (int)(gameEndState.TotalTimeLeft * prizeForTimeLeft);
        Debug.Log(score);
    }

    public void UpdateScore(int value)
    {
        score += value;
        SetScore();
    }
}