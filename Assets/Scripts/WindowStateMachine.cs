using UnityEngine;

public class WindowStateMachine : MonoBehaviour
{
    public WindowState CurrentState;

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<WindowState>() != null)
            {
                child.gameObject.SetActive(false);
            }
        }
        CurrentState.gameObject.SetActive(true);
    }


    public void ChangeState(WindowState nextState)
    {
        Debug.Log("state changed");
        CurrentState.gameObject.SetActive(false);
        nextState.gameObject.SetActive(true);
        CurrentState = nextState;
    }
}
