using ECGen.Generated.Command.Enums;
using ECGen.Generated.Command.Work;
using Materia.Attributes;

namespace Materia.Game;

[Injection]
public static unsafe class WorkManager
{
    private static readonly ECGen.Generated.Command.Work.WorkManager.StaticFields* staticFields = (ECGen.Generated.Command.Work.WorkManager.StaticFields*)Il2CppType<ECGen.Generated.Command.Work.WorkManager>.Instance.NativePtr->staticFields;
    public static ECGen.Generated.Command.Work.WorkManager* NativePtr => staticFields->instance;

    public static AccessoryWork.AccessoryRecipeStore* GetAccessoryRecipeStore(long id) => NativePtr != null && getOrCreateAccessoryRecipeStore != null ? getOrCreateAccessoryRecipeStore(NativePtr->accessory, id, 0) : null;
    public static AccessoryWork.AccessoryStore* GetAccessoryStore(long id) => NativePtr != null && getOrCreateAccessoryStore != null ? getOrCreateAccessoryStore(NativePtr->accessory, id, 0) : null;

    public static AreaBattleWork.SoloAreaGroupCategoryStore* GetSoloAreaGroupCategoryStore(long id) => NativePtr != null && getOrCreateSoloAreaGroupCategoryStore != null ? getOrCreateSoloAreaGroupCategoryStore(NativePtr->areaBattle, id, 0) : null;
    public static AreaBattleWork.SoloAreaGroupStore* GetSoloAreaGroupStore(long id) => NativePtr != null && getOrCreateSoloAreaGroupStore != null ? getOrCreateSoloAreaGroupStore(NativePtr->areaBattle, id, 0) : null;
    public static AreaBattleWork.SoloAreaStore* GetSoloAreaStore(long id) => NativePtr != null && getOrCreateSoloAreaStore != null ? getOrCreateSoloAreaStore(NativePtr->areaBattle, id, 0) : null;
    public static AreaBattleWork.SoloAreaBattleStore* GetSoloAreaBattleStore(long id) => NativePtr != null && getOrCreateSoloAreaBattleStore != null ? getOrCreateSoloAreaBattleStore(NativePtr->areaBattle, id, 0) : null;
    public static AreaBattleWork.MultiAreaStore* GetMultiAreaStore(long id) => NativePtr != null && getOrCreateMultiAreaStore != null ? getOrCreateMultiAreaStore(NativePtr->areaBattle, id, 0) : null;
    public static AreaBattleWork.MultiAreaBattleStore* GetMultiAreaBattleStore(long id) => NativePtr != null && getOrCreateMultiAreaBattleStore != null ? getOrCreateMultiAreaBattleStore(NativePtr->areaBattle, id, 0) : null;

    public static BattleWork.BattleStore* GetBattleStore(long id) => NativePtr != null && getOrCreateBattleStore != null ? getOrCreateBattleStore(NativePtr->battle, id, 0) : null;
    public static BattleWork.BattleEnemyGroupStore* GetBattleEnemyGroupStore(long id) => NativePtr != null && getOrCreateBattleEnemyGroupStore != null ? getOrCreateBattleEnemyGroupStore(NativePtr->battle, id, 0) : null;
    public static BattleWork.BattleRareWaveEncounterStore* GetBattleRareWaveEncounterStore(long id) => NativePtr != null && getOrCreateBattleRareWaveEncounterStore != null ? getOrCreateBattleRareWaveEncounterStore(NativePtr->battle, id, 0) : null;
    public static BattleWork.RareWaveStore* GetRareWaveStore(long id) => NativePtr != null && getOrCreateRareWaveStore != null ? getOrCreateRareWaveStore(NativePtr->battle, id, 0) : null;
    public static BattleWork.BattleFieldEffectStore* GetBattleFieldEffectStore(long id) => NativePtr != null && getOrCreateBattleFieldEffectStore != null ? getOrCreateBattleFieldEffectStore(NativePtr->battle, id, 0) : null;
    public static BattleWork.BattleTalkGroupStore* GetBattleTalkGroupStore(long id) => NativePtr != null && getOrCreateBattleTalkGroupStore != null ? getOrCreateBattleTalkGroupStore(NativePtr->battle, id, 0) : null;

    public static CharacterWork.CharacterStore* GetCharacterStore(long id) => NativePtr != null && getOrCreateCharacterStore != null ? getOrCreateCharacterStore(NativePtr->character, id, 0) : null;
    public static CharacterWork.CostumeStore* GetCostumeStore(long id) => NativePtr != null && getOrCreateCostumeStore != null ? getOrCreateCostumeStore(NativePtr->character, id, 0) : null;

    public static DungeonWork.AnotherAreaStore* GetAnotherAreaStore(long id) => NativePtr != null && getOrCreateAnotherAreaStore != null ? getOrCreateAnotherAreaStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.AnotherDungeonStore* GetAnotherDungeonStore(long id) => NativePtr != null && getOrCreateAnotherDungeonStore != null ? getOrCreateAnotherDungeonStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.AnotherDungeonEntryStore* GetAnotherDungeonEntryStore(int dungeonType) => NativePtr != null && getOrCreateAnotherDungeonEntryStore != null ? getOrCreateAnotherDungeonEntryStore(NativePtr->dungeon, dungeonType, 0) : null;
    public static DungeonWork.AnotherBattleStore* GetAnotherBattleStore(long id) => NativePtr != null && getOrCreateAnotherBattleStore != null ? getOrCreateAnotherBattleStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.AnotherBossBattleStore* GetAnotherBossBattleStore(long id, long bossEnhanceStage) => NativePtr != null && getOrCreateAnotherBossBattleStore != null ? getOrCreateAnotherBossBattleStore(NativePtr->dungeon, id, bossEnhanceStage, 0) : null;
    public static DungeonWork.DungeonEntryStore* GetDungeonEntryStore(int dungeonType) => NativePtr != null && getOrCreateDungeonEntryStore != null ? getOrCreateDungeonEntryStore(NativePtr->dungeon, dungeonType, 0) : null;
    public static DungeonWork.DungeonBuffGroupStore* GetDungeonBuffGroupStore(long id) => NativePtr != null && getOrCreateDungeonBuffGroupStore != null ? getOrCreateDungeonBuffGroupStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.DungeonDramaSelectionStore* GetDungeonDramaSelectionStore(long id) => NativePtr != null && getOrCreateDungeonDramaSelectionStore != null ? getOrCreateDungeonDramaSelectionStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.DungeonMapStore* GetDungeonMapStore(long id) => NativePtr != null && getOrCreateDungeonMapStore != null ? getOrCreateDungeonMapStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.DungeonMissionStore* GetDungeonMissionStore(long id) => NativePtr != null && getOrCreateDungeonMissionStore != null ? getOrCreateDungeonMissionStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.DungeonScoreOptionGroupStore* GetDungeonScoreOptionGroupStore(long id) => NativePtr != null && getOrCreateDungeonScoreOptionGroupStore != null ? getOrCreateDungeonScoreOptionGroupStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.DungeonTreasureStore* GetDungeonTreasureStore(long id) => NativePtr != null && getOrCreateDungeonTreasureStore != null ? getOrCreateDungeonTreasureStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.DungeonTriggerLockStore* GetDungeonTriggerLockStore(long id) => NativePtr != null && getOrCreateDungeonTriggerLockStore != null ? getOrCreateDungeonTriggerLockStore(NativePtr->dungeon, id, 0) : null;
    public static DungeonWork.StoryDungeonStore* GetStoryDungeonStore(long storyEpisodeId) => NativePtr != null && getOrCreateStoryDungeonStore != null ? getOrCreateStoryDungeonStore(NativePtr->dungeon, storyEpisodeId, 0) : null;
    public static DungeonWork.StoryDungeonStore* GetCharacterStoryDungeonStore(long characterStoryEpisodeId) => NativePtr != null && getOrCreateCharacterStoryDungeonStore != null ? getOrCreateCharacterStoryDungeonStore(NativePtr->dungeon, characterStoryEpisodeId, 0) : null;
    public static DungeonWork.StoryDungeonStore* GetEventStoryDungeonStore(long eventStoryEpisodeId) => NativePtr != null && getOrCreateEventStoryDungeonStore != null ? getOrCreateEventStoryDungeonStore(NativePtr->dungeon, eventStoryEpisodeId, 0) : null;

