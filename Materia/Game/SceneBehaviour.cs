namespace Materia.Game;

public unsafe class SceneBehaviour
{
    public ECGen.Generated.Command.SceneBehaviour* NativePtr { get; }
    public Il2CppType Type => Il2CppType.WrapPointer(NativePtr);
    protected SceneBehaviour(ECGen.Generated.Command.SceneBehaviour* ptr) => NativePtr = ptr;
    internal static SceneBehaviour? CreateInstance(ECGen.Generated.Command.SceneBehaviour* ptr) => ptr != null ? new SceneBehaviour(ptr) : null;
    public override string ToString() => Type.FullName;
}

public unsafe class SceneBehaviour<T> : SceneBehaviour where T : unmanaged
{
    public new T* NativePtr => (T*)base.NativePtr;
    private SceneBehaviour(ECGen.Generated.Command.SceneBehaviour* ptr) : base(ptr) { }
    internal new static SceneBehaviour<T>? CreateInstance(ECGen.Generated.Command.SceneBehaviour* ptr) => ptr != null ? new SceneBehaviour<T>(ptr) : null;
}