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
    public int ModalCount => NativePtr->instancedModalInfos != null ? NativePtr->instancedModalInfos->size : 0;
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

    public Modal? GetCurrentModal<T>() where T : unmanaged
    {
        var currentModal = CurrentModal;
        return currentModal != null && Il2CppType<T>.Is(currentModal.NativePtr) ? currentModal : null;
    }

    private Modal GetModal(int i) => Modal.CreateInstance(NativePtr->instancedModalInfos->GetPointer(i)->modal)!;
}