using ECGen.Generated.Command.UI;

namespace Materia.Game;

public unsafe class Screen
{
    public ScreenBase<nint>* NativePtr { get; }
    public Il2CppType Type => Il2CppType.WrapPointer(NativePtr);
    private Screen(IScreen* ptr) => NativePtr = (ScreenBase<nint>*)ptr;
    internal static Screen? CreateInstance(IScreen* ptr) => ptr != null ? new Screen(ptr) : null;
    public override string ToString() => Type.FullName;
}