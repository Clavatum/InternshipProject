using UnityEngine;

public class WindowStateMachine : MonoBehaviour
{
    public enum State
    {
        DryDirty,
        Wet,
        ChemicallyTreated,
        Rinsed,
        Wiped,
        Drying,
        Clean
    }

    public State CurrentState { get; private set; } = State.DryDirty;
    public float Cleanliness { get; private set; }
    public float Wetness { get; private set; }

    public bool TryApplyTool(ToolType tool)
    {
        switch (CurrentState)
        {
            case State.DryDirty when tool == ToolType.WaterSpray:
                TransitionTo(State.Wet);
                Wetness = 1f;
                return true;

            case State.Wet when tool == ToolType.ChemicalSpray:
                TransitionTo(State.ChemicallyTreated);
                Wetness = 0.7f;
                return true;

            case State.ChemicallyTreated when tool == ToolType.WaterSpray:
                TransitionTo(State.Rinsed);
                Wetness = 1f;
                return true;

            case State.Rinsed when tool == ToolType.Squeegee && Wetness > 0.3f:
                TransitionTo(State.Wiped);
                Wetness -= 0.3f;
                return true;

            case State.Wiped when tool == ToolType.Towel && Cleanliness >= 0.85f:
                TransitionTo(State.Clean);
                Wetness = 0f;
                return true;

            default:
                Debug.LogWarning($"Invalid tool {tool} for state {CurrentState}");
                return false;
        }
    }

    private void TransitionTo(State newState)
    {
        Debug.Log($"State change: {CurrentState} → {newState}");
        CurrentState = newState;
    }

    public void UpdateCleanliness(float cleanliness)
    {
        Cleanliness = Mathf.Clamp01(cleanliness);
    }
}