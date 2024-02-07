using ECGen.Generated.Command.Dungeon;

namespace Materia.Game;

public unsafe class DungeonHUD
{
    private static readonly DungeonHUD instance = new();
    public static DungeonHUD? Instance
    {
        get
        {
            var ptr = GameInterop.GetSharedMonoBehaviourInstance<HUD>(1);
            if (ptr == null) return null;
            instance.NativePtr = ptr;
            return instance;
        }
    }

    public HUD* NativePtr { get; private set; }
    private DungeonHUD() { }
}