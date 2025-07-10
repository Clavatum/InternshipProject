using UnityEngine;

public class WindowStateMachine : MonoBehaviour
{
    public const string DRY_DIRTY = "DryDirty";
    public const string WET = "Wet";
    public const string CHEMICALLY_TREATED = "ChemicallyTreated";
    public const string RINSED = "Rinsed";
    public const string WIPED = "Wiped";
    public const string CLEAN = "Clean";
    public WindowState InitialState;

    private Window Window;
    public IWindowState CurrentState { get; private set; }
    public float Wetness { get; private set; }
    public float Cleanliness => Window.CurrentCleanliness;

    void Awake()
    {
        TransitionTo(InitialState);
    }

    public bool TryApplyTool(Clean tool)
    {
        Window.CalculateCleanliness();// fix:Check the difference between next texture 
        return CurrentState.TryApplyTool(this, tool);
    }

    public void TransitionTo(IWindowState newState)
    {
        Debug.Log($"State change: {CurrentState?.StateName} → {newState.StateName}");
        CurrentState = newState;
    }

    internal void SetWetness(float value)
    {
        Wetness = Mathf.Clamp01(value);
    }

    internal bool IsCleanEnough() => Cleanliness == Window.CleanThreshold;
    internal bool IsWetEnough() => Wetness == Window.WetnessThreshold;
}