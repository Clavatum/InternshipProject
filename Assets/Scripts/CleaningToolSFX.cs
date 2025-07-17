using UnityEngine;

public class CleaningToolSFX : MonoBehaviour
{
    private CleaningTool cleaningTool;
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
        Debug.Log("zort");
        audioSource.PlayOneShot(audioClip);
    }
}