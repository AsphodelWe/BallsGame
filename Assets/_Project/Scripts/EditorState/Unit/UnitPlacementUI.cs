using UnityEngine;
using R3;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Reflex.Attributes;
using Unity.VisualScripting;
using System;

public class UnitPlacementUI : MonoBehaviour, IUnitPlacementUI
{
    [Inject] private IEventBus _eventBus;
    [SerializeField] private VisualTreeAsset _countryButtonTemplate;
    [SerializeField] private List<CountryConfig> _countries;
    [SerializeField] private string _battleName = "BattleButton";
    [SerializeField] private string _mapContainerName = "MapPreferenceButton";
    private UIDocument _document;
    private Button _battlebutton;
    private Button _mapButton;
    private Button _clearButton;
    private Toggle _rotateMode;
    private CompositeDisposable _disposables = new();
    private Slider _slider;
    public event Action<float> OnScaleChanged;
    public event Action<CountryConfig> OnSelectCountry;
    public event Action OnClearGhosts;
    public event Action<bool> OnRotateToggleChanged;


    void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        _document = GetComponent<UIDocument>();
        var root = _document.rootVisualElement;

        _battlebutton = root.Q<Button>(_battleName);

        _mapButton = root.Q<Button>(_mapContainerName);
        VisualElement _containerSettings = root.Q<VisualElement>("MapPanel");

        _mapButton.clicked += () =>
        {
            bool isVisible = _containerSettings.style.visibility == Visibility.Visible;
            _containerSettings.style.visibility = isVisible ? Visibility.Hidden : Visibility.Visible;
        };

        _battlebutton.clicked += () => _eventBus.OnBattleClicked.OnNext(R3.Unit.Default);

        _slider = root.Q<Slider>("MapSlider");
        _slider.RegisterValueChangedCallback(evt => OnScaleChanged?.Invoke(evt.newValue));

        _rotateMode = root.Q<Toggle>("IsRotate");
        _rotateMode.RegisterValueChangedCallback(evt => OnRotateToggleChanged?.Invoke(evt.newValue));

        _clearButton = root.Q<Button>("ClearButton");
        _clearButton.clicked += () => OnClearGhosts.Invoke();

        VisualElement container = root.Q<VisualElement>("CountryContainer");

        foreach (var country in _countries)
        {
            var buttonElement = _countryButtonTemplate.CloneTree();
            var button = buttonElement.Q<Button>();
            button.style.backgroundImage = new StyleBackground(country.CountryFlag);
            container.Add(buttonElement);

            button.clicked += () => OnSelectCountry?.Invoke(country);
        }

    }

    public void SetActiveBattleButton(bool flag)
    {
        _battlebutton.SetEnabled(flag);
    }

    private void OnDisable() => _disposables.Clear();
    private void OnDestroy() => _disposables.Dispose();
    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
}
