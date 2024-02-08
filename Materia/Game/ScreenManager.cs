using ECGen.Generated;

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
    public Screen? CurrentScreen => Screen.CreateInstance(NativePtr->currentScreen);
    private ScreenManager() { }

    public Screen? GetCurrentScreen<T>() where T : unmanaged
    {
        var currentScreen = CurrentScreen;
        return currentScreen != null && Il2CppType<T>.Is(currentScreen.NativePtr) ? currentScreen : null;
    }
}