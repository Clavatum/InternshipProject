using UnityEngine;

public class GameEndState : MonoBehaviour
{
    public GameStatsManager gameStatsManager;
    [SerializeField] private float totalTime;
    private float timeLeft;

    void Start()
    {
        timeLeft = totalTime;
        StartTimer();
    }

    void Update()
    {
        //Debug.Log(StartTimer());
        if (IsGameEnded())
        {
            gameStatsManager.CalculateScore();
            Debug.Log("Game finished!");
            Debug.Log("Your Total Score: " + gameStatsManager.score);
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