    public static EnemyWork.EnemyStore* GetEnemyStore(long id) => NativePtr != null && getOrCreateEnemyStore != null ? getOrCreateEnemyStore(NativePtr->enemy, id, 0) : null;

    public static EventWork.EventBaseStore* GetEventBaseStore(long id) => NativePtr != null && getOrCreateEventBaseStore != null ? getOrCreateEventBaseStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventDetailStore* GetEventDetailStore(long id) => NativePtr != null && getOrCreateEventDetailStore != null ? getOrCreateEventDetailStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventDramaSelectionStore* GetEventDramaSelectionStore(long id) => NativePtr != null && getOrCreateEventDramaSelectionStore != null ? getOrCreateEventDramaSelectionStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventIdlingCollectStore* GetEventIdlingCollectStore(long id) => NativePtr != null && getOrCreateEventIdlingCollectStore != null ? getOrCreateEventIdlingCollectStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventSoloAreaGroupStore* GetEventSoloAreaGroupStore(long id) => NativePtr != null && getOrCreateEventSoloAreaGroupStore != null ? getOrCreateEventSoloAreaGroupStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventSoloAreaStore* GetEventSoloAreaStore(long id) => NativePtr != null && getOrCreateEventSoloAreaStore != null ? getOrCreateEventSoloAreaStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventSoloBattleStore* GetEventSoloBattleStore(long id) => NativePtr != null && getOrCreateEventSoloBattleStore != null ? getOrCreateEventSoloBattleStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventMultiBattleStore* GetEventMultiBattleStore(long id) => NativePtr != null && getOrCreateEventMultiBattleStore != null ? getOrCreateEventMultiBattleStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventScenarioSectionStore* GetEventScenarioSectionStore(long id) => NativePtr != null && getOrCreateEventScenarioSectionStore != null ? getOrCreateEventScenarioSectionStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventScenarioEpisodeStore* GetEventScenarioEpisodeStore(long id) => NativePtr != null && getOrCreateEventScenarioEpisodeStore != null ? getOrCreateEventScenarioEpisodeStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventScoreBattleStore* GetEventScoreBattleStore(long eventSoloBattleId) => NativePtr != null && getOrCreateEventScoreBattleStore != null ? getOrCreateEventScoreBattleStore(NativePtr->@event, eventSoloBattleId, 0) : null;
    public static EventWork.EventScoreDungeonStore* GetEventScoreDungeonStore(long id) => NativePtr != null && getOrCreateEventScoreDungeonStore != null ? getOrCreateEventScoreDungeonStore(NativePtr->@event, id, 0) : null;
    public static EventWork.EventScoreDungeonScoreGroupStore* GetEventScoreDungeonScoreGroupStore(long id) => NativePtr != null && getOrCreateEventScoreDungeonScoreGroupStore != null ? getOrCreateEventScoreDungeonScoreGroupStore(NativePtr->@event, id, 0) : null;
    public static EventWork.LibraStore* GetLibraStore(long id) => NativePtr != null && getOrCreateLibraStore != null ? getOrCreateLibraStore(NativePtr->@event, id, 0) : null;
    public static EventWork.LibraCardStore* GetLibraCardStore(long id) => NativePtr != null && getOrCreateLibraCardStore != null ? getOrCreateLibraCardStore(NativePtr->@event, id, 0) : null;
    public static EventWork.LibraCardSettingStore* GetLibraCardSettingStore(long id) => NativePtr != null && getOrCreateLibraCardSettingStore != null ? getOrCreateLibraCardSettingStore(NativePtr->@event, id, 0) : null;

    public static InstantItemWork.InstantItemStore* GetInstantItemStore(long id) => NativePtr != null && getOrCreateInstantItemStore != null ? getOrCreateInstantItemStore(NativePtr->instantItem, id, 0) : null;

    public static ItemWork.ItemStore* GetItemStore(long id) => NativePtr != null && getOrCreateItemStore != null ? getOrCreateItemStore(NativePtr->item, id, 0) : null;
    public static ItemWork.ItemCraftOptionStore* GetItemCraftOptionStore(long itemId) => NativePtr != null && getOrCreateItemCraftOptionStore != null ? getOrCreateItemCraftOptionStore(NativePtr->item, itemId, 0) : null;

