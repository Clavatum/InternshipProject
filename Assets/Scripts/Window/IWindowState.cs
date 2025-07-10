public interface IWindowState
{
    bool TryApplyTool(WindowStateMachine context, Clean tool);
    string StateName { get; }
}