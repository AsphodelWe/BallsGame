using System;
using R3;
using UnityEngine.InputSystem;

public class KeyboardInputHandler : IInputHandler, IDisposable
{
    public event Action OnBattleRestart;
    public event Action OnExitToMenu;
    private readonly CompositeDisposable _disposables = new();
    private bool _enabled;

    public void Enable()
    {
        if (_enabled) return;
        _enabled = true;

        Observable.EveryUpdate()
            .Where(_ => _enabled)
            .Select(_ => Keyboard.current)
            .Where(k => k != null && k.rKey.wasPressedThisFrame)
            .Subscribe(k =>
            {
                if (k.leftCtrlKey.isPressed)
                    OnExitToMenu?.Invoke();
                else
                    OnBattleRestart?.Invoke();
            })
            .AddTo(_disposables);
    }

    public void Disable() => _enabled = false;

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