    public static MateriaWork.MateriaRecipeStore* GetMateriaRecipeStore(long id) => NativePtr != null && getOrCreateMateriaRecipeStore != null ? getOrCreateMateriaRecipeStore(NativePtr->materia, id, 0) : null;
    public static MateriaWork.MasterMateriaEvolveStore* GetMateriaEvolveStore(long id) => NativePtr != null && getOrCreateMateriaEvolveStore != null ? getOrCreateMateriaEvolveStore(NativePtr->materia, id, 0) : null;
    public static MateriaWork.MasterMateriaEvolveStore* GetMateriaEvolveStore(long materiaId, long evolveCount) => NativePtr != null && getOrCreateMateriaEvolveStore2 != null ? getOrCreateMateriaEvolveStore2(NativePtr->materia, materiaId, evolveCount, 0) : null;
    public static MateriaWork.MateriaNotesSetStore* GetMateriaNotesSetStore(long notesSetId) => NativePtr != null && getOrCreateMateriaNotesSetStore != null ? getOrCreateMateriaNotesSetStore(NativePtr->materia, notesSetId, 0) : null;
    public static MateriaWork.MateriaOptionGroupStore* GetMateriaOptionGroupStore(long id) => NativePtr != null && getOrCreateMateriaOptionGroupStore != null ? getOrCreateMateriaOptionGroupStore(NativePtr->materia, id, 0) : null;
    public static MateriaWork.MateriaParameterCountGroupStore* GetMateriaParameterCountGroupStore(long id) => NativePtr != null && getOrCreateMateriaParameterCountGroupStore != null ? getOrCreateMateriaParameterCountGroupStore(NativePtr->materia, id, 0) : null;
    public static MateriaWork.MateriaQualityLotGroupStore* GetMateriaQualityLotGroupStore(long id) => NativePtr != null && getOrCreateMateriaQualityLotGroupStore != null ? getOrCreateMateriaQualityLotGroupStore(NativePtr->materia, id, 0) : null;
    public static MateriaWork.MateriaSupportStore* GetMateriaSupportStore(long id) => NativePtr != null && getOrCreateMateriaSupportStore != null ? getOrCreateMateriaSupportStore(NativePtr->materia, id, 0) : null;

    public static MissionWork.MissionPanelMissionGroupStore* GetMissionPanelMissionGroupStore(long missionPanelId, int idx) => NativePtr != null && getOrCreateMissionPanelMissionGroupStore != null ? getOrCreateMissionPanelMissionGroupStore(NativePtr->mission, missionPanelId, idx, 0) : null;
    public static MissionWork.MissionPanelStore* GetMissionPanelStore(long id) => NativePtr != null && getOrCreateMissionPanelStore != null ? getOrCreateMissionPanelStore(NativePtr->mission, id, 0) : null;
    public static MissionWork.MissionSetStore* GetMissionSetStore(long id) => NativePtr != null && getOrCreateMissionSetStore != null ? getOrCreateMissionSetStore(NativePtr->mission, id, 0) : null;
    public static MissionWork.MissionStore* GetMissionStore(long id) => NativePtr != null && getOrCreateMissionStore != null ? getOrCreateMissionStore(NativePtr->mission, id, 0) : null;
    public static MissionWork.MissionProgressStore* GetMissionProgressStore(long missionId, long progressCount) => NativePtr != null && getOrCreateMissionProgressStore != null ? getOrCreateMissionProgressStore(NativePtr->mission, missionId, progressCount, 0) : null;
    public static MissionWork.MissionBonusStore* GetMissionBonusStore(long id) => NativePtr != null && getOrCreateMissionBonusStore != null ? getOrCreateMissionBonusStore(NativePtr->mission, id, 0) : null;
    public static MissionWork.MissionGuideUsageGroupStore* GetMissionGuideUsageGroupStore(long id) => NativePtr != null && getOrCreateMissionGuideUsageGroupInfo != null ? getOrCreateMissionGuideUsageGroupInfo(NativePtr->mission, id, 0) : null;
    public static MissionWork.MissionGuideUsageStore* GetMissionGuideUsageStore(int missionType) => NativePtr != null && getOrCreateMissionGuideUsageInfo != null ? getOrCreateMissionGuideUsageInfo(NativePtr->mission, missionType, 0) : null;

    public static PartyWork.PartyGroupStore* GetPartyGroupStore(long id) => NativePtr != null && getOrCreatePartyGroupStore != null ? getOrCreatePartyGroupStore(NativePtr->party, id, 0) : null;
    public static PartyWork.PartyStore* GetPartyStore(long id, bool createDefault) => NativePtr != null && getOrCreatePartyStore != null ? getOrCreatePartyStore(NativePtr->party, id, createDefault, 0) : null;
    public static PartyWork.PartyCharacterStore* GetPartyCharacterStore(long partyMemberId, long idx, bool createDefault) => NativePtr != null && getOrCreatePartyCharacterStore != null ? getOrCreatePartyCharacterStore(NativePtr->party, partyMemberId, idx, createDefault, 0) : null;
    public static PartyCharacterInfo* GetStatusParamInfo(PartyCharacterInfo* info) => NativePtr != null && getStatusParamInfo != null ? getStatusParamInfo(NativePtr->party, info, 0) : null;

