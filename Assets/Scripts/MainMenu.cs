using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private TextMeshProUGUI TotalPlayedTimeText;
    [SerializeField] private TextMeshProUGUI totalGoldText;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource SFXAudioSource;

    private bool isSettingsPanelActive = false;

    private float messageDuration = 1f;

    void Start()
    {
        musicAudioSource.Play();
        musicSlider.value = Settings.GetMusicVolume();
        SFXSlider.value = Settings.GetSFXVolume();
    }

    public void IsSettingsPanelActive()
    {
        isSettingsPanelActive = !isSettingsPanelActive;
    }

    public void SetSettingsPanelActiveness()
    {
        settingsPanel.SetActive(isSettingsPanelActive);
        Settings.SetSFXVolume(SFXSlider.value);
        Settings.SetMusicVolume(musicSlider.value);
        Settings.Save();
    }

    public void OnMusicVolumeChanged()
    {
        musicAudioSource.volume = musicSlider.value;
    }

    public void OnSFXVolumeChanged()
    {
        SFXAudioSource.volume = SFXSlider.value;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShopButton()
    {
        feedbackText.transform.gameObject.SetActive(true);
        feedbackText.text = "VERY SOON!";
        Invoke(nameof(ClearMessage), messageDuration);
    }

    private void ClearMessage()
    {
        feedbackText.text = "";
        feedbackText.transform.gameObject.SetActive(false);
    }
}
