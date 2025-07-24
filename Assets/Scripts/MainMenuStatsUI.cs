using TMPro;
using UnityEngine;

public class MainMenuStatsUI : MonoBehaviour
{
    [Header("Class References")]
    private GameStatsManager gameStatsManager;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI totalPlayedTimeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    #region - Awake/Start -

    void Awake()
    {
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
    }

    void Start()
    {
        totalPlayedTimeText.text = "Total Played Time: " + (gameStatsManager.GetTotalPlayedTime() / 60 + ":" + gameStatsManager.GetTotalPlayedTime() % 60).ToString();
        scoreText.text = "Total Gold: " + gameStatsManager.GetScore().ToString();
    }

    #endregion
}