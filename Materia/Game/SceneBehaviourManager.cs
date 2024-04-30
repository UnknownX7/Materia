namespace Materia.Game;

public unsafe class SceneBehaviourManager
{
    private static readonly SceneBehaviourManager instance = new();
    public static SceneBehaviourManager? Instance
    {
        get
        {
            var ptr = GameInterop.GetSingletonMonoBehaviourInstance<ECGen.Generated.Command.SceneBehaviourManager>();
            if (ptr == null) return null;
            instance.NativePtr = ptr;
            return instance;
        }
    }

    public ECGen.Generated.Command.SceneBehaviourManager* NativePtr { get; private set; }
    private SceneBehaviourManager() { }

    public static SceneBehaviour? CurrentSceneBehaviour => Instance != null ? SceneBehaviour.CreateInstance(instance.NativePtr->currentSceneBehaviour->value) : null;

    public static SceneBehaviour<T>? GetCurrentSceneBehaviour<T>() where T : unmanaged
    {
        if (Instance == null) return null;
        var currentScene = instance.NativePtr->currentSceneBehaviour->value;
        return Il2CppType<T>.Instance == currentScene ? SceneBehaviour<T>.CreateInstance(currentScene) : null;
    }

    public static bool IsCurrentSceneBehaviour<T>() where T : unmanaged => GetCurrentSceneBehaviour<T>() != null;
}