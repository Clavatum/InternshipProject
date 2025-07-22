using UnityEngine;

public class GameEndState : MonoBehaviour
{
    public GameStatsManager gameStatsManager;
    public InGameMenu UIManager;
    [SerializeField] private float totalTime;
    private float timeLeft;

    void Awake()
    {
        UIManager = FindAnyObjectByType<InGameMenu>();
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
    }

    void Start()
    {
        timeLeft = totalTime;
        StartTimer();
    }

    void Update()
    {
        UIManager.ShowTimeLeft(timeLeft);

        if (IsGameEnded())
        {
            gameStatsManager.CalculateScore();
            UIManager.SetFeedbackText("Game Finished!", Color.red, UIManager.EndGameSFX);
            Time.timeScale = 0;
            timeLeft = totalTime;
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
