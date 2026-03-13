
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using R3;
using System;

public class ChooseMapUI : MonoBehaviour, IMapUI
{
    public Subject<Unit> OnNextClicked { get; } = new();
    public Subject<Unit> OnPreviousClicked { get; } = new();
    public Subject<Unit> OnNextStage { get; } = new();
    private UIDocument _document;
    private Button _nextButton;
    private Button _previousButton;
    private Button _nextStage;
    private CompositeDisposable _disposables = new();
    private Dictionary<Button, Subject<Unit>> _listButtonsAction = new();

    void Awake()
    {
        InitializationReferences();
        SetupSubscriptions();
    }

    private void InitializationReferences()
    {
        _document = GetComponent<UIDocument>();
        var root = _document.rootVisualElement;

        _nextButton = root.Q<Button>("RightButton");
        _previousButton = root.Q<Button>("LeftButton");
        _nextStage = root.Q<Button>("Next");

        _listButtonsAction.Add(_nextButton, OnNextClicked);
        _listButtonsAction.Add(_previousButton, OnPreviousClicked);
        _listButtonsAction.Add(_nextStage, OnNextStage);
    }

    private void SetupSubscriptions()
    {
        foreach (var action in _listButtonsAction)
        {
            SetStreams(action.Key, action.Value);
        }
    }

    private void SetStreams(Button btn, Subject<Unit> action)
    {
        Observable.FromEvent(
        h => btn.clicked += h,
        h => btn.clicked -= h)
        .Subscribe(_ =>
        {
            action.OnNext(Unit.Default);
        })
        .AddTo(_disposables);
    }

    private void OnDisable() => _disposables.Clear();
    private void OnDestroy() => _disposables.Dispose();
    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);

}
