using UnityEngine;

public class GameEndState : MonoBehaviour
{
    private GameStatsManager gameStatsManager;
    private SceneController sceneController;
    private InGameStatsUI inGameStatsUI;
    [SerializeField] private float totalTime;
    private float timeLeft;

    void Awake()
    {
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
        sceneController = FindAnyObjectByType<SceneController>();
    }

    void Start()
    {
        timeLeft = totalTime;
        StartTimer();
    }

    void Update()
    {
        inGameStatsUI.ShowTimeLeft(timeLeft);

        if (IsGameEnded())
        {
            gameStatsManager.CalculateScore();
            inGameStatsUI.SetFeedbackText("Game Finished!", Color.red, inGameStatsUI.EndGameSFX);
            Time.timeScale = 0;
            sceneController.SetScene(0);
            timeLeft = totalTime;
            gameStatsManager.totalPlayedTime += timeLeft;
            gameStatsManager.SetTotalPlayedTime();
        }
    }

    private bool IsGameEnded()
    {
        return StartTimer() <= 0f || gameStatsManager.totalCleanedWindow == gameStatsManager.totalDirtyWindow;
    }

    public float StartTimer()
    {
        return timeLeft -= Time.deltaTime;
    }
}
