using UnityEngine;
using R3;
using Reflex.Attributes;
using Reflex.Core;
using System.Linq;

using Cysharp.Threading.Tasks;

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
        Dispose();
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

        _buildController.OnGhostPlaced.Subscribe(OnGhostPlaced).AddTo(Disposables);
        _buildController.OnGhostDeleted.Subscribe(OnGhostDeleted).AddTo(Disposables);

        _buildController.Initialize();
    }

    private void SetUI()
    {
        _ui.OnScaleChanged.Subscribe(OnScaleChanged).AddTo(Disposables);
        _ui.OnClearGhosts.Subscribe(_ => DeleteAllGhost()).AddTo(Disposables);
        _ui.OnRotateToggleChanged.Subscribe(OnRotateToggleChanged).AddTo(Disposables);
    }

    private void OnGhostPlaced(Ghost ghost) { _battleData.AddGhost(ghost); UpdateGhosts(); }
    private void OnGhostDeleted(Ghost ghost) { _battleData.RemoveGhost(ghost); UpdateGhosts(); }
    private void OnRotateToggleChanged(bool rotate) => _battleData.SetRotateMap(rotate);
    private async void OnScaleChanged(float value)
    {
        _mapScaler.ChangeScale(value);
        await UniTask.WaitForFixedUpdate();
        UpdateGhosts();
    }

    public override void Dispose()
    {
        _buildController?.Dispose(); 
        base.Dispose();
    }
}

