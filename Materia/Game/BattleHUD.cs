using ECGen.Generated.Command.Battle;

namespace Materia.Game;

public unsafe class BattleHUD
{
    private static readonly BattleHUD instance = new();
    public static BattleHUD? Instance
    {
        get
        {
            var ptr = GameInterop.GetSharedMonoBehaviourInstance<HUD>();
            if (ptr == null) return null;
            instance.NativePtr = ptr;
            return instance;
        }
    }

    public HUD* NativePtr { get; private set; }
    public HUD.Status CurrentStatus => (HUD.Status)(int)NativePtr->battleStatus->currentKey->value;
    private BattleHUD() { }
}