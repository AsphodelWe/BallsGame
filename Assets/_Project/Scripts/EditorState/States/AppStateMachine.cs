using UnityEngine;
using System;
using Reflex.Attributes;
using Reflex.Core;
using R3;
public enum AppMode
{
    Editor,
    Battle
}

public class AppStateMachine : MonoBehaviour
{
    [Inject] private Container _container;
    [Inject] private IEventBus _eventBus;
    private IGameState _currentState;
    private const AppMode START_MODE = AppMode.Editor;

    void Start()
    {

        SwitchState(START_MODE);

        _eventBus.OnBattleClicked
        .Subscribe(_ =>
        {
            SwitchState(AppMode.Battle);
        })
        .AddTo(this);
    }

    private void SwitchState(AppMode mode)
    {
        _currentState?.Exit();

        _currentState = mode switch
        {
            AppMode.Editor => _container.Resolve<EditorState>(),
            AppMode.Battle => _container.Resolve<BattleState>(),
            _ => throw new ArgumentException()
        };

        _currentState.Enter();
    }


}
