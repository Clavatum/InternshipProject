using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Class References")]
    private GameStatsManager gameStatsManager;
    private SceneController sceneController;
    private InGameStatsUI inGameStatsUI;

    [Space, SerializeField] private float totalTime;
    public float TimeLeft { get; private set; } = 0f;

    private bool IsGameEnded => TimeLeft <= 0f || gameStatsManager.totalCleanedState / 4 == gameStatsManager.totalDirtyWindow;

    #region - Awake/Start/Update -

    void Awake()
    {
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
        sceneController = FindAnyObjectByType<SceneController>();
    }

    void Start()
    {
        TimeLeft = totalTime;
    }

    void Update()
    {
        inGameStatsUI.ShowTimeLeft(TimeLeft);

        TimeLeft -= Time.deltaTime;

        if (IsGameEnded)
        {
            gameStatsManager.CalculateScore();
            inGameStatsUI.EndGameFeedback("Game Finished!");
            Time.timeScale = 0;
            sceneController.SetScene(0);
            TimeLeft = 0f;
            gameStatsManager.totalPlayedTime += TimeLeft;
            gameStatsManager.SetTotalPlayedTime();
        }
    }

    #endregion
}