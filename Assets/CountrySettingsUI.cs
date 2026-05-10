using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

public class CountrySettingsUI : MonoBehaviour
{
    [Inject] private CountryDatabase _countries;
    [Inject] private AttackerData _attackerData;
    [Inject] private SideData _sideData;

    private UIDocument _document;
    private VisualElement _root;
    private ListView _countrySettingsList;
    private CountryConfig _selectedCountry;

    private DropdownField _dropDownWeapon;
    private DropdownField _dropDownSlot;
    private DropdownField _dropDownSide;
    private VisualElement _currentCountrySettingsContainer;
    private Label _countryName;
    private UnsignedIntegerField _healthField;
    private Button _saveButton;
    private Button _closetButton;

    private Label _toastLabel;
    private VisualElement _toast;

    public Subject<Unit> OnClosed { get; } = new();
    private CompositeDisposable _disposables = new();
    private int _lastSelectedButtonIndex = 0;


    private void Awake()
    {
        FindElements();
        SetupSubscriptions();
        SetupDropdowns();
        SetCountrySettingsPanel();
        SetupTabFocusRestore();
        OpenFirstCountry();
        Hide();
    }

    private void FindElements()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        _countrySettingsList = _root.Q<ListView>("CountrySettingsList");
        _saveButton = _root.Q<Button>("SaveButton");
        _closetButton = _root.Q<Button>("ClosetButton");
        _currentCountrySettingsContainer = _root.Q<VisualElement>("CountrySettings");
        _dropDownWeapon = _root.Q<DropdownField>("DropdownWeapon");
        _dropDownSide = _root.Q<DropdownField>("DropdownSide");
        _dropDownSlot = _root.Q<DropdownField>("DropdownSlot");
        _healthField = _root.Q<UnsignedIntegerField>("HalthInteger");
        _countryName = _root.Q<Label>("CountryName");
        _toast = _root.Q<VisualElement>("SaveMessage");
        _toastLabel = _root.Q<Label>("SaveText");
    }

    private void SetupSubscriptions()
    {
        Observable.FromEvent(h => _saveButton.clicked += h, h => _saveButton.clicked -= h)
            .Subscribe(_ => SaveCurrentCountry())
            .AddTo(_disposables);

        Observable.FromEvent(h => _closetButton.clicked += h, h => _closetButton.clicked -= h)
            .Subscribe(_ => { Hide(); OnClosed.OnNext(Unit.Default); })
            .AddTo(_disposables);
    }

    private void SetupDropdowns()
    {
        _dropDownWeapon.choices = _attackerData.AttackerList.Select(a => a.Name).ToList();
        _dropDownSide.choices = _sideData.SideInfo.Select(a => a.SideName).ToList();
    }

    private void SetupTabFocusRestore()
    {
        var tabView = _root.Q<TabView>();
        if (tabView == null) return;

        tabView.activeTabChanged += (oldTab, newTab) =>
        {
            if (newTab.name == "CountryTab")
                FocusCountryButton();
        };
    }

    private void OpenFirstCountry()
    {
        var firstCountry = _countries.AllCountries.FirstOrDefault();
        if (firstCountry != null)
            OpenSettingsCountry(firstCountry);
    }


    private void SetCountrySettingsPanel()
    {
        _countrySettingsList.itemsSource = _countries.AllCountries;

        _countrySettingsList.bindItem = (element, index) =>
        {
            (element.userData as CompositeDisposable)?.Dispose();

            var country = _countries.AllCountries[index];
            var button = element.Q<Button>("CountryButtonTemplate");
            button.style.backgroundImage = new StyleBackground(country.CountryFlag);

            var disposables = new CompositeDisposable();
            Observable.FromEvent(h => button.clicked += h, h => button.clicked -= h)
                .Subscribe(_ =>
                {
                    _lastSelectedButtonIndex = index;
                    OpenSettingsCountry(country);
                })
                .AddTo(disposables);

            element.userData = disposables;
        };

        FocusCountryButton();
    }

    public void OpenSettingsCountry(CountryConfig country)
    {
        _selectedCountry = country;
        CountrySaveSystem.LoadCountry(_selectedCountry, _attackerData, _sideData);

        _currentCountrySettingsContainer.style.visibility = Visibility.Visible;
        _countryName.text = country.CountryName;

        _dropDownWeapon.index = FindDropIndexByName(_dropDownWeapon,
            country.SelectedAttacker?.Name ?? country.DefaultAttacker.Name);
        _dropDownSide.index = FindDropIndexByName(_dropDownSide,
            country.Side?.SideName ?? country.DefaultSide.SideName);
        _healthField.value = (uint)(country.CountryGameplayConfig.MaxHealth > 0
            ? country.CountryGameplayConfig.MaxHealth
            : country.CountryGameplayConfig.DefaultMaxHealth);
        _dropDownSlot.index = country.SelectedSlotIndex >= 0
            ? country.SelectedSlotIndex
            : country.DefaultAttackerSlotIndex;
    }

    private void ShowToast(string message)
    {
        _toastLabel.text = message;
        _toast.RemoveFromClassList("show");
        _toast.schedule.Execute(() => _toast.AddToClassList("show")).ExecuteLater(10);

        Observable.Timer(TimeSpan.FromSeconds(2))
            .Subscribe(_ => _toast.RemoveFromClassList("show"))
            .AddTo(_disposables);
    }

    private void SaveCurrentCountry()
    {
        if (_selectedCountry == null) return;

        _saveButton.SetEnabled(false);

        _selectedCountry.SelectedSlotIndex = _dropDownSlot.index;
        _selectedCountry.SelectedAttacker = _attackerData.AttackerList
            .FirstOrDefault(a => a.Name == _dropDownWeapon.value);
        _selectedCountry.Side = _sideData.SideInfo
            .FirstOrDefault(a => a.SideName == _dropDownSide.value);
        _selectedCountry.CountryGameplayConfig.MaxHealth = (int)_healthField.value;

        CountrySaveSystem.SaveCountry(_selectedCountry);
        ShowToast($"<color=#ffaf24>{_selectedCountry.CountryName}</color>\nis SAVE✅");
        
        Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ => _saveButton.SetEnabled(true)).AddTo(_disposables);
    }


    private int FindDropIndexByName(DropdownField dropDown, string name)
        => dropDown.index = dropDown.choices.FindIndex(a => a == name);

    private void FocusCountryButton()
    {
        EventCallback<GeometryChangedEvent> handler = null;
        handler = evt =>
        {
            _countrySettingsList.UnregisterCallback(handler);
            _countrySettingsList.GetRootElementForIndex(_lastSelectedButtonIndex)?.Q<Button>("CountryButtonTemplate")?.Focus();
        };
        _countrySettingsList.RegisterCallback(handler);
    }

    public void Show() => _root.style.display = DisplayStyle.Flex;
    public void Hide() => _root.style.display = DisplayStyle.None;

    private void OnDestroy()
    {
        _disposables?.Dispose();
        OnClosed?.Dispose();
    }
}