    public static ResetWork.ResetStore* GetResetStore(long id) => NativePtr != null && getOrCreateResetStore != null ? getOrCreateResetStore(NativePtr->reset, id, 0) : null;
    public static TimeSpan GetTimeUntilReset(long resetId)
    {
        if (NativePtr == null || getRemainingMillSecond == null) return TimeSpan.Zero;

        var resetStore = GetResetStore(resetId);
        if (resetStore == null || resetStore->cycleType != ConditionCycleType.Month) return TimeSpan.FromMilliseconds(getRemainingMillSecond(NativePtr->reset, resetId, 0));

        // TODO: Fix
        var utcNow = DateTimeOffset.UtcNow;
        var offsetUtcNow = utcNow.AddMilliseconds(-resetStore->masterReset->resetOffsetDatetime);
        var nextMonth = new DateTimeOffset(offsetUtcNow.Year, offsetUtcNow.Month + 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(resetStore->masterReset->resetOffsetDatetime);
        return nextMonth - utcNow;
    }

    public static RewardWork.ConsumptionSetConsumptionRelStore* GetConsumptionSetConsumptionRelStore(long id) => NativePtr != null && getOrCreateConsumptionSetConsumptionRelStore != null ? getOrCreateConsumptionSetConsumptionRelStore(NativePtr->reward, id, 0) : null;
    public static RewardWork.RewardSetRewardRelStore* GetRewardSetRewardRelStore(long id) => NativePtr != null && getOrCreateRewardSetRewardRelStore != null ? getOrCreateRewardSetRewardRelStore(NativePtr->reward, id, 0) : null;
    public static RewardWork.RewardSetStore* GetRewardSetStore(long id) => NativePtr != null && getOrCreateRewardSetStore != null ? getOrCreateRewardSetStore(NativePtr->reward, id, 0) : null;
    public static RewardWork.RewardStore* GetRewardStore(long id) => NativePtr != null && getOrCreateRewardStore != null ? getOrCreateRewardStore(NativePtr->reward, id, 0) : null;
    public static RewardWork.RewardAccessoryStore* GetRewardAccessoryStore(long rewardId) => NativePtr != null && getOrCreateRewardAccessoryStore != null ? getOrCreateRewardAccessoryStore(NativePtr->reward, rewardId, 0) : null;
    public static RewardWork.RewardMateriaStore* GetRewardMateriaStore(long rewardId) => NativePtr != null && getOrCreateRewardMateriaStore != null ? getOrCreateRewardMateriaStore(NativePtr->reward, rewardId, 0) : null;
    public static RewardWork.RewardWeaponStore* GetRewardWeaponStore(long rewardId) => NativePtr != null && getOrCreateRewardWeaponStore != null ? getOrCreateRewardWeaponStore(NativePtr->reward, rewardId, 0) : null;

    public static ShopWork.ShopGroupStore* GetShopGroupStore(long id) => NativePtr != null && getOrCreateMasterShopGroupStore != null ? getOrCreateMasterShopGroupStore(NativePtr->shop, id, 0) : null;
    public static ShopWork.ShopStore* GetShopStore(long id) => NativePtr != null && getOrCreateMasterShopStore != null ? getOrCreateMasterShopStore(NativePtr->shop, id, 0) : null;
    public static ShopWork.ShopItemStore* GetShopItemStore(long id) => NativePtr != null && getOrCreateMasterShopItemStore != null ? getOrCreateMasterShopItemStore(NativePtr->shop, id, 0) : null;
    public static ShopWork.ShopPickupItemStore* GetShopPickupItemStore(long id) => NativePtr != null && getOrCreateMasterShopPickupItemStore != null ? getOrCreateMasterShopPickupItemStore(NativePtr->shop, id, 0) : null;

    public static SkillWork.BaseSkillStore* GetBaseSkillStore(long id, long skillLevel, int skillCoefficient, int damageCoefficient, long skillEffectGroupId) => NativePtr != null && getOrCreateBaseSkillStore != null ? getOrCreateBaseSkillStore(NativePtr->skill, id, skillLevel, skillCoefficient, damageCoefficient, skillEffectGroupId, 0) : null;
    public static SkillWork.NotesSetGroupSeriesStore* GetNotesSetGroupSeriesStore(long id) => NativePtr != null && getOrCreateNotesSetGroupSeriesStore != null ? getOrCreateNotesSetGroupSeriesStore(NativePtr->skill, id, 0) : null;
    public static SkillWork.NotesStore* GetNotesStore(long id) => NativePtr != null && getOrCreateNotesStore != null ? getOrCreateNotesStore(NativePtr->skill, id, 0) : null;
    public static SkillWork.PassiveSkillStore* GetPassiveSkillStore(long id) => NativePtr != null && getOrCreatePassiveSkillStore != null ? getOrCreatePassiveSkillStore(NativePtr->skill, id, 0) : null;
    public static SkillWork.SpecialSkillStore* GetSpecialSkillStore(long id, int skillLevel) => NativePtr != null && getOrCreateSpecialSkillInfo != null ? getOrCreateSpecialSkillInfo(NativePtr->skill, id, skillLevel, 0) : null;
    public static SkillWork.SpecialSkillStore* GetUserSpecialSkillStore(long specialSkillId, int skillLevel) => NativePtr != null && getOrCreateUserSpecialSkillInfo != null ? getOrCreateUserSpecialSkillInfo(NativePtr->skill, specialSkillId, skillLevel, 0) : null;
    public static SkillWork.EnemyActiveSkillStore* GetEnemyActiveSkillStore(long id, int skillLevel) => NativePtr != null && getOrCreateEnemyActiveSkillStore != null ? getOrCreateEnemyActiveSkillStore(NativePtr->skill, id, skillLevel, 0) : null;

    public static StatusStreamWork.StatusStreamGroupStore* GetStatusStreamGroupStore(long id) => NativePtr != null && getOrCreateStatusStreamGroupStore != null ? getOrCreateStatusStreamGroupStore(NativePtr->statusStream, id, 0) : null;
    public static StatusStreamWork.StatusStreamNodeStore* GetStatusStreamNodeStore(long id) => NativePtr != null && getOrCreateStatusStreamNodeStore != null ? getOrCreateStatusStreamNodeStore(NativePtr->statusStream, id, 0) : null;

    public static StoryWork.StoryTitleStore* GetStoryTitleStore(long id) => NativePtr != null && getOrCreateStoryTitleStore != null ? getOrCreateStoryTitleStore(NativePtr->story, id, 0) : null;
    public static StoryWork.StorySectionStore* GetStorySectionStore(long id) => NativePtr != null && getOrCreateStorySectionStore != null ? getOrCreateStorySectionStore(NativePtr->story, id, 0) : null;
    public static StoryWork.StoryEpisodeStore* GetStoryEpisodeStore(long id) => NativePtr != null && getOrCreateStoryEpisodeStore != null ? getOrCreateStoryEpisodeStore(NativePtr->story, id, 0) : null;
    public static StoryWork.StoryChapterStore* GetStoryChapterStore(long id) => NativePtr != null && getOrCreateStoryChapterStore != null ? getOrCreateStoryChapterStore(NativePtr->story, id, 0) : null;
    public static StoryWork.StoryBattleStore* GetStoryBattleStore(long id) => NativePtr != null && getOrCreateStoryBattleStore != null ? getOrCreateStoryBattleStore(NativePtr->story, id, 0) : null;
    public static StoryWork.StoryMapStore* GetStoryMapStore(long id) => NativePtr != null && getOrCreateStoryMapInfo != null ? getOrCreateStoryMapInfo(NativePtr->story, id, 0) : null;
    public static StoryWork.StoryMapPositionStore* GetStoryMapPositionStore(long id) => NativePtr != null && getOrCreateStoryMapPositionInfo != null ? getOrCreateStoryMapPositionInfo(NativePtr->story, id, 0) : null;
    public static StoryWork.StoryCharacterGroupStore* GetStoryCharacterGroupStore(long id) => NativePtr != null && getOrCreateStoryCharacterGroupInfo != null ? getOrCreateStoryCharacterGroupInfo(NativePtr->story, id, 0) : null;
    public static StoryWork.StoryCharacterGroupStore* GetStoryCharacterStore(long characterGroupId) => NativePtr != null && getOrCreateStoryCharacterInfo != null ? getOrCreateStoryCharacterInfo(NativePtr->story, characterGroupId, 0) : null;

    public static SummonWork.SummonBaseParameterStore* GetSummonBaseParameterStore(long summonId) => NativePtr != null && getOrCreateSummonBaseParameterStore != null ? getOrCreateSummonBaseParameterStore(NativePtr->summon, summonId, 0) : null;

    public static TowerWork.TowerGroupStore* GetTowerGroupStore(long id) => NativePtr != null && getOrCreateTowerGroupStore != null ? getOrCreateTowerGroupStore(NativePtr->tower, id, 0) : null;
    public static TowerWork.TowerStore* GetTowerStore(long id) => NativePtr != null && getOrCreateTowerStore != null ? getOrCreateTowerStore(NativePtr->tower, id, 0) : null;
    public static TowerWork.TowerFloorGroupStore* GetTowerFloorGroupStore(long id) => NativePtr != null && getOrCreateTowerFloorGroupStore != null ? getOrCreateTowerFloorGroupStore(NativePtr->tower, id, 0) : null;
    public static TowerWork.TowerFloorStore* GetTowerFloorStore(long id) => NativePtr != null && getOrCreateTowerFloorStore != null ? getOrCreateTowerFloorStore(NativePtr->tower, id, 0) : null;

    public static WeaponWork.WeaponStore* GetWeaponStore(long id) => NativePtr != null && getOrCreateMasterWeaponStore != null ? getOrCreateMasterWeaponStore(NativePtr->weapon, id, 0) : null;
    public static WeaponWork.WeaponNotesSetStore* GetWeaponNotesSetStore(long notesSetId) => NativePtr != null && getOrCreateWeaponNotesSetStore != null ? getOrCreateWeaponNotesSetStore(NativePtr->weapon, notesSetId, 0) : null;
    public static WeaponWork.WeaponRarityReleaseSkillStore* GetWeaponRarityReleaseSkillStore(long weaponRarityId, int releaseCount) => NativePtr != null && getOrCreateMasterWeaponRarityReleaseSkillStore != null ? getOrCreateMasterWeaponRarityReleaseSkillStore(NativePtr->weapon, weaponRarityId, releaseCount, 0) : null;
    public static WeaponWork.WeaponSkillStore* GetWeaponSkillStore(long id, long weaponReleaseParameterGroupId, long releaseCount, long level) => NativePtr != null && getOrCreateMasterWeaponSkillStore != null ? getOrCreateMasterWeaponSkillStore(NativePtr->weapon, id, weaponReleaseParameterGroupId, releaseCount, level, 0) : null;

    [GameSymbol("Command.Work.AccessoryWork$$GetOrCreateAccessoryRecipeStore")]
    private static delegate* unmanaged<AccessoryWork*, long, nint, AccessoryWork.AccessoryRecipeStore*> getOrCreateAccessoryRecipeStore;
    [GameSymbol("Command.Work.AccessoryWork$$GetOrCreateAccessoryStore")]
    private static delegate* unmanaged<AccessoryWork*, long, nint, AccessoryWork.AccessoryStore*> getOrCreateAccessoryStore;

    [GameSymbol("Command.Work.AreaBattleWork$$GetOrCreateSoloAreaGroupCategoryStore")]
    private static delegate* unmanaged<AreaBattleWork*, long, nint, AreaBattleWork.SoloAreaGroupCategoryStore*> getOrCreateSoloAreaGroupCategoryStore;
    [GameSymbol("Command.Work.AreaBattleWork$$GetOrCreateSoloAreaGroupStore")]
    private static delegate* unmanaged<AreaBattleWork*, long, nint, AreaBattleWork.SoloAreaGroupStore*> getOrCreateSoloAreaGroupStore;
    [GameSymbol("Command.Work.AreaBattleWork$$GetOrCreateSoloAreaStore")]
    private static delegate* unmanaged<AreaBattleWork*, long, nint, AreaBattleWork.SoloAreaStore*> getOrCreateSoloAreaStore;
    [GameSymbol("Command.Work.AreaBattleWork$$GetOrCreateSoloAreaBattleStore")]
    private static delegate* unmanaged<AreaBattleWork*, long, nint, AreaBattleWork.SoloAreaBattleStore*> getOrCreateSoloAreaBattleStore;
    [GameSymbol("Command.Work.AreaBattleWork$$GetOrCreateMultiAreaStore")]
    private static delegate* unmanaged<AreaBattleWork*, long, nint, AreaBattleWork.MultiAreaStore*> getOrCreateMultiAreaStore;
    [GameSymbol("Command.Work.AreaBattleWork$$GetOrCreateMultiAreaBattleStore")]
    private static delegate* unmanaged<AreaBattleWork*, long, nint, AreaBattleWork.MultiAreaBattleStore*> getOrCreateMultiAreaBattleStore;

    [GameSymbol("Command.Work.BattleWork$$GetOrCreateBattleStore")]
    private static delegate* unmanaged<BattleWork*, long, nint, BattleWork.BattleStore*> getOrCreateBattleStore;
    [GameSymbol("Command.Work.BattleWork$$GetOrCreateBattleEnemyGroupStore")]
    private static delegate* unmanaged<BattleWork*, long, nint, BattleWork.BattleEnemyGroupStore*> getOrCreateBattleEnemyGroupStore;
    [GameSymbol("Command.Work.BattleWork$$GetOrCreateBattleRareWaveEncounterStore")]
    private static delegate* unmanaged<BattleWork*, long, nint, BattleWork.BattleRareWaveEncounterStore*> getOrCreateBattleRareWaveEncounterStore;
    [GameSymbol("Command.Work.BattleWork$$GetOrCreateRareWaveStore")]
    private static delegate* unmanaged<BattleWork*, long, nint, BattleWork.RareWaveStore*> getOrCreateRareWaveStore;
    [GameSymbol("Command.Work.BattleWork$$GetOrCreateBattleFieldEffectStore")]
    private static delegate* unmanaged<BattleWork*, long, nint, BattleWork.BattleFieldEffectStore*> getOrCreateBattleFieldEffectStore;
    [GameSymbol("Command.Work.BattleWork$$GetOrCreateBattleTalkGroupStore")]
    private static delegate* unmanaged<BattleWork*, long, nint, BattleWork.BattleTalkGroupStore*> getOrCreateBattleTalkGroupStore;

    [GameSymbol("Command.Work.CharacterWork$$GetOrCreateCharacterStore")]
    private static delegate* unmanaged<CharacterWork*, long, nint, CharacterWork.CharacterStore*> getOrCreateCharacterStore;
    [GameSymbol("Command.Work.CharacterWork$$GetOrCreateCostumeStore")]
    private static delegate* unmanaged<CharacterWork*, long, nint, CharacterWork.CostumeStore*> getOrCreateCostumeStore;

    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateAnotherAreaStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.AnotherAreaStore*> getOrCreateAnotherAreaStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateAnotherDungeonStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.AnotherDungeonStore*> getOrCreateAnotherDungeonStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateAnotherDungeonEntryStore")]
    private static delegate* unmanaged<DungeonWork*, int, nint, DungeonWork.AnotherDungeonEntryStore*> getOrCreateAnotherDungeonEntryStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateAnotherBattleStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.AnotherBattleStore*> getOrCreateAnotherBattleStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateAnotherBossBattleStore")]
    private static delegate* unmanaged<DungeonWork*, long, long, nint, DungeonWork.AnotherBossBattleStore*> getOrCreateAnotherBossBattleStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateDungeonEntryStore")]
    private static delegate* unmanaged<DungeonWork*, int, nint, DungeonWork.DungeonEntryStore*> getOrCreateDungeonEntryStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateDungeonBuffGroupStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.DungeonBuffGroupStore*> getOrCreateDungeonBuffGroupStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateDungeonDramaSelectionStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.DungeonDramaSelectionStore*> getOrCreateDungeonDramaSelectionStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateDungeonMapStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.DungeonMapStore*> getOrCreateDungeonMapStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateDungeonMissionStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.DungeonMissionStore*> getOrCreateDungeonMissionStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateDungeonScoreOptionGroupStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.DungeonScoreOptionGroupStore*> getOrCreateDungeonScoreOptionGroupStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateDungeonTreasureStore_1")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.DungeonTreasureStore*> getOrCreateDungeonTreasureStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateDungeonTriggerLockStore_1")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.DungeonTriggerLockStore*> getOrCreateDungeonTriggerLockStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateStoryDungeonStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.StoryDungeonStore*> getOrCreateStoryDungeonStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateCharacterStoryDungeonStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.StoryDungeonStore*> getOrCreateCharacterStoryDungeonStore;
    [GameSymbol("Command.Work.DungeonWork$$GetOrCreateEventStoryDungeonStore")]
    private static delegate* unmanaged<DungeonWork*, long, nint, DungeonWork.StoryDungeonStore*> getOrCreateEventStoryDungeonStore;

