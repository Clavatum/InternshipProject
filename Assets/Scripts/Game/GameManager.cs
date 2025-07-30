using System;
using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManager : MonoBehaviour
{
    [Header("Class References")]
    private Heist heist;
    private GameStatsManager gameStatsManager;
    private SceneController sceneController;
    private InGameStatsUI inGameStatsUI;

    private Animator gameOverAnimator;

    [Space, SerializeField] private float totalTime;

    public float TotalTimeLeft { get; private set; }
    public float TotalHeistTimeLeft { get; private set; }
    public bool IsGameOver => TotalHeistTimeLeft <= 0f || TotalTimeLeft <= 0f;
    private bool IsGameWin => gameStatsManager.totalCleanedState / 4 == gameStatsManager.totalDirtyWindow;
    private bool isGameEndScreenShown = false;

    #region - Awake/Start/Update -

    void Awake()
    {
        heist = FindAnyObjectByType<Heist>();
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
        sceneController = FindAnyObjectByType<SceneController>();
        gameOverAnimator = inGameStatsUI.gameOverPanel.GetComponent<Animator>();
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
            gameStatsManager.CalculateCurrentScore();
            EndGame();
            StartCoroutine(ReturnMainMenu());
            gameStatsManager.totalPlayedTime += totalTime - TotalTimeLeft;
            TotalTimeLeft = 0f;
            TotalHeistTimeLeft = 0f;
            gameStatsManager.SetTotalPlayedTime();
            gameStatsManager.UpdateTotalScore();
            gameStatsManager.currentScore = 0;
            PlayerPrefs.Save();
        }
    }

    private void StartHeistTimer()
    {
        if (heist.IsHeistStarted)
        {
            inGameStatsUI.ShowTimeLeft(inGameStatsUI.heistTimeLeftText, TotalHeistTimeLeft);
            TotalHeistTimeLeft -= Time.deltaTime;
            return;
        }
        inGameStatsUI.heistTimeLeftText.text = "";
        TotalHeistTimeLeft = heist.totalHeistTime;
    }

    private void StartTotalTimer()
    {
        inGameStatsUI.ShowTimeLeft(inGameStatsUI.totalTimeLeftText, TotalTimeLeft);

        TotalTimeLeft -= Time.deltaTime;
    }

    private void EndGame()
    {
        if (IsGameOver)
        {
            inGameStatsUI.GameOverFeedback("Time Is Up Game Over!");
        }

        if (IsGameWin)
        {
            inGameStatsUI.GameWonFeedback("You Win!");
        }

    }

    private IEnumerator ReturnMainMenu()
    {
        gameOverAnimator.SetTrigger("isGameOver");
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 0;
        sceneController.SetScene(0);
    }

    void OnEnable()
    {
        heist.SetHeistTimerOnHeistStarted += StartHeistTimer;
    }

    void OnDisable()
    {
        heist.SetHeistTimerOnHeistStarted -= StartHeistTimer;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<XROrigin>() != null)
        {
            DynamicMoveProvider dynamicMoveProvider = other.GetComponent<DynamicMoveProvider>();
            dynamicMoveProvider.moveSpeed = 0f;
            inGameStatsUI.GameOverFeedback("You died!");
            StartCoroutine(ReturnMainMenu());
        }
    }
    #endregion
}