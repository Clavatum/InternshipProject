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

    public float TotalTimeLeft { get; private set; } = 0f;
    public float TotalHeistTimeLeft { get; private set; } = 0f;
    private bool IsGameEnded => heist.totalHeistTime <= 0f || TotalTimeLeft <= 0f || gameStatsManager.totalCleanedState / 4 == gameStatsManager.totalDirtyWindow;

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
        StartTotalTimer();
        StartHeistTimer();

        if (IsGameEnded)
        {
            gameStatsManager.CalculateScore();
            StartCoroutine(EndGame());
            gameStatsManager.totalPlayedTime += TotalTimeLeft;
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
            heist.totalHeistTime -= Time.deltaTime;
        }

    }

    private void StartTotalTimer()
    {
        inGameStatsUI.ShowTimeLeft(inGameStatsUI.totalTimeLeftText, TotalTimeLeft);

        TotalTimeLeft -= Time.deltaTime;
    }

    private IEnumerator EndGame()
    {
        inGameStatsUI.EndGameFeedback("Game Finished!");
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