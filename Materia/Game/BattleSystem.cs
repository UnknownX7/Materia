using ECGen.Generated;

namespace Materia.Game;

public unsafe class BattleSystem
{
    private static readonly BattleSystem instance = new();
    public static BattleSystem? Instance
    {
        get
        {
            var ptr = GameInterop.GetSharedMonoBehaviourInstance<Command_Battle_BattleSystem>();
            if (ptr == null) return null;
            instance.NativePtr = ptr;
            return instance;
        }
    }

    public Command_Battle_BattleSystem* NativePtr { get; private set; }
#pragma warning disable CA1822 // Mark members as static
    public bool IsBattling => DungeonSystem.Instance?.IsBattling ?? true;
#pragma warning restore CA1822 // Mark members as static
    public bool IsMultiplayer => NativePtr->isMulti;
    public bool IsServerside => NativePtr->usesBattleServer;
    public bool IsDefeated => IsBattling && (NativePtr->ownIsStatusDefeated->GetValue<bool>() || NativePtr->ownIsDefeated->GetValue<bool>());
    public bool IsPaused => IsBattling && NativePtr->isPause->value;
    public bool IsBattleWon => IsBattling && NativePtr->battleResultType->GetValue<int>() == 1;
    public bool IsPlayingCutscene => IsBattling && NativePtr->playingCutscene->value;
    public bool IsLimitBreak => IsBattling && NativePtr->limitCombo->value;
    private BattleSystem() { }
}