using UnityEngine;

public class WindowState : MonoBehaviour
{
    [Header("Class References")]
    public WindowState NextState;
    private GameStatsManager gameStatsManager;
    private InGameStatsUI inGameStatsUI;

    [Header("Object References")]
    [SerializeField] private Material MaterialToWorkOn;
    public Material CopyOfMaterialToWorkOn { get; private set; } = null;
    private MeshRenderer meshRenderer;

    [SerializeField] private string PermittedToolName;
    [HideInInspector] public Color[] pixels;
    private int convertedPixelCount;

    void Awake()
    {
        #region - Caching -

        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
        gameStatsManager = FindAnyObjectByType<GameStatsManager>();
        meshRenderer = transform.GetComponentInParent<MeshRenderer>();

        #endregion

        gameStatsManager.totalDirtyWindow++;

        CopyOfMaterialToWorkOn = new Material(MaterialToWorkOn);
        if (MaterialToWorkOn.HasTexture("_Mask"))
        {
            CopyOfMaterialToWorkOn.SetTexture("_Mask", CopyTexture(MaterialToWorkOn.GetTexture("_Mask")));
            pixels = ((Texture2D)CopyOfMaterialToWorkOn.GetTexture("_Mask")).GetPixels();
        }
        meshRenderer.material = CopyOfMaterialToWorkOn;
    }

    #region - Texture -

    private Texture CopyTexture(Texture texture)
    {
        Texture2D copyTexture = new(texture.width, texture.height);
        Graphics.CopyTexture(texture, copyTexture);
        return copyTexture;
    }

    public void ChangeMaterial()
    {
        meshRenderer.material = NextState.CopyOfMaterialToWorkOn;
        gameStatsManager.totalCleanedState++;
        inGameStatsUI.UpdateScore(gameStatsManager.totalCleanedState * gameStatsManager.prizeForEachCleanedState);
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

    #endregion

    public bool CanUseTool(CleaningTool cleaningTool)
    {
        return cleaningTool.transform.parent.gameObject.name == PermittedToolName;
    }
}