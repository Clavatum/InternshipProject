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
        totalPlayedTimeText.text = $"Total Played Time: {(int)(gameStatsManager.GetTotalPlayedTime() / 60)}:{(int)(gameStatsManager.GetTotalPlayedTime() % 60)}";
        scoreText.text = $"Total Gold: {gameStatsManager.GetTotalScore()}";
        Debug.Log("stat text updated");
    }
}