using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatsManager : MonoBehaviour
{
    [Header("Class References")]
    private GameManager gameManager;

    [Header("Stats")]
    [HideInInspector] public int totalDirtyWindow;
    [HideInInspector] public int totalCleanedState;
    [HideInInspector] public float totalPlayedTime;
    [SerializeField] private int prizeForTimeLeft = 10;
    private int totalScore;
    [HideInInspector] public int currentScore = 0;
    public int PrizeForEachCleanedState { get; private set; } = 100;


    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void Start()
    {
        currentScore = 0;
        totalCleanedState = 0;
        totalScore = GetTotalScore();
        totalPlayedTime = GetTotalPlayedTime();
    }

    #region - Getter/Setter -

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

    public void CalculateCurrentScore()
    {
        if (gameManager == null) { return; }
        currentScore += gameManager.IsGameOver ? totalCleanedState * PrizeForEachCleanedState : totalCleanedState * PrizeForEachCleanedState + (int)(gameManager.TotalTimeLeft * prizeForTimeLeft);
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

    /*void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }*/
    #endregion
}