using UnityEngine;
using R3;
using Reflex.Attributes;
using Reflex.Core;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;
using TMPro;
public class UnitPlacementState : BaseState
{
    [Inject] private BuildController _buildController;
    [Inject] private IUnitPlacementView _view;
    [Inject] private IUnitPlacementUI _ui;
    [Inject] private Container _container;
    [Inject] private BattleData _battleData;
    [Inject] private MapScaler _mapScaler;
    [Inject] private CameraController _cameraController;

    public override void Enter()
    {
        _view.Show();
        _mapScaler.Initialize();

        SetCam();
        SetBuildController();
        SetUI();
    }

    public override void Exit()
    {
        _battleData.SetMapScale(_mapScaler.GetScale);
        _buildController.OnGhostPlaced -= (ghost) => { };
    }

    private void UpdateGhosts()
    {
        bool hasInvalid = false;
        foreach (var ghost in _battleData.GhostList)
        {
            bool isValid = _buildController.IsWithinMap(ghost.Position);
            ghost.SetColor(isValid ? Color.green : Color.red);
            if (!isValid) hasInvalid = true;
        }
        UpdateBattleButton(hasInvalid);
    }

    private void UpdateBattleButton(bool hasInvalid)
    {
        bool hasAny = _battleData.GhostList.Count > 0;
        bool canStart = hasAny && !hasInvalid;
        _ui.SetActiveBattleButton(canStart);
    }

    private void DeleteAllGhost()
    {
        foreach (var ghost in _battleData.GhostList.ToList())
        {
            ghost.Destroy();
        }
        _battleData.RemoveAllGhost();
        UpdateBattleButton(false);
    }

    private void SetCam()
    {
        _cameraController.SetMapScaler(_mapScaler);
        _cameraController.SetEditorPosition();
    }

    private void SetBuildController()
    {
        _buildController = _container.Resolve<BuildController>();
        
        _buildController.OnGhostPlaced += (ghost) =>
        {
            _battleData.AddGhost(ghost);
            UpdateGhosts();
        };

        _buildController.OnGhostDeleted += (ghost) =>
        {
            _battleData.RemoveGhost(ghost);
            UpdateGhosts();
        };

        _buildController.Initialize();
    }

    private void SetUI()
    {
        _ui.OnScaleChanged += async (value) =>{_mapScaler.ChangeScale(value); await UniTask.WaitForFixedUpdate();UpdateGhosts();};
        _ui.OnClearGhosts += DeleteAllGhost;
        _ui.OnRotateToggleChanged += (rotate) => _battleData.SetRotateMap(rotate);
    }

}
