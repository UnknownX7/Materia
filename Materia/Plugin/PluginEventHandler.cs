namespace Materia.Plugin;

public sealed class PluginEventHandler
{
    public event Action? Update;
    public void InvokeUpdate() => Update?.Invoke();

    public event Action? Draw;
    public void InvokeDraw() => Draw?.Invoke();

    public event Action? Dispose;
    public void InvokeDispose() => Dispose?.Invoke();

    public Action? ToggleMenu;
}