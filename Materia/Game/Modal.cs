using ECGen.Generated;
using ECGen.Generated.Command.UI;

namespace Materia.Game;

public unsafe class Modal
{
    public ModalBase<Il2CppObject>* NativePtr { get; }
    public Il2CppType Type => Il2CppType.WrapPointer(NativePtr);
    private Modal(IModal* ptr) => NativePtr = (ModalBase<Il2CppObject>*)ptr;
    internal static Modal? CreateInstance(IModal* ptr) => ptr != null ? new Modal(ptr) : null;
    public override string ToString() => Type.FullName;
}