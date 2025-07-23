using UnityEngine;

public static class Settings
{
    public static void SetMusicVolume(float volume) => PlayerPrefs.SetFloat("MusicVolume", volume);
    public static float GetMusicVolume() => PlayerPrefs.GetFloat("MusicVolume", 1f);

    public static void SetSFXVolume(float volume) => PlayerPrefs.SetFloat("SFXVolume", volume);
    public static float GetSFXVolume() => PlayerPrefs.GetFloat("SFXVolume", 1f);

    public static void Save() => PlayerPrefs.Save();
}