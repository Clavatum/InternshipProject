using UnityEngine;

public class SprayChemicalBottle : MonoBehaviour, ICleaningTool
{
    [SerializeField] private ToolType toolType;
    [SerializeField] private ParticleSystem sprayChemicalParticles;
    [SerializeField] private AudioSource sprayChemicalSound;

    public ToolType ToolType => toolType;

    public void PlayParticleEffect()
    {
        sprayChemicalParticles.Play();
    }

    public void PlaySoundEffect()
    {
        sprayChemicalSound.Play();
    }
}