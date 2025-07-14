using System.Linq;
using UnityEngine;

public class Window : MonoBehaviour
{
    public Material MaterialToWorkOn;
    public Material CopyOfMaterialToWorkOn { get; private set; } = null;
    public Texture2D MaskOfMaterialToWorkOn { get; private set; } = null;
    public Texture2D StartOfMaterialToWorkOn { get; private set; } = null;
    [SerializeField] private Texture2D startTexture;
    [SerializeField] private Texture2D maskTemplate;
    [SerializeField] private string PermittedToolName;
    public Window NextState;
    public string StateName => gameObject.name;

    void Awake()
    {
        StartOfMaterialToWorkOn = Instantiate(startTexture);

        MaskOfMaterialToWorkOn = new Texture2D(maskTemplate.width, maskTemplate.height, TextureFormat.RGBA32, false);
        MaskOfMaterialToWorkOn.SetPixels(maskTemplate.GetPixels());
        MaskOfMaterialToWorkOn.Apply();

        CopyOfMaterialToWorkOn = new Material(MaterialToWorkOn);
        CopyOfMaterialToWorkOn.SetTexture("StartTexture", StartOfMaterialToWorkOn);
        CopyOfMaterialToWorkOn.SetTexture("Mask", MaskOfMaterialToWorkOn);

        GetComponentInParent<MeshRenderer>().material = CopyOfMaterialToWorkOn;
    }

    public bool CanUseTool(CleaningTool cleaningTool)
    {
        if (string.IsNullOrEmpty(PermittedToolName))
            return false;

        return cleaningTool.transform.parent.name == PermittedToolName;
    }


    public void ChangeMaterial()
    {
        if (NextState != null && NextState.MaterialToWorkOn != null)
        {
            MaterialToWorkOn = NextState.MaterialToWorkOn;
        }
    }


    public float CalculateConvertedPercentage()
    {
        CopyOfMaterialToWorkOn.SetTexture("Mask", MaskOfMaterialToWorkOn);
        Color[] pixels = MaskOfMaterialToWorkOn.GetPixels();

        int convertedPixelCount = 0;
        foreach (Color pixel in pixels)
        {
            if (CleaningTool.IsBlackEnough(pixel))
            {
                convertedPixelCount++;
            }
        }
        return (float)convertedPixelCount / pixels.Length * 100f;
    }
}