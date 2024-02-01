using ECGen.Generated;

namespace Materia.Game;

public unsafe class DungeonSystem
{
    private static readonly DungeonSystem instance = new();
    public static DungeonSystem? Instance
    {
        get
        {
            var ptr = GameInterop.GetSharedMonoBehaviourInstance<Command_Dungeon_DungeonSystem>();
            if (ptr == null) return null;
            instance.NativePtr = ptr;
            return instance;
        }
    }

    public Command_Dungeon_DungeonSystem* NativePtr { get; private set; }
    public bool IsBattling => NativePtr->encountingBattle->value;
    private DungeonSystem() { }
}