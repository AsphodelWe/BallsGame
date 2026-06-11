using UnityEngine;
using R3;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Reflex.Attributes;
using System;
using static UnityEngine.UIElements.ScrollView;
using Cysharp.Threading.Tasks;

public class UnitPlacementUI : MonoBehaviour, IUnitPlacementUI
{
    [Inject] private IEventBus _eventBus;
    [Inject] private CountryDatabase _countries;
    [SerializeField] private VisualTreeAsset _countryButtonTemplate;
    [SerializeField] private UIDocument _settingsUIDocument;
    [SerializeField] private CountrySettingsUI _countrySettingsUI;
    [SerializeField] private MobileUnitPlacementView _mobilePlacementView;
    [SerializeField] private WebGLUnitPlacementView _webGLPlacementView;
    private UIDocument _document;
    private VisualElement _root;
    private Button _battleButton;
    private Button _mapButton;
    private Button _clearButton;
    private Button _settingsCountryButton;
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


        SetSettingsCountryButton();
    }

    public void SetActiveBattleButton(bool flag) => _battleButton?.SetEnabled(flag);

    private void SetCountryContainer()
    {
        var container = _root.Q<VisualElement>("CountryContainer");
        var scrollView = _root.Q<ScrollView>("CountryScrollView");

        if (container == null || scrollView == null) return;

        foreach (var country in _countries.AllCountries)
        {
            var buttonElement = _countryButtonTemplate.CloneTree();
            var button = buttonElement.Q<Button>();
            button.style.backgroundImage = new StyleBackground(country.CountryFlag);

/* #if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL */
            //button.clicked += () => _mobilePlacementView?.PrepareForNewGhost(country);
/* #else
            button.clicked += () => OnSelectCountry.OnNext(country);
#endif */
            button.clicked += () => _webGLPlacementView?.PrepareForNewGhost(country);

            container.Add(buttonElement);
        }
    }

    public void SpawnGhostForCountry(CountryConfig country)
    {
        OnSelectCountry.OnNext(country);
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

    private void SetSettingsCountryButton()
    {
        _settingsCountryButton = _root.Q<Button>("SettingsCountryButton");
        if (_settingsCountryButton == null) return;

        Observable.FromEvent(
        h => _settingsCountryButton.clicked += h,
        h => _settingsCountryButton.clicked -= h)
        .Subscribe(_ => OpenSettings())
        .AddTo(_disposables);

        _countrySettingsUI.OnClosed.Subscribe(_ => Show()).AddTo(_disposables);
    }

    private void OpenSettings()
    {
        _root.style.display = DisplayStyle.None;
        _settingsUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
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

    public void Hide() => _root.style.display = DisplayStyle.None;
    public void Show() => _root.style.display = DisplayStyle.Flex;
}
