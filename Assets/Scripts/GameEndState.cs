using UnityEngine;

public class GameEndState : MonoBehaviour
{
    public int totalDirtyWindowCount = 0;
    public int totalCleanedWindowCount = 0;

    void Update()
    {
        if (IsGameEnded())
        {
            Debug.Log("Game finished!");
        }
    }

    private bool IsGameEnded()
    {
        return totalCleanedWindowCount / 5 == totalDirtyWindowCount;
    }
}
