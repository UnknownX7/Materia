using ECGen.Generated.Command.UI;
using Materia.Attributes;
using Materia.Plugin;

namespace Materia.Game;

[Injection]
public unsafe class ModalManager
{
    private static readonly ModalManager instance = new();
    public static ModalManager? Instance
    {
        get
        {
            var ptr = UISystem.NativePtr->modalManager;
            if (ptr == null) return null;
            instance.NativePtr = ptr;
            return instance;
        }
    }

    public ECGen.Generated.Command.UI.ModalManager* NativePtr { get; private set; }
    public int ModalCount => NativePtr->instancedModalInfos is var instancedModalInfos && instancedModalInfos != null ? instancedModalInfos->size : 0;
    public Modal? CurrentModal => ModalCount > 0 ? GetModal(ModalCount - 1) : null;
    public IEnumerable<Modal> CurrentModals
    {
        get
        {
            for (int i = 0; i < ModalCount; i++)
                yield return GetModal(i);
        }
    }
    private ModalManager() { }

    public Modal<T>? GetModal<T>() where T : unmanaged
    {
        var il2CppType = Il2CppType<T>.Instance;
        var modal = CurrentModals.LastOrDefault(m => il2CppType == m.NativePtr);
        return modal != null ? Modal<T>.CreateInstance((IModal*)modal.NativePtr) : null;
    }

    public Modal<T>? GetCurrentModal<T>() where T : unmanaged
    {
        var currentModal = ModalCount > 0 ? NativePtr->instancedModalInfos->GetPtr(ModalCount - 1)->modal : null;
        return Il2CppType<T>.Instance == currentModal ? Modal<T>.CreateInstance(currentModal) : null;
    }

    public bool IsModalOpen<T>() where T : unmanaged => GetModal<T>() != null;
    public bool IsCurrentModal<T>() where T : unmanaged => GetCurrentModal<T>() != null;
    private Modal GetModal(int i) => Modal.CreateInstance(NativePtr->instancedModalInfos->GetPtr(i)->modal)!;

    private delegate void SetStatusChangeEventDelegate(ECGen.Generated.Command.UI.ModalManager* modalManager, ECGen.Generated.Command.UI.ModalManager.ModalInfo* modalInfo, nint method);
    [GameSymbol("Command.UI.ModalManager$$SetStatusChangeEvent")]
    private static IMateriaHook<SetStatusChangeEventDelegate>? SetStatusChangeEventHook;
    private static void SetStatusChangeEventDetour(ECGen.Generated.Command.UI.ModalManager* modalManager, ECGen.Generated.Command.UI.ModalManager.ModalInfo* modalInfo, nint method)
    {
        SetStatusChangeEventHook!.Original(modalManager, modalInfo, method);
        var modal = Modal.CreateInstance(modalInfo->modal)!;
        Materia.PluginManager.InvokeAll(p => p.PluginServiceManager?.EventHandler.InvokeModalCreated(modal), nameof(PluginEventHandler.ModalCreated));
    }
}