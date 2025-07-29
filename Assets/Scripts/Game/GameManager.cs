using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Class References")]
    private Heist heist;
    private GameStatsManager gameStatsManager;
    private SceneController sceneController;
    private InGameStatsUI inGameStatsUI;

    [Space, SerializeField] private float totalTime;

    public float TotalTimeLeft { get; private set; }
    public float TotalHeistTimeLeft { get; private set; }
    private bool IsGameOver => TotalHeistTimeLeft <= 0f || TotalTimeLeft <= 0f;
    private bool IsGameWin => gameStatsManager.totalCleanedState / 4 == gameStatsManager.totalDirtyWindow;
    private bool isGameEndScreenShown = false;

    #region - Awake/Start/Update -

    void Awake()
    {
        heist = FindAnyObjectByType<Heist>();
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
        sceneController = FindAnyObjectByType<SceneController>();
    }

    void Start()
    {
        TotalTimeLeft = totalTime;
        TotalHeistTimeLeft = heist.totalHeistTime;
    }

    void Update()
    {
        if (isGameEndScreenShown) { return; }
        StartTotalTimer();
        StartHeistTimer();

        if (IsGameOver || IsGameWin)
        {
            isGameEndScreenShown = true;
            gameStatsManager.CalculateScore();
            StartCoroutine(EndGame());
            gameStatsManager.totalPlayedTime += totalTime - TotalTimeLeft;
            TotalTimeLeft = 0f;
            TotalHeistTimeLeft = 0f;
            gameStatsManager.SetTotalPlayedTime();
            gameStatsManager.SetScore();
            PlayerPrefs.Save();
        }
    }

    private void StartHeistTimer()
    {
        if (heist.IsHeistStarted)
        {
            inGameStatsUI.ShowTimeLeft(inGameStatsUI.heistTimeLeftText, TotalHeistTimeLeft);
            TotalHeistTimeLeft -= Time.deltaTime;
        }

    }

    private void StartTotalTimer()
    {
        inGameStatsUI.ShowTimeLeft(inGameStatsUI.totalTimeLeftText, TotalTimeLeft);

        TotalTimeLeft -= Time.deltaTime;
    }

    private IEnumerator EndGame()
    {

        if (IsGameOver)
        {
            inGameStatsUI.GameOverFeedback("Game Over!");
        }

        if (IsGameWin)
        {
            inGameStatsUI.GameWonFeedback("You Win!");
        }

        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3f);
        sceneController.SetScene(0);
    }

    void OnEnable()
    {
        Heist.SetHeistTimerOnHeistStarted += StartHeistTimer;
    }

    void OnDisable()
    {
        Heist.SetHeistTimerOnHeistStarted -= StartHeistTimer;
    }

    #endregion
}