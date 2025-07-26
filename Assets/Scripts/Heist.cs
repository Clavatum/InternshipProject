using UnityEngine;
using System;

public class Heist : MonoBehaviour
{
    public static event Action<AudioClip> SetMusicOnHeistStarted;
    public static event Action<AudioClip> RemoveMusicOnHeistFinished;
    public static event Action SetHeistTimerOnHeistStarted;

    private Settings settings;
    private bool isHeistStarted = false;
    public bool IsHeistStarted => isHeistStarted;
    public float totalHeistTime = 30f;
    public AudioClip heistAudioClip;

    void Awake()
    {
        settings = FindAnyObjectByType<Settings>();
    }

    public void SetHeist()
    {
        if (!isHeistStarted)
        {
            SetMusicOnHeistStarted?.Invoke(heistAudioClip);
            SetHeistTimerOnHeistStarted?.Invoke();
            isHeistStarted = true;
            return;
        }
        RemoveMusicOnHeistFinished?.Invoke(settings.windAudioClip);
    }
}