using ECGen.Generated;
using Materia.Utilities;

namespace Materia.Game;

public unsafe class Screen
{
    public Command_UI_ScreenBase_TScreenSetupParameter_* NativePtr { get; }
    public string TypeName => DebugUtil.GetTypeName(NativePtr);
    private Screen(Command_UI_IScreen* ptr) => NativePtr = (Command_UI_ScreenBase_TScreenSetupParameter_*)ptr;
    internal static Screen? CreateInstance(Command_UI_IScreen* ptr) => ptr != null ? new Screen(ptr) : null;
    public override string ToString() => TypeName;
}