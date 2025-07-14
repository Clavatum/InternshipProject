using UnityEngine;

public class WindowStateMachine : MonoBehaviour
{
    public Window CurrentState { get; private set; }

    public void ChangeState(Window nextState)
    {
        Debug.Log($"Changing state from {CurrentState?.name} to {nextState?.name}");

        if (CurrentState != null)
            CurrentState.gameObject.SetActive(false);

        if (nextState != null)
            nextState.gameObject.SetActive(true);

        CurrentState = nextState;
    }
}
