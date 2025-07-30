using UnityEngine;

public class CleaningToolSFX : MonoBehaviour
{
    [Header("Class References")]
    private CleaningTool cleaningTool;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioClip audioClip;

    void Awake()
    {
        cleaningTool = GetComponent<CleaningTool>();
    }

    public void PlaySFX()
    {
        if (cleaningTool.IsContinuous)
        {
            SFXAudioSource.clip = audioClip;
            SFXAudioSource.Play();
        }
        SFXAudioSource.PlayOneShot(audioClip);
    }
}