    [GameSymbol("Command.Work.EnemyWork$$GetOrCreateEnemyStore")]
    private static delegate* unmanaged<EnemyWork*, long, nint, EnemyWork.EnemyStore*> getOrCreateEnemyStore;

    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventBaseStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventBaseStore*> getOrCreateEventBaseStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventDetailStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventDetailStore*> getOrCreateEventDetailStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventDramaSelectionStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventDramaSelectionStore*> getOrCreateEventDramaSelectionStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventIdlingCollectStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventIdlingCollectStore*> getOrCreateEventIdlingCollectStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventSoloAreaGroupStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventSoloAreaGroupStore*> getOrCreateEventSoloAreaGroupStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventSoloAreaStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventSoloAreaStore*> getOrCreateEventSoloAreaStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventSoloBattleStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventSoloBattleStore*> getOrCreateEventSoloBattleStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventMultiBattleStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventMultiBattleStore*> getOrCreateEventMultiBattleStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventScenarioSectionStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventScenarioSectionStore*> getOrCreateEventScenarioSectionStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventScenarioEpisodeStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventScenarioEpisodeStore*> getOrCreateEventScenarioEpisodeStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventScoreBattleStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventScoreBattleStore*> getOrCreateEventScoreBattleStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventScoreDungeonStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventScoreDungeonStore*> getOrCreateEventScoreDungeonStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateEventScoreDungeonScoreGroupStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.EventScoreDungeonScoreGroupStore*> getOrCreateEventScoreDungeonScoreGroupStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateLibraStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.LibraStore*> getOrCreateLibraStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateLibraCardStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.LibraCardStore*> getOrCreateLibraCardStore;
    [GameSymbol("Command.Work.EventWork$$GetOrCreateLibraCardSettingStore")]
    private static delegate* unmanaged<EventWork*, long, nint, EventWork.LibraCardSettingStore*> getOrCreateLibraCardSettingStore;

