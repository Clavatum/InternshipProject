using UnityEngine;

public class SprayWaterBottle : MonoBehaviour, ICleaningTool
{
    [SerializeField] private ToolType toolType;
    [SerializeField] private ParticleSystem sprayWaterParticles;
    [SerializeField] private AudioSource sprayWaterSound;

    public ToolType ToolType => toolType;

    public void PlayParticleEffect()
    {
        sprayWaterParticles.Play();
    }

    public void PlaySoundEffect()
    {
        sprayWaterSound.Play();
    }
}