using System.Linq;
using UnityEngine;

public class Window : MonoBehaviour
{
    public Material MaterialToWorkOn;
    public Material CopyOfMaterialToWorkOn { get; private set; } = null;
    public Texture2D MaskOfMaterialToWorkOn { get; private set; } = null;
    public Texture2D StartOfMaterialToWorkOn { get; private set; } = null;
    [SerializeField] private string PermittedToolName;
    public Window NextState;
    public string StateName => gameObject.name;

    void Awake()
    {
        CopyOfMaterialToWorkOn = new Material(MaterialToWorkOn);
        CopyOfMaterialToWorkOn.SetTexture("StartTexture", StartOfMaterialToWorkOn);
    }

    public bool CanUseTool(CleaningTool cleaningTool)
    {
        return cleaningTool.transform.parent.name == PermittedToolName;
    }

    public void ChangeMaterial()
    {
        MaterialToWorkOn = NextState.MaterialToWorkOn;
    }

    public float CalculateConvertedPercentage()
    {
        CopyOfMaterialToWorkOn.SetTexture("Mask", MaskOfMaterialToWorkOn);
        Color[] maskOfMaterialToWorkOnPixels = MaskOfMaterialToWorkOn.GetPixels();
        Color[] maskOfMaterialToWorkOnBlackPixels = null;

        for (int i = 0; i < maskOfMaterialToWorkOnPixels.Length; i++)
        {
            if (CleaningTool.IsBlackEnough(maskOfMaterialToWorkOnPixels[i]))
            {
                maskOfMaterialToWorkOnBlackPixels.Append(maskOfMaterialToWorkOnPixels[i]);
            }
        }
        return maskOfMaterialToWorkOnBlackPixels.Length / maskOfMaterialToWorkOnPixels.Length * 100f;
    }
}