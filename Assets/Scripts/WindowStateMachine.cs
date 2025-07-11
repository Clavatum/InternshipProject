using UnityEngine;

public class WindowStateMachine : MonoBehaviour
{
    public Window CurrentState { get; private set; } = null;

    public void ChangeState(Window nextState)
    {
        Debug.Log($"State change: {CurrentState?.StateName} → {nextState.StateName}");
    }
}
