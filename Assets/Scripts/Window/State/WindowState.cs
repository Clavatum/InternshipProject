using UnityEngine;

public class WindowState : MonoBehaviour, IWindowState
{
    public Material MaterialToWorkOn = null;
    public string PermittedToolName = "WaterSpray";
    public WindowState NextState = null;

    public string StateName => gameObject.name;

    private void Awake()
    {
        Material material = new Material(MaterialToWorkOn);
    }

    public bool TryApplyTool(WindowStateMachine context, Clean tool)
    {
        if (tool.ToolName == PermittedToolName)
        {
            context.TransitionTo(NextState);
            return true;
        }
        return false;
    }
}
