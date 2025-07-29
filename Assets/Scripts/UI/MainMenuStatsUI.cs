using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        /*PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetFloat("TotalPlayedTime", 0f);
        PlayerPrefs.Save();*/
        //gameStatsManager = FindAnyObjectByType<GameStatsManager>();
        UpdateStatText();
    }

    void Start()
    {
        //UpdateStatText();

    }

    private void UpdateStatText()
    {
        totalPlayedTimeText.text = $"Total Played Time: {(int)(GameStatsManager.Instance.GetTotalPlayedTime() / 60)}:{(int)(GameStatsManager.Instance.GetTotalPlayedTime() % 60)}";
        scoreText.text = $"Total Gold: {GameStatsManager.Instance.GetScore()}";
        Debug.Log("stat text updated");
    }

    #endregion
}