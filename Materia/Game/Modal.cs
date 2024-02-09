using ECGen.Generated.Command.UI;

namespace Materia.Game;

public unsafe class Modal<T> where T : unmanaged
{
    public T* NativePtr { get; }
    public Il2CppType Type => Il2CppType.WrapPointer(NativePtr);
    private Modal(IModal* ptr) => NativePtr = (T*)ptr;
    internal static Modal<T>? CreateInstance(IModal* ptr) => ptr != null ? new Modal<T>(ptr) : null;
    public override string ToString() => Type.FullName;
}