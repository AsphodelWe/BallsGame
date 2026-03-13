using UnityEngine;
using R3;
using Reflex.Attributes;
using Reflex.Core;
using System.Collections.Generic;
public class UnitPlacementState : BaseState
{
    [Inject] private BuildController _buildController;
    [Inject] private IUnitPlacementView _view;
    [Inject] private Container _container;
    [Inject] private BattleData _battleData;

    public override void Enter()
    {
        _view.Show();

        _buildController = _container.Resolve<BuildController>();

        _buildController.OnGhostPlaced += (ghost) =>
        {
            _battleData.AddGhost(ghost);
        };

        _buildController.StartStream();
    }

    public override void Exit()
    {
        _buildController.OnGhostPlaced -= (ghost) => {};
    }
}
