using Palmmedia.ReportGenerator.Core;
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
    [SerializeField] private GameObject settingsPanel;

    private bool isMenuPanelActive = false;
    private bool isSettingsPanelActive = false;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private float messageDuration = 2f;

    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    public AudioClip ErrorSFX;
    public AudioClip SuccessSFX;
    public AudioClip EndGameSFX;

    void Start()
    {
        musicAudioSource.Play();
        musicSlider.value = Settings.GetMusicVolume();
        SFXSlider.value = Settings.GetSFXVolume();
    }

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

    public void SetMenuPanelActiveness()
    {
        menuPanel.SetActive(isMenuPanelActive);
    }

    public void SetSettingsPanelActiveness()
    {
        settingsPanel.SetActive(isSettingsPanelActive);
        Settings.SetMusicVolume(musicSlider.value);
        Settings.SetSFXVolume(SFXSlider.value);
        Settings.Save();
    }

    public void IsMenuPanelActive()
    {
        isMenuPanelActive = !isMenuPanelActive;
    }

    public void IsSettingsPanelActive()
    {
        isSettingsPanelActive = !isSettingsPanelActive;
    }

    public void OnMusicVolumeChanged()
    {
        musicAudioSource.volume = musicSlider.value;
    }

    public void OnSFXVolumeChanged()
    {
        SFXAudioSource.volume = SFXSlider.value;
    }
}