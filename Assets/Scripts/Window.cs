using System.Linq;
using UnityEngine;

public class Window : MonoBehaviour
{
    public Material MaterialToWorkOn;
    public Material CopyOfMaterialToWorkOn { get; private set; } = null;

    [SerializeField] private string PermittedToolName;
    public Window NextState;
    public string StateName => gameObject.name;

    void Awake()
    {
        CopyOfMaterialToWorkOn = new Material(MaterialToWorkOn);
        CopyOfMaterialToWorkOn.SetTexture("StartTexture", MaterialToWorkOn.GetTexture("_StartTexture"));
        CopyOfMaterialToWorkOn.SetTexture("Mask", MaterialToWorkOn.GetTexture("_Mask"));
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
        Color[] pixels = ((Texture2D)CopyOfMaterialToWorkOn.GetTexture("_Mask")).GetPixels();

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