using UnityEngine;
using R3;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Reflex.Attributes;

public class UnitPlacementUI : MonoBehaviour, IUnitPlacementUI
{
    [Inject] private IEventBus _eventBus;
    [SerializeField] private string _battleName = "Battle";
    private UIDocument _document;
    private Button _battlebutton;
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
        _battlebutton = root.Q<Button>(_battleName);
        _listButtonsAction.Add(_battlebutton, _eventBus.OnBattleClicked);
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
