using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponSettingsUI : MonoBehaviour
{
    [Inject] private AttackerData _attackerData;
    [Inject] private TargetRegistry _targetRegistry;

    private UIDocument _document;
    private VisualElement _root;
    private ListView _weaponList;

    private Label _weaponNameLabel;
    private UnsignedIntegerField _damageField;
    private DropdownField _targetDropdown;

    private VisualElement _strategySection;
    private DropdownField _strategyDropdown;

    private Button _saveButton;
    public Subject<Unit> OnClosed { get; } = new();

    private Label _toastLabel;
    private VisualElement _toast;

    private AttackerConfig _currentConfig;
    private Dictionary<AttackWeaponType, IWeaponPanel> _weaponPanels = new();
    private IWeaponPanel _currentPanel;
    private CompositeDisposable _disposables = new();

    private int _lastSelectedButtonIndex = 0;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        FindElements();
        RegisterWeaponPanels();
        InitializePanels();
        SetWeaponSettingsPanel();
        SetupSubscriptions();
        SetupTabFocusRestore();
        OpenFirstWeapon();

        Hide();
    }

    private void FindElements()
    {
        _weaponList = _root.Q<ListView>("WeaponSettingsList");
        _weaponNameLabel = _root.Q<Label>("WeaponText");
        _damageField = _root.Q<UnsignedIntegerField>("DamageInteger");
        _strategySection = _root.Q<VisualElement>("StrategysContainer");
        _strategyDropdown = _root.Q<DropdownField>("StrategyDropdown");
        _saveButton = _root.Q<Button>("SaveButtonWeapon");
        _targetDropdown = _root.Q<DropdownField>("TargetDropdown");
        _toast = _root.Q<VisualElement>("SaveMessage");
        _toastLabel = _root.Q<Label>("SaveText");
    }

    private void RegisterWeaponPanels()
    {
        RegisterPanel(new AutoWeaponPanel());
        RegisterPanel(new BurstWeaponPanel());
        RegisterPanel(new SingleWeaponPanel());
        RegisterPanel(new ShotgunWeaponPanel());
        RegisterPanel(new SniperWeaponPanel());
    }

    private void RegisterPanel(IWeaponPanel panel) => _weaponPanels[panel.Type] = panel;

    private void InitializePanels()
    {
        foreach (var panel in _weaponPanels.Values)
            panel.Initialize(_root);
    }

    private void SetupSubscriptions()
    {
        Observable.FromEvent(h => _saveButton.clicked += h, h => _saveButton.clicked -= h)
            .Subscribe(_ => SaveCurrent())
            .AddTo(_disposables);


        _strategyDropdown.RegisterValueChangedCallback(evt =>
        {
            if (Enum.TryParse<AttackWeaponType>(evt.newValue, out var type))
                ShowWeaponPanel(type);
        });
    }

    private void SetupTabFocusRestore()
    {
        var tabView = _root.Q<TabView>();
        if (tabView == null) return;

        tabView.activeTabChanged += (oldTab, newTab) =>
        {
            if (newTab.name == "WeaponTab")
                FocusWeaponButton();
        };
    }

    private void SetWeaponSettingsPanel()
    {
        _weaponList.itemsSource = _attackerData.AttackerList;

        _weaponList.bindItem = (element, index) =>
        {
            (element.userData as CompositeDisposable)?.Dispose();

            var attacker = _attackerData.AttackerList[index];
            var button = element.Q<Button>("WeaponButtonTemplate");
            button.style.backgroundImage = new StyleBackground(attacker.ImageUI);

            var disposables = new CompositeDisposable();
            Observable.FromEvent(h => button.clicked += h, h => button.clicked -= h)
                .Subscribe(_ => { _lastSelectedButtonIndex = index; OpenSettings(attacker); })
                .AddTo(disposables);

            element.userData = disposables;
        };

        FocusWeaponButton();
    }

    private void OpenFirstWeapon()
    {
        var firstWeapon = _attackerData.AttackerList.FirstOrDefault();
        if (firstWeapon != null)
            OpenSettings(firstWeapon);
    }

    public void OpenSettings(AttackerConfig config)
    {
        _currentConfig = config;

        _weaponNameLabel.text = config.Name;
        _damageField.value = (uint)config.Damage;

        LoadTargetDropdown(config);

        HideAllSpecificPanels();
        LoadSpecificSettings(config);

        switch (config)
        {
            case WeaponConfig weapon:
                WeaponSaveSystem.LoadWeapon(weapon);
                ShowWeaponSettings(weapon);
                LoadActiveModule(weapon);
                break;
        }
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

    private void LoadTargetDropdown(AttackerConfig config)
    {
        if (config.TargetStrategyConfig == null) return;

        _targetDropdown.choices = _targetRegistry.AllTargets
            .Where(t => config.IsTargetStrategyAllowed(t))
            .Select(t => t.Name)
            .ToList();

        _targetDropdown.value = config.TargetStrategyConfig.Name;
    }

    private void LoadSpecificSettings(AttackerConfig config)
    {
        switch (config)
        {
            case WeaponConfig weapon:
                ShowWeaponSettings(weapon);
                LoadActiveModule(weapon);
                break;
        }
    }

    private void ShowWeaponSettings(WeaponConfig weapon)
    {
        _strategySection.style.display = DisplayStyle.Flex;

        var allowedTypes = weapon.GetAllowedWeaponTypes();
        _strategyDropdown.choices = allowedTypes.Select(t => t.ToString()).ToList();
        _strategyDropdown.value = weapon.GetActiveWeaponType().ToString();

        ShowWeaponPanel(weapon.GetActiveWeaponType());
    }

    private void LoadActiveModule(WeaponConfig weapon)
    {
        if (_currentPanel == null) return;

        var module = weapon.GetActiveModule<IShootConfig>();
        if (module != null)
            _currentPanel.LoadFrom(module);
    }


    private void ShowWeaponPanel(AttackWeaponType type)
    {
        _currentPanel?.Show(false);

        if (_weaponPanels.TryGetValue(type, out var panel))
        {
            panel.Show(true);
            _currentPanel = panel;
        }
    }

    private void HideAllSpecificPanels()
    {
        _strategySection.style.display = DisplayStyle.None;
        _currentPanel?.Show(false);
    }


    private void SaveCurrent()
    {
        if (_currentConfig == null) return;

        _saveButton.SetEnabled(false);

        _currentConfig.Damage = (int)_damageField.value;

        SaveTargetStrategy();
        SaveSpecificSettings();

        switch (_currentConfig)
        {
            case WeaponConfig weapon:
                WeaponSaveSystem.SaveWeapon(weapon);
                break;
        }
        ShowToast($"<color=#c91508>{_currentConfig.name}</color>\nis SAVE✅");

        Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ => _saveButton.SetEnabled(true)).AddTo(_disposables);
    }

    private void SaveTargetStrategy()
    {
        if (_currentConfig.TargetStrategyConfig == null) return;

        string selectedName = _targetDropdown.value;
        _currentConfig.TargetStrategyConfig = _targetRegistry.AllTargets
            .FirstOrDefault(t => t.Name == selectedName);
    }

    private void SaveSpecificSettings()
    {
        switch (_currentConfig)
        {
            case WeaponConfig weapon:
                var selectedType = Enum.Parse<AttackWeaponType>(_strategyDropdown.value);
                weapon.SetActiveWeaponType(selectedType);

                var module = weapon.GetActiveModule<IShootConfig>();
                if (module != null && _currentPanel != null)
                    _currentPanel.SaveTo(module);
                break;
        }
    }

    private void FocusWeaponButton()
    {
        EventCallback<GeometryChangedEvent> handler = null;
        handler = evt =>
        {
            _weaponList.UnregisterCallback(handler);
            _weaponList.GetRootElementForIndex(_lastSelectedButtonIndex)?.Q<Button>("WeaponButtonTemplate")?.Focus();
        };
        _weaponList.RegisterCallback(handler);
    }

    public void Show() => _root.style.display = DisplayStyle.Flex;
    public void Hide() => _root.style.display = DisplayStyle.None;

    private void OnDestroy()
    {
        _disposables?.Dispose();
        OnClosed?.Dispose();
    }
}
