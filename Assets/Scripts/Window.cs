using UnityEngine;

public class Window : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public Material Material { get; private set; } = null;
    public Texture2D TemplateDirtMask { get; private set; } = null;
    public Texture2D DirtMaskBase { get; private set; } = null;

    void Awake()
    {
        CreateTexture();
        Material = meshRenderer.material;
    }

    private void CreateTexture()
    {
        TemplateDirtMask = new Texture2D(DirtMaskBase.width, DirtMaskBase.height);
        TemplateDirtMask.SetPixels(DirtMaskBase.GetPixels());
        TemplateDirtMask.Apply();
        Material.SetTexture("_DirtMask", TemplateDirtMask);
    }
}
