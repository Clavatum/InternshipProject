using TMPro;
using UnityEngine;

public class MainMenuStatsUI : MonoBehaviour
{
    private GameStatsManager gameStatsManager;

    [SerializeField] private TextMeshProUGUI totalPlayedTimeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
    }

    void Start()
    {
        totalPlayedTimeText.text = "Total Played Time: " + (gameStatsManager.GetTotalPlayedTime() / 60 + ":" + gameStatsManager.GetTotalPlayedTime() % 60).ToString();
        scoreText.text = "Total Gold: " + gameStatsManager.GetScore().ToString();
    }
}