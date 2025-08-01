using System;
using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManager : MonoBehaviour
{
    [Header("Class References")]
    private Heist heist;
    private GameStatsManager gameStatsManager;
    private SceneController sceneController;
    private InGameStatsUI inGameStatsUI;
    private Teleport teleport;
    private LeftControllerInputAction leftController;

    [SerializeField] private GameObject player;

    [Header("Player Position Boundaries")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

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
        teleport = FindAnyObjectByType<Teleport>();
        heist = FindAnyObjectByType<Heist>();
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
        sceneController = FindAnyObjectByType<SceneController>();
        gameOverAnimator = inGameStatsUI.gameOverPanel.GetComponent<Animator>();
        leftController = new LeftControllerInputAction();
        leftController.LeftController.YButton.performed += e => ReturnToElevator();
        leftController.Enable();
    }

    void Start()
    {
        TotalTimeLeft = totalTime;
        TotalHeistTimeLeft = heist.totalHeistTime;
    }

    void Update()
    {
        if (IsPlayerOutOfBoundaries())
        {
            inGameStatsUI.feedbackText.transform.gameObject.SetActive(true);
            inGameStatsUI.feedbackText.text = "To go back to elevator, press secondary button on left controller";
        }

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

    public void ReturnToElevator()
    {
        if (!IsPlayerOutOfBoundaries()) { return; }
        player.transform.localPosition = teleport.playerTransformInElevator.localPosition;
        player.transform.localRotation = teleport.playerTransformInElevator.localRotation;
        inGameStatsUI.ClearMessage();
    }

    private bool IsPlayerOutOfBoundaries()
    {
        return player.transform.localPosition.x < minX || player.transform.localPosition.x > maxX || player.transform.localPosition.y < minY || player.transform.localPosition.y > maxY;
    }

    #endregion
}