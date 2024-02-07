using ECGen.Generated.Command.UI;
using Materia.Utilities;

namespace Materia.Game;

public unsafe class Screen
{
    public ScreenBase<nint>* NativePtr { get; }
    public string TypeName => DebugUtil.GetTypeName(NativePtr);
    private Screen(IScreen* ptr) => NativePtr = (ScreenBase<nint>*)ptr;
    internal static Screen? CreateInstance(IScreen* ptr) => ptr != null ? new Screen(ptr) : null;
    public override string ToString() => TypeName;
}