public interface ICleaningTool
{
    ToolType ToolType { get; }

    void PlaySoundEffect();
    void PlayParticleEffect();
}