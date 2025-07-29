using TMPro;
using UnityEngine;

public class MainMenuStatsUI : MonoBehaviour
{
    [Header("Class References")]
    private GameStatsManager gameStatsManager;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI totalPlayedTimeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
        UpdateStatText();
    }

    private void UpdateStatText()
    {
        float totalTime = gameStatsManager.GetTotalPlayedTime();
        totalPlayedTimeText.text = $"Total Played Time: {(int)(totalTime / 60):D2}:{(int)(totalTime % 60):D2}";
        scoreText.text = $"Total Gold: {gameStatsManager.GetTotalScore()}";
    }
}