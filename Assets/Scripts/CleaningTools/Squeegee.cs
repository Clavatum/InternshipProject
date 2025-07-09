using UnityEngine;

public class Squeegee : MonoBehaviour, ICleaningTool
{
    [SerializeField] private ToolType toolType;
    [SerializeField] private ParticleSystem wipeEffect;
    [SerializeField] private AudioSource wipeSound;

    public ToolType ToolType => toolType;

    public void PlayParticleEffect()
    {
        wipeEffect.Play();
    }

    public void PlaySoundEffect()
    {
        wipeSound.Play();
    }
}