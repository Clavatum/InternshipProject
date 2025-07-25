using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameStatsUI : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private AudioSource SFXAudioSource;
    private AudioClip errorSFX;
    private AudioClip successSFX;
    private AudioClip endGameSFX;
    private Button apartmentButton;

    [SerializeField] private float messageDuration = 2f;

    public bool isEnteredApartment;

    public void SuccessFeedback(string message) => SetFeedbackText(message, Color.green, successSFX);
    public void ErrorFeedback(string message) => SetFeedbackText(message, Color.red, errorSFX);
    public void EndGameFeedback(string message) => SetFeedbackText(message, Color.red, endGameSFX);

    private void SetFeedbackText(string message, Color color, AudioClip audioClip)
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
        scoreText.text = $"Score: {score}";
    }

    public void ShowTimeLeft(float timeLeft)
    {
        timeLeftText.text = $"Time Left: {(int)timeLeft / 60}:{(int)timeLeft % 60}";
    }

    private void ClearMessage()
    {
        feedbackText.text = "";
        feedbackText.transform.gameObject.SetActive(false);
    }

    public void SetOpenedWindowPanel()
    {
        if (!isEnteredApartment)
        {
            apartmentButton.gameObject.SetActive(true);
            feedbackText.text = "$Enter apartment and steal stuff";
        }
        apartmentButton.gameObject.SetActive(true);
        feedbackText.text = "$Exit apartment?";
    }

    public void ResetOpenedWindowPanel()
    {
        apartmentButton.gameObject.SetActive(false);
        ClearMessage();
    }
}