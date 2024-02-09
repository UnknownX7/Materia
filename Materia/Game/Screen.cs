using ECGen.Generated.Command.UI;

namespace Materia.Game;

public unsafe class Screen<T> where T : unmanaged
{
    public T* NativePtr { get; }
    public Il2CppType Type => Il2CppType.WrapPointer(NativePtr);
    private Screen(IScreen* ptr) => NativePtr = (T*)ptr;
    internal static Screen<T>? CreateInstance(IScreen* ptr) => ptr != null ? new Screen<T>(ptr) : null;
    public override string ToString() => Type.FullName;
}