    [GameSymbol("Command.Work.InstantItemWork$$GetOrCreateInstantItemStore")]
    private static delegate* unmanaged<InstantItemWork*, long, nint, InstantItemWork.InstantItemStore*> getOrCreateInstantItemStore;

    [GameSymbol("Command.Work.ItemWork$$GetOrCreateItemStore")]
    private static delegate* unmanaged<ItemWork*, long, nint, ItemWork.ItemStore*> getOrCreateItemStore;
    [GameSymbol("Command.Work.ItemWork$$GetOrCreateItemCraftOptionStore")]
    private static delegate* unmanaged<ItemWork*, long, nint, ItemWork.ItemCraftOptionStore*> getOrCreateItemCraftOptionStore;

    [GameSymbol("Command.Work.MateriaWork$$GetOrCreateMateriaRecipeStore")]
    private static delegate* unmanaged<MateriaWork*, long, nint, MateriaWork.MateriaRecipeStore*> getOrCreateMateriaRecipeStore;
    [GameSymbol("Command.Work.MateriaWork$$GetOrCreateMateriaEvolveStore_1")]
    private static delegate* unmanaged<MateriaWork*, long, nint, MateriaWork.MasterMateriaEvolveStore*> getOrCreateMateriaEvolveStore;
    [GameSymbol("Command.Work.MateriaWork$$GetOrCreateMateriaEvolveStore")]
    private static delegate* unmanaged<MateriaWork*, long, long, nint, MateriaWork.MasterMateriaEvolveStore*> getOrCreateMateriaEvolveStore2;
    [GameSymbol("Command.Work.MateriaWork$$GetOrCreateMateriaNotesSetStore")]
    private static delegate* unmanaged<MateriaWork*, long, nint, MateriaWork.MateriaNotesSetStore*> getOrCreateMateriaNotesSetStore;
    [GameSymbol("Command.Work.MateriaWork$$GetOrCreateMateriaOptionGroupStore")]
    private static delegate* unmanaged<MateriaWork*, long, nint, MateriaWork.MateriaOptionGroupStore*> getOrCreateMateriaOptionGroupStore;
    [GameSymbol("Command.Work.MateriaWork$$GetOrCreateMateriaParameterCountGroupStore")]
    private static delegate* unmanaged<MateriaWork*, long, nint, MateriaWork.MateriaParameterCountGroupStore*> getOrCreateMateriaParameterCountGroupStore;
    [GameSymbol("Command.Work.MateriaWork$$GetOrCreateMateriaQualityLotGroupStore")]
    private static delegate* unmanaged<MateriaWork*, long, nint, MateriaWork.MateriaQualityLotGroupStore*> getOrCreateMateriaQualityLotGroupStore;
    [GameSymbol("Command.Work.MateriaWork$$GetOrCreateMateriaSupportStore")]
    private static delegate* unmanaged<MateriaWork*, long, nint, MateriaWork.MateriaSupportStore*> getOrCreateMateriaSupportStore;

