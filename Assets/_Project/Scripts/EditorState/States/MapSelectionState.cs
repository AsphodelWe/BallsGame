using UnityEngine;
using R3;
using Reflex.Attributes;

public class MapSelectionState : BaseState
{
    [Inject] private IMapUI _ui;
    [Inject] private IMapView _view;
    [Inject] private MapRegistry _mapRegistry;
    private int _currentIndex;
    private int _totalMaps => _mapRegistry.AllMaps.Length;
    
    public override void Enter()
    {
        ShowCurrentMap();

    _ui.OnNextClicked
        .Subscribe(_ =>
        {
            _currentIndex = (_currentIndex + 1) % _totalMaps;
            ShowCurrentMap();

        })
        .AddTo(Disposables);

    _ui.OnPreviousClicked
        .Subscribe(_ =>
        {
            _currentIndex = (_currentIndex - 1 + _totalMaps) % _totalMaps;
            ShowCurrentMap();
        })
        .AddTo(Disposables);
    }

    private void ShowCurrentMap()
    {
        _view.ShowMap(_mapRegistry.GetMap(_currentIndex));
        _mapRegistry.SelectMap(_currentIndex);
    }
}
