using UnityEngine;

public class Window : MonoBehaviour
{
    [Header("Base Textures")]
    [SerializeField] private Texture2D dirtMaskBase;
    public Texture2D ReferenceCleanTexture { get; private set; } = null;
    public Texture2D DirtyWindowTexture { get; private set; } = null;
    public Texture2D WetWindowTexture { get; private set; } = null;
    public Texture2D ChemicalSprayedWindowTexture { get; private set; } = null;
    public Texture2D DryWindowTexture { get; private set; } = null;

    [Header("State Settings")]
    [SerializeField][Range(0, 1)] private float cleanThreshold = 0.85f;
    [SerializeField][Range(0, 1)] private float wetnessThreshold = 0.3f;

    private MeshRenderer meshRenderer;
    private Material material;
    private Texture2D dirtMask;
    private WindowStateMachine stateMachine;

    private Color[] cleanPixels;
    private Color[] currentPixels;

    public Material Material => material;
    public Texture2D TemplateDirtMask => dirtMask;
    public float CurrentCleanliness { get; private set; }
    public float CleanThreshold => cleanThreshold;
    public float WetnessThreshold => wetnessThreshold;
    public float Wetness => stateMachine.Wetness;
    public string CurrentState => stateMachine.CurrentState.StateName;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        stateMachine = FindAnyObjectByType<WindowStateMachine>();
        InitializeDirtMask();
        currentPixels = dirtMask.GetPixels();
        // ReferenceCleanTexture.GetPixels();
    }

    private void InitializeDirtMask()
    {
        dirtMask = new Texture2D(dirtMaskBase.width, dirtMaskBase.height)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        dirtMask.SetPixels(dirtMaskBase.GetPixels());
        dirtMask.Apply();
        material.SetTexture("_DirtMask", dirtMask);
    }

    public bool CanUseTool(Clean tool)
    {
        return stateMachine.TryApplyTool(tool);
    }

    public void UpdateCleaningState(Clean tool)
    {
        stateMachine.TryApplyTool(tool);
    }

    public float CalculateCleanliness()
    {
        if (cleanPixels == null || currentPixels == null)
        {
            CurrentCleanliness = 0f;
            return 0f;
        }

        int matchingPixels = 0;
        float thresholdSqr = 0.2f * 0.2f;

        for (int i = 0; i < currentPixels.Length; i++)
        {
            Color current = currentPixels[i];
            Color clean = cleanPixels[i];

            float rDiff = current.r - clean.r;
            float gDiff = current.g - clean.g;
            float bDiff = current.b - clean.b;
            float aDiff = current.a - clean.a;

            float distSqr = rDiff * rDiff + gDiff * gDiff + bDiff * bDiff + aDiff * aDiff;

            if (distSqr < thresholdSqr)
            {
                matchingPixels++;
            }
        }

        CurrentCleanliness = (float)matchingPixels / currentPixels.Length;
        return CurrentCleanliness;
    }
}