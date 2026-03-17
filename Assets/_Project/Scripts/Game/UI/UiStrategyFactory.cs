using Reflex.Attributes;
using UnityEngine;

public class UiStrategyFactory
{
    [Inject] private BattleData _battleData;
    [Inject] private BallRegistry _ballRegistry;
    private const int MAX_TEAM_INDIVIDUAL_UI = 4;
    public IUiStrategy CreateUIStratege() =>_battleData.GhostList.Count <= MAX_TEAM_INDIVIDUAL_UI ? new IndividualHealthUI() : new TeamHealthUI(_ballRegistry);
}
