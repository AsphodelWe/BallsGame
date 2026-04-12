
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

    private CompositeDisposable _disposables = new();

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        SubscribeButton(root.Q<Button>("RightButton"), OnNextClicked);
        SubscribeButton(root.Q<Button>("LeftButton"), OnPreviousClicked);
        SubscribeButton(root.Q<Button>("Next"), OnNextStage);
    }

    private void SubscribeButton(Button button, Subject<Unit> subject)
    {
        if (button == null) return;

        Observable.FromEvent(
            h => button.clicked += h,
            h => button.clicked -= h)
            .Subscribe(_ => subject.OnNext(Unit.Default))
            .AddTo(_disposables);
    }

    private void OnDestroy() => _disposables.Dispose();

    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);

}
