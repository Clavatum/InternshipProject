using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private GameObject menuPanel;
    private bool isMenuPanelActive = false;
    [SerializeField] private float messageDuration = 2f;
    [SerializeField] private AudioSource InGameMenuAudioSource;

    public AudioClip ErrorSFX;
    public AudioClip SuccessSFX;
    public AudioClip EndGameSFX;

    public void SetFeedbackText(string message, Color color, AudioClip audioClip)
    {
        feedbackText.transform.gameObject.SetActive(true);
        feedbackText.text = message;
        feedbackText.color = color;
        InGameMenuAudioSource.clip = audioClip;
        InGameMenuAudioSource.PlayOneShot(audioClip);
        Invoke(nameof(ClearMessage), messageDuration);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void ShowTimeLeft(float timeLeft)
    {
        timeLeftText.text = "Time Left: " + ((int)timeLeft / 60).ToString() + "." + ((int)timeLeft % 60).ToString();
    }

    private void ClearMessage() => feedbackText.text = "";

    public void SetMenuPanelActiveness()
    {
        menuPanel.SetActive(isMenuPanelActive);
    }

    public void IsMenuPanelActive()
    {
        isMenuPanelActive = !isMenuPanelActive;
    }
}