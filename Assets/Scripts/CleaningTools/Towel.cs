using UnityEngine;

public class Towel : MonoBehaviour, ICleaningTool
{
    [SerializeField] private ToolType toolType;
    [SerializeField] private ParticleSystem dryingEffect;
    [SerializeField] private AudioSource dryingSound;

    public ToolType ToolType => toolType;

    public void PlayParticleEffect()
    {
        dryingEffect.Play();
    }

    public void PlaySoundEffect()
    {
        dryingSound.Play();
    }
}