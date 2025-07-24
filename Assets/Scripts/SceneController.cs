using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Class References")]
    private GameStatsManager gameStatsManager;

    void Awake()
    {
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
    }

    public void SetScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        gameStatsManager.SetTotalPlayedTime();
        gameStatsManager.SetScore();
        Time.timeScale = 1;
    }
}