    [GameSymbol("Command.Work.MissionWork$$GetOrCreateMissionPanelMissionGroupStore")]
    private static delegate* unmanaged<MissionWork*, long, int, nint, MissionWork.MissionPanelMissionGroupStore*> getOrCreateMissionPanelMissionGroupStore;
    [GameSymbol("Command.Work.MissionWork$$GetOrCreateMissionPanelStore")]
    private static delegate* unmanaged<MissionWork*, long, nint, MissionWork.MissionPanelStore*> getOrCreateMissionPanelStore;
    [GameSymbol("Command.Work.MissionWork$$GetOrCreateMissionSetStore")]
    private static delegate* unmanaged<MissionWork*, long, nint, MissionWork.MissionSetStore*> getOrCreateMissionSetStore;
    [GameSymbol("Command.Work.MissionWork$$GetOrCreateMissionStore")]
    private static delegate* unmanaged<MissionWork*, long, nint, MissionWork.MissionStore*> getOrCreateMissionStore;
    [GameSymbol("Command.Work.MissionWork$$GetOrCreateMissionProgressStore")]
    private static delegate* unmanaged<MissionWork*, long, long, nint, MissionWork.MissionProgressStore*> getOrCreateMissionProgressStore;
    [GameSymbol("Command.Work.MissionWork$$GetOrCreateMissionBonusStore")]
    private static delegate* unmanaged<MissionWork*, long, nint, MissionWork.MissionBonusStore*> getOrCreateMissionBonusStore;
    [GameSymbol("Command.Work.MissionWork$$GetOrCreateMissionGuideUsageGroupInfo")]
    private static delegate* unmanaged<MissionWork*, long, nint, MissionWork.MissionGuideUsageGroupStore*> getOrCreateMissionGuideUsageGroupInfo; // Actual return is the interface version
    [GameSymbol("Command.Work.MissionWork$$GetOrCreateMissionGuideUsageInfo")]
    private static delegate* unmanaged<MissionWork*, int, nint, MissionWork.MissionGuideUsageStore*> getOrCreateMissionGuideUsageInfo; // Actual return is the interface version

    [GameSymbol("Command.Work.PartyWork$$GetOrCreatePartyGroupStore")]
    private static delegate* unmanaged<PartyWork*, long, nint, PartyWork.PartyGroupStore*> getOrCreatePartyGroupStore;
    [GameSymbol("Command.Work.PartyWork$$GetOrCreatePartyStore")]
    private static delegate* unmanaged<PartyWork*, long, bool, nint, PartyWork.PartyStore*> getOrCreatePartyStore;
    [GameSymbol("Command.Work.PartyWork$$GetOrCreatePartyCharacterStore")]
    private static delegate* unmanaged<PartyWork*, long, long, bool, nint, PartyWork.PartyCharacterStore*> getOrCreatePartyCharacterStore;
    [GameSymbol("Command.Work.PartyWork$$GetStatusParamInfo")]
    private static delegate* unmanaged<PartyWork*, PartyCharacterInfo*, nint, PartyCharacterInfo*> getStatusParamInfo;

    [GameSymbol("Command.Work.ResetWork$$GetOrCreateResetStore")]
    private static delegate* unmanaged<ResetWork*, long, nint, ResetWork.ResetStore*> getOrCreateResetStore;
    [GameSymbol("Command.Work.ResetWork$$GetRemainingMillSecond")]
    private static delegate* unmanaged<ResetWork*, long, nint, long> getRemainingMillSecond;

    [GameSymbol("Command.Work.RewardWork$$GetOrCreateConsumptionSetConsumptionRelStore")]
    private static delegate* unmanaged<RewardWork*, long, nint, RewardWork.ConsumptionSetConsumptionRelStore*> getOrCreateConsumptionSetConsumptionRelStore;
    [GameSymbol("Command.Work.RewardWork$$GetOrCreateRewardSetRewardRelStore")]
    private static delegate* unmanaged<RewardWork*, long, nint, RewardWork.RewardSetRewardRelStore*> getOrCreateRewardSetRewardRelStore;
    [GameSymbol("Command.Work.RewardWork$$GetOrCreateRewardSetStore")]
    private static delegate* unmanaged<RewardWork*, long, nint, RewardWork.RewardSetStore*> getOrCreateRewardSetStore;
    [GameSymbol("Command.Work.RewardWork$$GetOrCreateRewardStore")]
    private static delegate* unmanaged<RewardWork*, long, nint, RewardWork.RewardStore*> getOrCreateRewardStore;
    [GameSymbol("Command.Work.RewardWork$$GetOrCreateRewardAccessoryStore")]
    private static delegate* unmanaged<RewardWork*, long, nint, RewardWork.RewardAccessoryStore*> getOrCreateRewardAccessoryStore;
    [GameSymbol("Command.Work.RewardWork$$GetOrCreateRewardMateriaStore")]
    private static delegate* unmanaged<RewardWork*, long, nint, RewardWork.RewardMateriaStore*> getOrCreateRewardMateriaStore;
    [GameSymbol("Command.Work.RewardWork$$GetOrCreateRewardWeaponStore")]
    private static delegate* unmanaged<RewardWork*, long, nint, RewardWork.RewardWeaponStore*> getOrCreateRewardWeaponStore;

    [GameSymbol("Command.Work.ShopWork$$GetOrCreateMasterShopGroupStore")]
    private static delegate* unmanaged<ShopWork*, long, nint, ShopWork.ShopGroupStore*> getOrCreateMasterShopGroupStore;
    [GameSymbol("Command.Work.ShopWork$$GetOrCreateMasterShopStore")]
    private static delegate* unmanaged<ShopWork*, long, nint, ShopWork.ShopStore*> getOrCreateMasterShopStore;
    [GameSymbol("Command.Work.ShopWork$$GetOrCreateMasterShopItemStore")]
    private static delegate* unmanaged<ShopWork*, long, nint, ShopWork.ShopItemStore*> getOrCreateMasterShopItemStore;
    [GameSymbol("Command.Work.ShopWork$$GetOrCreateMasterShopPickupItemStore")]
    private static delegate* unmanaged<ShopWork*, long, nint, ShopWork.ShopPickupItemStore*> getOrCreateMasterShopPickupItemStore;

