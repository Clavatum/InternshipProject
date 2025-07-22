using UnityEngine;

public class WindowState : MonoBehaviour
{
    public Color[] pixels;
    private MeshRenderer meshRenderer;
    public Material MaterialToWorkOn;
    public Material CopyOfMaterialToWorkOn { get; private set; } = null;
    [SerializeField] private string PermittedToolName;
    public WindowState NextState;
    public GameStatsManager gameStatsManager;
    public InGameMenu UIManager;
    public string StateName => gameObject.name;
    private int convertedPixelCount;

    void Awake()
    {
        UIManager = FindAnyObjectByType<InGameMenu>();
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();

        gameStatsManager.totalDirtyWindow++;

        meshRenderer = transform.GetComponentInParent<MeshRenderer>();

        CopyOfMaterialToWorkOn = new Material(MaterialToWorkOn);
        if (MaterialToWorkOn.HasTexture("_Mask"))
        {
            CopyOfMaterialToWorkOn.SetTexture("_Mask", CopyTexture(MaterialToWorkOn.GetTexture("_Mask")));
            pixels = ((Texture2D)CopyOfMaterialToWorkOn.GetTexture("_Mask")).GetPixels();
        }
        meshRenderer.material = CopyOfMaterialToWorkOn;
    }

    private Texture CopyTexture(Texture texture)
    {
        Texture2D copyTexture = new(texture.width, texture.height);
        Graphics.CopyTexture(texture, copyTexture);
        return copyTexture;
    }

    public bool CanUseTool(CleaningTool cleaningTool)
    {
        return cleaningTool.transform.parent.gameObject.name == PermittedToolName;
    }

    public void ChangeMaterial()
    {
        meshRenderer.material = NextState.CopyOfMaterialToWorkOn;
        gameStatsManager.totalCleanedWindow++;
        UIManager.UpdateScore(gameStatsManager.totalCleanedWindow * gameStatsManager.prizeForEachCleanedWindow);
    }

    public float CalculateConvertedPercentage()
    {
        convertedPixelCount = 0;
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