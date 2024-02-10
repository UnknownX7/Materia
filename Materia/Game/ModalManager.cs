using ECGen.Generated;
using ECGen.Generated.Command.UI;

namespace Materia.Game;

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
    public Modal<ModalBase<Il2CppObject>>? CurrentModal => ModalCount > 0 ? GetModal<ModalBase<Il2CppObject>>(ModalCount - 1) : null;
    public IEnumerable<Modal<ModalBase<Il2CppObject>>> CurrentModals
    {
        get
        {
            for (int i = 0; i < ModalCount; i++)
                yield return GetModal(i);
        }
    }
    private ModalManager() { }

    public Modal<T>? GetCurrentModal<T>() where T : unmanaged
    {
        var currentModal = ModalCount > 0 ? NativePtr->instancedModalInfos->GetPtr(ModalCount - 1)->modal : null;
        return Il2CppType<T>.Is(currentModal) ? Modal<T>.CreateInstance(currentModal) : null;
    }

    private Modal<T> GetModal<T>(int i) where T : unmanaged => Modal<T>.CreateInstance(NativePtr->instancedModalInfos->GetPtr(i)->modal)!;
    private Modal<ModalBase<Il2CppObject>> GetModal(int i) => GetModal<ModalBase<Il2CppObject>>(i);
}