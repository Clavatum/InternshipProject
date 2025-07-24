using UnityEngine;

public class CleaningToolSFX : MonoBehaviour
{
    [Header("Class References")]
    private CleaningTool cleaningTool;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    void Awake()
    {
        cleaningTool = GetComponent<CleaningTool>();
    }

    public void PlaySFX()
    {
        if (cleaningTool.IsContinuous)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        audioSource.PlayOneShot(audioClip);
    }
}