using ECGen.Generated;
using Materia.Utilities;

namespace Materia.Game;

public unsafe class Modal
{
    public Command_UI_ModalBase_TResult_* NativePtr { get; }
    public string TypeName => DebugUtil.GetTypeName(NativePtr);
    private Modal(Command_UI_IModal* ptr) => NativePtr = (Command_UI_ModalBase_TResult_*)ptr;
    internal static Modal? CreateInstance(Command_UI_IModal* ptr) => ptr != null ? new Modal(ptr) : null;
    public override string ToString() => TypeName;
}