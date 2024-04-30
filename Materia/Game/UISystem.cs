using ECGen.Generated.Command.UI;
using ECGen.Generated;
using Materia.Attributes;

namespace Materia.Game;

[Injection]
public static unsafe class UISystem
{
    public static ECGen.Generated.UISystem.StaticFields* NativePtr { get; } = (ECGen.Generated.UISystem.StaticFields*)Il2CppType<ECGen.Generated.UISystem>.Instance.NativePtr->staticFields;

    [GameSymbol("Command.UI.InformationManager$$PlayAsync", ReturnPointer = true)]
    private static delegate* unmanaged<void*, IInformationManager*, int, int, Unmanaged_String*, Unmanaged_String*, CBool, int, nint, ECGen.Generated.Cysharp.Threading.Tasks.UniTask*> informationManagerPlayAsync;
    public static ECGen.Generated.Cysharp.Threading.Tasks.UniTask* ShowInfoBanner(string message, string subMessage = "", int type = 0, bool isForceTapWait = false)
    {
        if (NativePtr->informationManager == null) return null;
        var ret = new Il2CppObject<ECGen.Generated.Cysharp.Threading.Tasks.UniTask>();
        return informationManagerPlayAsync(ret, NativePtr->informationManager, 0, type, GameInterop.CreateString(message), GameInterop.CreateString(subMessage), isForceTapWait, 2, 0);
    }
}