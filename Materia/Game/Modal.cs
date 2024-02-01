using ECGen.Generated;
using Materia.Utilities;

namespace Materia.Game;

public unsafe class Modal
{
    internal static Modal? CreateInstance(Command_UI_IModal* ptr) => ptr != null ? new Modal(ptr) : null;
    public Command_UI_IModal* NativePtr { get; }
    public string TypeName => DebugUtil.GetTypeName(NativePtr);
    private Modal(Command_UI_IModal* ptr) => NativePtr = ptr;
    public override string ToString() => TypeName;
}