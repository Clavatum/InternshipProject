using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private TextMeshProUGUI musicVolumeValueText;
    [SerializeField] private TextMeshProUGUI SFXVolumeValueText;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource SFXAudioSource;

    private bool isSettingsPanelActive = false;

    private float musicVolumeValue;
    private float SFXVolumeValue;
    private int changeAmount = 10;

    void Start()
    {
        musicAudioSource.Play();
        musicVolumeValue = SaveSettings.GetMusicVolume();
        SFXVolumeValue = SaveSettings.GetSFXVolume();
        ApplyVolumeValue(musicAudioSource, musicVolumeValue);
        ApplyVolumeValue(SFXAudioSource, SFXVolumeValue);
    }

    public void IsSettingsPanelActive()
    {
        isSettingsPanelActive = !isSettingsPanelActive;
    }

    public void SetSettingsPanelActiveness()
    {
        settingsPanel.SetActive(isSettingsPanelActive);
        SaveSettings.SetSFXVolume(SFXVolumeValue);
        SaveSettings.SetMusicVolume(musicVolumeValue);
        SaveSettings.Save();
    }

    public void ChangeMusicVolume(bool isIncreasing)
    {
        musicVolumeValue += isIncreasing ? changeAmount : -changeAmount;
        ApplyVolumeValue(musicAudioSource, musicVolumeValue);
    }

    public void ChangeSFXVolume(bool isIncreasing)
    {
        SFXVolumeValue += isIncreasing ? changeAmount : -changeAmount;
        ApplyVolumeValue(SFXAudioSource, SFXVolumeValue);
    }

    private void ChangeVolumeValueText()
    {
        SetBoundries();
        musicVolumeValueText.text = musicVolumeValue.ToString();
        SFXVolumeValueText.text = SFXVolumeValue.ToString();
    }

    private void SetBoundries()
    {
        if (SFXVolumeValue <= 0) { SFXVolumeValue = 0; }
        if (SFXVolumeValue >= 100f) { SFXVolumeValue = 100f; }
        if (musicVolumeValue <= 0) { musicVolumeValue = 0; }
        if (musicVolumeValue >= 100f) { musicVolumeValue = 100f; }
    }

    private void ApplyVolumeValue(AudioSource audioSource, float volume)
    {
        audioSource.volume = volume / 10;
        ChangeVolumeValueText();
    }
}