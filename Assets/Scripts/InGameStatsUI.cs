using TMPro;
using UnityEngine;

public class InGameStatsUI : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private AudioSource SFXAudioSource;
    public AudioClip ErrorSFX;
    public AudioClip SuccessSFX;
    public AudioClip EndGameSFX;

    [SerializeField] private float messageDuration = 2f;

    public void SetFeedbackText(string message, Color color, AudioClip audioClip)
    {
        feedbackText.transform.gameObject.SetActive(true);
        feedbackText.text = message;
        feedbackText.color = color;
        SFXAudioSource.clip = audioClip;
        SFXAudioSource.PlayOneShot(audioClip);
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

    private void ClearMessage()
    {
        feedbackText.text = "";
        feedbackText.transform.gameObject.SetActive(false);
    }
}