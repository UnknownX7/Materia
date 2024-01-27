using Reloaded.Hooks;
using Reloaded.Hooks.Definitions;

namespace Materia;

public interface IMateriaHook
{
    public bool IsEnabled { get; }
    public nint Address { get; }
    public void Enable();
    public void Disable();
    public void Toggle();
    public void Dispose();
}

public interface IMateriaHook<out T> : IMateriaHook where T : Delegate
{
    public T Original { get; }
}

internal sealed class MateriaHook<T> : IMateriaHook<T>, IDisposable where T : Delegate
{
    private readonly IHook<T>? hook;

    public bool IsEnabled => hook?.IsHookEnabled ?? false;
    public nint Address => hook?.OriginalFunctionAddress ?? nint.Zero;
    public T Original => hook?.OriginalFunction!;

    internal MateriaHook(nint address, T detour)
    {
        hook = ReloadedHooks.Instance.CreateHook(detour, address).Activate();
        hook.Disable();
    }

    public void Enable() => hook?.Enable();
    public void Disable() => hook?.Disable();

    public void Toggle()
    {
        if (IsEnabled)
            Disable();
        else
            Enable();
    }

    public void Dispose() => hook?.Disable();
}