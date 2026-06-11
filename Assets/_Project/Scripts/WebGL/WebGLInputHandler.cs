using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class WebGLInputHandler : MonoBehaviour, IInputHandler, IDisposable
{
    [SerializeField] private UIDocument _mobileUI;
    public event Action OnBattleRestart;
    public event Action OnExitToMenu;
    private readonly CompositeDisposable _disposables = new();

    private bool _enabled;
    private bool _isMobile;

    void Awake()
    {
        _isMobile = DeviceDetector.IsMobile();
    }

    public void Enable()
    {
        if (_isMobile)
        {

            var root = _mobileUI.rootVisualElement;
            var container = root.Q<VisualElement>("MobileInputContainer");
            container.style.display = DisplayStyle.Flex;

            root.Q<Button>("MobileRestartButton").clicked += () => OnBattleRestart?.Invoke();
            root.Q<Button>("MobileExitButton").clicked += () => OnExitToMenu?.Invoke();
        }
        else
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
    }

    public void Disable() => _enabled = false;
    public void Dispose()
    {
        _enabled = false;
        _disposables?.Dispose();
    }
}
