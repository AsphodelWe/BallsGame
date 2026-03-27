using UnityEngine;
using R3;
using Reflex.Attributes;
using Reflex.Core;

public class EditorState : BaseState
{
    [Inject] private IMapUI _mapUI;
    [Inject] private IMapView _mapView;
    [Inject] private BattleData _battleData;
    [Inject] private MapRegistry _mapRegistry;
    [Inject] private Container _container;
    [Inject] private IEventBus _eventBus;
    private IGameState _currentSubState;
    public override void Enter()
    {
        _currentSubState = _container.Resolve<MapSelectionState>();
        _currentSubState.Enter();
        SetStreams();
    }

    private void SetStreams()
    {
        _mapUI.OnNextStage
        .Subscribe(_ =>
        {
            SwitchToPlacementStage();
            _eventBus.OnMapBoundsReady.OnNext(_mapView.GetCollider);
        })
        .AddTo(Disposables);
    }


    private void SwitchToPlacementStage()
    {
        _battleData.SetSelectedMap(_mapRegistry.SelectedMap);
        HideMapElements();

        _currentSubState.Exit();
        _currentSubState = _container.Resolve<UnitPlacementState>();
        _currentSubState.Enter();

    }

    public override void Exit()
    {
        _currentSubState?.Exit();
        Disposables.Clear();
    }

    private void HideMapElements()
    {
        _mapUI.Hide();
        _mapView.Hide();
    }

}
