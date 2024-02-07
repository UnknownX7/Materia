using ECGen.Generated.Command.Enums;

namespace Materia.Game;

public unsafe class BattleSystem
{
    private static readonly BattleSystem instance = new();
    public static BattleSystem? Instance
    {
        get
        {
            var ptr = GameInterop.GetSharedMonoBehaviourInstance<ECGen.Generated.Command.Battle.BattleSystem>();
            if (ptr == null) return null;
            instance.NativePtr = ptr;
            return instance;
        }
    }

    public ECGen.Generated.Command.Battle.BattleSystem* NativePtr { get; private set; }
#pragma warning disable CA1822 // Mark members as static
    public bool IsBattling => DungeonSystem.Instance?.IsBattling ?? true;
#pragma warning restore CA1822 // Mark members as static
    public bool IsMultiplayer => NativePtr->isMulti;
    public bool IsServerside => NativePtr->usesBattleServer;
    public bool IsDefeated => IsBattling && (NativePtr->ownIsStatusDefeated->GetValue() || NativePtr->ownIsDefeated->GetValue());
    public bool IsPaused => IsBattling && NativePtr->isPause->GetValue();
    public bool IsBattleWon => IsBattling && NativePtr->battleResultType->GetValue() == BattleResultType.Win;
    public bool IsPlayingCutscene => IsBattling && NativePtr->playingCutscene->value;
    public bool IsLimitBreak => IsBattling && NativePtr->limitCombo->value;
    private BattleSystem() { }
}