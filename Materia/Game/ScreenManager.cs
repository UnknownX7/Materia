using ECGen.Generated;
using ECGen.Generated.Command.Enums;
using ECGen.Generated.Command.UI;
using ECGen.Generated.System;
using Materia.Attributes;
using Materia.Plugin;

namespace Materia.Game;

[Injection]
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
    public bool IsBlocking => NativePtr->isBlocking is var isBlocking && isBlocking != null && isBlocking->GetValue();
    public bool InTransition => NativePtr->inTransition || CurrentScreen == null;
    private ScreenManager() { }

    public Screen<T>? GetCurrentScreen<T>() where T : unmanaged
    {
        var currentScreen = NativePtr->currentScreen;
        return Il2CppType<T>.Instance == currentScreen ? Screen<T>.CreateInstance(currentScreen) : null;
    }

    public bool IsCurrentScreen<T>() where T : unmanaged => GetCurrentScreen<T>() != null;

    private delegate IScreen* CreateScreenDelegate(ECGen.Generated.Command.UI.ScreenManager* screenManager, Unmanaged_Type* screenType, int sortingOrderValueToAdd, nint method);
    [GameSymbol("Command.UI.ScreenManager$$CreateScreen")]
    private static IMateriaHook<CreateScreenDelegate>? CreateScreenHook;
    private static IScreen* CreateScreenDetour(ECGen.Generated.Command.UI.ScreenManager* screenManager, Unmanaged_Type* screenType, int sortingOrderValueToAdd, nint method)
    {
        var ret = CreateScreenHook!.Original(screenManager, screenType, sortingOrderValueToAdd, method);
        var screen = Screen.CreateInstance(ret)!;
        Materia.PluginManager.InvokeAll(p => p.PluginServiceManager?.EventHandler.InvokeScreenCreated(screen), nameof(PluginEventHandler.ScreenCreated));
        return ret;
    }

    [GameSymbol("Command.OutGame.TransitionUtility$$CanTransition")]
    private static delegate* unmanaged<TransitionType, long, nint, CBool> canTransition;
    public static bool CanTransition(TransitionType transitionType, long id = 0)
    {
        try
        {
            return canTransition(transitionType, id, 0);
        }
        catch
        {
            return false;
        }
    }

    [GameSymbol("Command.OutGame.TransitionUtility$$TransitionAsync", ReturnPointer = true)]
    private static delegate* unmanaged<ECGen.Generated.Cysharp.Threading.Tasks.UniTask<bool>*, int, TransitionType, long, void*, nint, ECGen.Generated.Cysharp.Threading.Tasks.UniTask<bool>*> transitionAsync;
    public static bool TransitionAsync(TransitionType transitionType, long id = 0, Action<bool>? continueWith = null)
    {
        try
        {
            var ret = new Il2CppObject<ECGen.Generated.Cysharp.Threading.Tasks.UniTask<bool>>();
            transitionAsync(ret, 0, transitionType, id, null, 0);
            if (continueWith != null)
                ret.Ptr->ContinueWith(continueWith);
            return ret.Ptr->source != null || (long)ret.Ptr->result != 0;
        }
        catch
        {
            return false;
        }
    }
}