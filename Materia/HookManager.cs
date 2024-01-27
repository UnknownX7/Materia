namespace Materia;

public sealed class HookManager : IDisposable
{
    private readonly List<IMateriaHook> hooks = new();

    public IMateriaHook<T> CreateHook<T>(nint address, T detour) where T : Delegate
    {
        var hook = new MateriaHook<T>(address, detour);
        hooks.Add(hook);
        return hook;
    }

    public void Dispose()
    {
        foreach (var hook in hooks)
            hook.Dispose();
    }
}