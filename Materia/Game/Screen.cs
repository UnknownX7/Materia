using ECGen.Generated;
using Materia.Utilities;

namespace Materia.Game;

public unsafe class Screen
{
    internal static Screen? CreateInstance(Command_UI_IScreen* ptr) => ptr != null ? new Screen(ptr) : null;
    public Command_UI_IScreen* NativePtr { get; }
    public string TypeName => DebugUtil.GetTypeName(NativePtr);
    private Screen(Command_UI_IScreen* ptr) => NativePtr = ptr;
    public override string ToString() => TypeName;
}