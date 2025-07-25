using UnityEngine;

public static class SaveSettings
{
    #region - Getter/Setter -

    public static void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public static float GetMusicVolume() => PlayerPrefs.GetFloat("MusicVolume", 100f);

    public static void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public static float GetSFXVolume() => PlayerPrefs.GetFloat("SFXVolume", 100f);

    #endregion
}