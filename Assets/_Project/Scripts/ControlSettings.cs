using System;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlSettings : MonoBehaviour
{
    private VisualElement _root;
    private Button _closedButton;
    public Subject<Unit> OnClosed { get; } = new();
    private CompositeDisposable _disposables = new();
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        FindElement();
        SetupSubscribe();
        Hide();
    }

    private void FindElement()
    {
        _closedButton = _root.Q<Button>("ClosetButton");
    }

    private void SetupSubscribe()
    {
        Observable.FromEvent(h => _closedButton.clicked += h, h => _closedButton.clicked -= h)
            .Subscribe(_ => {OnClosed.OnNext(Unit.Default); })
            .AddTo(_disposables);
    }

    public void Show() => _root.style.display = DisplayStyle.Flex;
    public void Hide() => _root.style.display = DisplayStyle.None;

    private void OnDestroy()
    {
        _disposables?.Dispose();
        OnClosed?.Dispose();
    }
}
