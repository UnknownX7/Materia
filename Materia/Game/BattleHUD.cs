using ECGen.Generated;

namespace Materia.Game;

public unsafe class BattleHUD
{
    private static readonly BattleHUD instance = new();
    public static BattleHUD? Instance
    {
        get
        {
            var ptr = GameInterop.GetSharedMonoBehaviourInstance<Command_Battle_HUD>();
            if (ptr == null) return null;
            instance.NativePtr = ptr;
            return instance;
        }
    }

    public Command_Battle_HUD* NativePtr { get; private set; }
    public int CurrentStatus => (int)NativePtr->battleStatus->currentKey->value;
    private BattleHUD() { }
}