using UnityEngine;
using R3;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Reflex.Attributes;
using System;

public class UnitPlacementUI : MonoBehaviour, IUnitPlacementUI
{
    [Inject] private IEventBus _eventBus;
    [SerializeField] private VisualTreeAsset _countryButtonTemplate;
    [SerializeField] private List<CountryConfig> _countries;

    private UIDocument _document;
    private VisualElement _root;
    private Button _battleButton;
    private Button _mapButton;
    private Button _clearButton;
    private Toggle _rotateMode;
    private Slider _slider;
    private CountryConfig _currentSelectedCountry;
    private CompositeDisposable _disposables = new();
    public Subject<float> OnScaleChanged { get; } = new();
    public Subject<CountryConfig> OnSelectCountry { get; } = new();
    public Subject<Unit> OnClearGhosts { get; } = new();
    public Subject<bool> OnRotateToggleChanged { get; } = new();

    public CountryConfig CurrentSelectedCountry => _currentSelectedCountry;

    void Awake() => Initialization();

    private void Initialization()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        SetBattleButton();
        SetMapPanel();
        SetMapSlider();
        SetClearButton();
        SetCountryContainer();
    }

    public void SetActiveBattleButton(bool flag) => _battleButton?.SetEnabled(flag);

    private void SetCountryContainer()
    {
        var container = _root.Q<VisualElement>("CountryContainer");
        if (container == null) return;

        foreach (var country in _countries)
        {
            var buttonElement = _countryButtonTemplate.CloneTree();
            var button = buttonElement.Q<Button>();
            button.style.backgroundImage = new StyleBackground(country.CountryFlag);
            container.Add(buttonElement);

            Observable.FromEvent(
                h => button.clicked += h,
                h => button.clicked -= h)
                .Subscribe(_ => OnSelectCountry.OnNext(country))
                .AddTo(_disposables);
        }
    }

    private void SetClearButton()
    {
        _clearButton = _root.Q<Button>("ClearButton");
        if (_clearButton == null) return;

        Observable.FromEvent(
            h => _clearButton.clicked += h,
            h => _clearButton.clicked -= h)
            .Subscribe(_ => OnClearGhosts.OnNext(Unit.Default))
            .AddTo(_disposables);
    }

    private void SetMapPanel()
    {
        _mapButton = _root.Q<Button>("MapPreferenceButton");
        var containerSettings = _root.Q<VisualElement>("MapPanel");

        if (_mapButton != null && containerSettings != null)
        {
            Observable.FromEvent(
                h => _mapButton.clicked += h,
                h => _mapButton.clicked -= h)
                .Subscribe(_ =>
                {
                    bool isVisible = containerSettings.style.visibility == Visibility.Visible;
                    containerSettings.style.visibility = isVisible ? Visibility.Hidden : Visibility.Visible;
                })
                .AddTo(_disposables);
        }

        _rotateMode = _root.Q<Toggle>("IsRotate");
        if (_rotateMode != null)
        {
            _rotateMode.RegisterValueChangedCallback(OnRotateChanged);
        }
    }

    private void OnRotateChanged(ChangeEvent<bool> evt) => OnRotateToggleChanged.OnNext(evt.newValue);

    private void SetBattleButton()
    {
        _battleButton = _root.Q<Button>("BattleButton");
        if (_battleButton == null) return;

        Observable.FromEvent(
            h => _battleButton.clicked += h,
            h => _battleButton.clicked -= h)
            .Subscribe(_ => _eventBus.OnBattleClicked.OnNext(Unit.Default))
            .AddTo(_disposables);
    }

    private void SetMapSlider()
    {
        _slider = _root.Q<Slider>("MapSlider");
        if (_slider == null) return;

        _slider.RegisterValueChangedCallback(OnScaleValueChanged);
    }

    private void OnScaleValueChanged(ChangeEvent<float> evt) => OnScaleChanged.OnNext(evt.newValue);

    private void OnDestroy()
    {
        if (_slider != null)
            _slider.UnregisterValueChangedCallback(OnScaleValueChanged);

        if (_rotateMode != null)
            _rotateMode.UnregisterValueChangedCallback(OnRotateChanged);

        _disposables.Dispose();

        OnScaleChanged.Dispose();
        OnSelectCountry.Dispose();
        OnClearGhosts.Dispose();
        OnRotateToggleChanged.Dispose();
    }

    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
}
