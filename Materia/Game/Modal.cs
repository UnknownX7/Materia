using ECGen.Generated;
using ECGen.Generated.Command.UI;

namespace Materia.Game;

public unsafe class Modal
{
    public ModalBase<Il2CppObject>* NativePtr { get; }
    public Il2CppType Type => Il2CppType.WrapPointer(NativePtr);
    protected Modal(IModal* ptr) => NativePtr = (ModalBase<Il2CppObject>*)ptr;
    internal static Modal? CreateInstance(IModal* ptr) => ptr != null ? new Modal(ptr) : null;
    public override string ToString() => Type.FullName;
}

public unsafe class Modal<T> : Modal where T : unmanaged
{
    public new T* NativePtr => (T*)base.NativePtr;
    private Modal(IModal* ptr) : base(ptr) { }
    internal new static Modal<T>? CreateInstance(IModal* ptr) => ptr != null ? new Modal<T>(ptr) : null;
}