    [GameSymbol("Command.Work.SkillWork$$GetOrCreateBaseSkillStore")]
    private static delegate* unmanaged<SkillWork*, long, long, int, int, long, nint, SkillWork.BaseSkillStore*> getOrCreateBaseSkillStore;
    [GameSymbol("Command.Work.SkillWork$$GetOrCreateNotesSetGroupSeriesStore")]
    private static delegate* unmanaged<SkillWork*, long, nint, SkillWork.NotesSetGroupSeriesStore*> getOrCreateNotesSetGroupSeriesStore;
    [GameSymbol("Command.Work.SkillWork$$GetOrCreateNotesStore")]
    private static delegate* unmanaged<SkillWork*, long, nint, SkillWork.NotesStore*> getOrCreateNotesStore;
    [GameSymbol("Command.Work.SkillWork$$GetOrCreatePassiveSkillStore")]
    private static delegate* unmanaged<SkillWork*, long, nint, SkillWork.PassiveSkillStore*> getOrCreatePassiveSkillStore;
    [GameSymbol("Command.Work.SkillWork$$GetOrCreateSpecialSkillInfo")]
    private static delegate* unmanaged<SkillWork*, long, int, nint, SkillWork.SpecialSkillStore*> getOrCreateSpecialSkillInfo;
    [GameSymbol("Command.Work.SkillWork$$GetOrCreateUserSpecialSkillInfo")]
    private static delegate* unmanaged<SkillWork*, long, int, nint, SkillWork.SpecialSkillStore*> getOrCreateUserSpecialSkillInfo;
    [GameSymbol("Command.Work.SkillWork$$GetOrCreateEnemyActiveSkillStore")]
    private static delegate* unmanaged<SkillWork*, long, nint, int, SkillWork.EnemyActiveSkillStore*> getOrCreateEnemyActiveSkillStore;

    [GameSymbol("Command.Work.StatusStreamWork$$GetOrCreateStatusStreamGroupStore")]
    private static delegate* unmanaged<StatusStreamWork*, long, nint, StatusStreamWork.StatusStreamGroupStore*> getOrCreateStatusStreamGroupStore;
    [GameSymbol("Command.Work.StatusStreamWork$$GetOrCreateStatusStreamNodeStore")]
    private static delegate* unmanaged<StatusStreamWork*, long, nint, StatusStreamWork.StatusStreamNodeStore*> getOrCreateStatusStreamNodeStore;

    [GameSymbol("Command.Work.StoryWork$$GetOrCreateStoryTitleStore")]
    private static delegate* unmanaged<StoryWork*, long, nint, StoryWork.StoryTitleStore*> getOrCreateStoryTitleStore;
    [GameSymbol("Command.Work.StoryWork$$GetOrCreateStorySectionStore")]
    private static delegate* unmanaged<StoryWork*, long, nint, StoryWork.StorySectionStore*> getOrCreateStorySectionStore;
    [GameSymbol("Command.Work.StoryWork$$GetOrCreateStoryEpisodeStore")]
    private static delegate* unmanaged<StoryWork*, long, nint, StoryWork.StoryEpisodeStore*> getOrCreateStoryEpisodeStore;
    [GameSymbol("Command.Work.StoryWork$$GetOrCreateStoryChapterStore")]
    private static delegate* unmanaged<StoryWork*, long, nint, StoryWork.StoryChapterStore*> getOrCreateStoryChapterStore;
    [GameSymbol("Command.Work.StoryWork$$GetOrCreateStoryBattleStore")]
    private static delegate* unmanaged<StoryWork*, long, nint, StoryWork.StoryBattleStore*> getOrCreateStoryBattleStore;
    [GameSymbol("Command.Work.StoryWork$$GetOrCreateStoryMapInfo")]
    private static delegate* unmanaged<StoryWork*, long, nint, StoryWork.StoryMapStore*> getOrCreateStoryMapInfo;
    [GameSymbol("Command.Work.StoryWork$$GetOrCreateStoryMapPositionInfo")]
    private static delegate* unmanaged<StoryWork*, long, nint, StoryWork.StoryMapPositionStore*> getOrCreateStoryMapPositionInfo;
    [GameSymbol("Command.Work.StoryWork$$GetOrCreateStoryCharacterGroupInfo")]
    private static delegate* unmanaged<StoryWork*, long, nint, StoryWork.StoryCharacterGroupStore*> getOrCreateStoryCharacterGroupInfo;
    [GameSymbol("Command.Work.StoryWork$$GetOrCreateStoryCharacterInfo")]
    private static delegate* unmanaged<StoryWork*, long, nint, StoryWork.StoryCharacterGroupStore*> getOrCreateStoryCharacterInfo; // Actual return is the interface version

    [GameSymbol("Command.Work.SummonWork$$GetOrCreateSummonBaseParameterStore")]
    private static delegate* unmanaged<SummonWork*, long, nint, SummonWork.SummonBaseParameterStore*> getOrCreateSummonBaseParameterStore;

    [GameSymbol("Command.Work.TowerWork$$GetOrCreateTowerGroupStore")]
    private static delegate* unmanaged<TowerWork*, long, nint, TowerWork.TowerGroupStore*> getOrCreateTowerGroupStore;
    [GameSymbol("Command.Work.TowerWork$$GetOrCreateTowerStore")]
    private static delegate* unmanaged<TowerWork*, long, nint, TowerWork.TowerStore*> getOrCreateTowerStore;
    [GameSymbol("Command.Work.TowerWork$$GetOrCreateTowerFloorGroupStore")]
    private static delegate* unmanaged<TowerWork*, long, nint, TowerWork.TowerFloorGroupStore*> getOrCreateTowerFloorGroupStore;
    [GameSymbol("Command.Work.TowerWork$$GetOrCreateTowerFloorStore")]
    private static delegate* unmanaged<TowerWork*, long, nint, TowerWork.TowerFloorStore*> getOrCreateTowerFloorStore;

    [GameSymbol("Command.Work.WeaponWork$$GetOrCreateMasterWeaponStore")]
    private static delegate* unmanaged<WeaponWork*, long, nint, WeaponWork.WeaponStore*> getOrCreateMasterWeaponStore;
    [GameSymbol("Command.Work.WeaponWork$$GetOrCreateWeaponNotesSetStore")]
    private static delegate* unmanaged<WeaponWork*, long, nint, WeaponWork.WeaponNotesSetStore*> getOrCreateWeaponNotesSetStore;
    [GameSymbol("Command.Work.WeaponWork$$GetOrCreateMasterWeaponRarityReleaseSkillStore")]
    private static delegate* unmanaged<WeaponWork*, long, int, nint, WeaponWork.WeaponRarityReleaseSkillStore*> getOrCreateMasterWeaponRarityReleaseSkillStore;
    [GameSymbol("Command.Work.WeaponWork$$GetOrCreateMasterWeaponSkillStore")]
    private static delegate* unmanaged<WeaponWork*, long, long, long, long, nint, WeaponWork.WeaponSkillStore*> getOrCreateMasterWeaponSkillStore;
}