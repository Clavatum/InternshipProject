using UnityEngine;

public class Window : MonoBehaviour
{
    public enum CleaningStage { DryDirty, Wet, ChemicallyTreated, Rinsed, Wiped, Drying, Clean }

    [Header("Base Textures")]
    [SerializeField] private Texture2D DirtMaskBase;
    [SerializeField] private Texture2D _referenceCleanTexture;

    [Header("State Settings")]
    [SerializeField][Range(0, 1)] private float _cleanThreshold = 0.85f;
    [SerializeField][Range(0, 1)] private float _wetnessThreshold = 0.3f;

    private MeshRenderer _meshRenderer;
    private Material _material;
    private Texture2D _dirtMask;
    private CleaningStage _currentStage = CleaningStage.DryDirty;
    private float _wetness;

    public Material Material => _material;
    public Texture2D TemplateDirtMask => _dirtMask;
    public CleaningStage CurrentStage => _currentStage;
    public float CurrentCleanliness { get; private set; }

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
        InitializeDirtMask();
    }

    private void InitializeDirtMask()
    {
        _dirtMask = new Texture2D(DirtMaskBase.width, DirtMaskBase.height)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        _dirtMask.SetPixels(DirtMaskBase.GetPixels());
        _dirtMask.Apply();
        _material.SetTexture("_DirtMask", _dirtMask);
    }

    public bool CanUseTool(Clean.ToolType tool)
    {
        switch (_currentStage)
        {
            case CleaningStage.DryDirty:
                return tool == Clean.ToolType.WaterSpray;

            case CleaningStage.Wet:
                return tool == Clean.ToolType.ChemicalSpray;

            case CleaningStage.ChemicallyTreated:
                return tool == Clean.ToolType.WaterSpray;

            case CleaningStage.Rinsed:
                return tool == Clean.ToolType.Squeegee && _wetness > _wetnessThreshold;

            case CleaningStage.Wiped:
                return tool == Clean.ToolType.Towel && CurrentCleanliness >= _cleanThreshold;

            default:
                return false;
        }
    }

    public void UpdateCleaningState(Clean.ToolType usedTool)
    {
        CurrentCleanliness = CalculateCleanliness();

        switch (usedTool)
        {
            case Clean.ToolType.WaterSpray:
                _wetness = 1f;
                _currentStage = _currentStage == CleaningStage.ChemicallyTreated ?
                    CleaningStage.Rinsed : CleaningStage.Wet;
                break;

            case Clean.ToolType.ChemicalSpray:
                _wetness = 0.7f;
                _currentStage = CleaningStage.ChemicallyTreated;
                break;

            case Clean.ToolType.Squeegee:
                _wetness = Mathf.Max(0, _wetness - 0.3f);
                if (_wetness <= 0) _currentStage = CleaningStage.Wiped;
                break;

            case Clean.ToolType.Towel when CurrentCleanliness >= _cleanThreshold:
                _currentStage = CleaningStage.Clean;
                break;
        }
    }

    public float CalculateCleanliness()
    {
        if (_referenceCleanTexture == null || _dirtMask == null) return 0f;

        int matchingPixels = 0;
        Color[] currentPixels = _dirtMask.GetPixels();
        Color[] cleanPixels = _referenceCleanTexture.GetPixels();

        for (int i = 0; i < currentPixels.Length; i++)
        {
            if (Vector4.Distance(currentPixels[i], cleanPixels[i]) < 0.2f)
            {
                matchingPixels++;
            }
        }

        CurrentCleanliness = (float)matchingPixels / currentPixels.Length;
        return CurrentCleanliness;
    }
}