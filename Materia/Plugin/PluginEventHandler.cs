using Materia.Game;

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

    public event Action<Screen>? ScreenCreated;
    public void InvokeScreenCreated(Screen screen) => ScreenCreated?.Invoke(screen);

    public event Action<Modal>? ModalCreated;
    public void InvokeModalCreated(Modal modal) => ModalCreated?.Invoke(modal);
}