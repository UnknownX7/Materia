using ECGen.Generated.Command.UI;

namespace Materia.Game;

public unsafe class Screen
{
    public ScreenBase<ScreenSetupParameter>* NativePtr { get; }
    public Il2CppType Type => Il2CppType.WrapPointer(NativePtr);
    protected Screen(IScreen* ptr) => NativePtr = (ScreenBase<ScreenSetupParameter>*)ptr;
    internal static Screen? CreateInstance(IScreen* ptr) => ptr != null ? new Screen(ptr) : null;
    public override string ToString() => Type.FullName;
}

public unsafe class Screen<T> : Screen where T : unmanaged
{
    public new T* NativePtr => (T*)base.NativePtr;
    private Screen(IScreen* ptr) : base(ptr) { }
    internal new static Screen<T>? CreateInstance(IScreen* ptr) => ptr != null ? new Screen<T>(ptr) : null;
}