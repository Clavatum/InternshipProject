using UnityEngine;
using System;

public class Heist : MonoBehaviour
{
    public event Action<AudioClip> SetMusicOnHeistStarted;
    public event Action<AudioClip> RemoveMusicOnHeistFinished;
    public event Action SetHeistTimerOnHeistStarted;

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
        SetHeistTimerOnHeistStarted?.Invoke();
        if (!isHeistStarted)
        {
            isHeistStarted = true;
            SetMusicOnHeistStarted?.Invoke(heistAudioClip);
            return;
        }
        isHeistStarted = false;
        RemoveMusicOnHeistFinished?.Invoke(settings.windAudioClip);
    }
}