using ECGen.Generated.Command.UI;

namespace Materia.Game;

public unsafe class ScreenManager
{
    private static readonly ScreenManager instance = new();
    public static ScreenManager? Instance
    {
        get
        {
            var ptr = UISystem.NativePtr->screenManager;
            if (ptr == null || ptr->value == null) return null;
            instance.NativePtr = ptr->value;
            return instance;
        }
    }

    public ECGen.Generated.Command.UI.ScreenManager* NativePtr { get; private set; }
    public Screen<ScreenBase<ScreenSetupParameter>>? CurrentScreen => Screen<ScreenBase<ScreenSetupParameter>>.CreateInstance(NativePtr->currentScreen);
    public bool IsBlocking => NativePtr->isBlocking is var isBlocking && isBlocking != null && isBlocking->GetValue();
    private ScreenManager() { }

    public Screen<T>? GetCurrentScreen<T>() where T : unmanaged
    {
        var currentScreen = NativePtr->currentScreen;
        return Il2CppType<T>.Is(currentScreen) ? Screen<T>.CreateInstance(currentScreen) : null;
    }
}