using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    [SerializeField] private CountrySettingsUI _ui;
    [SerializeField] private ControlSettings _controlSettings;
    private Button _startButton;
    private Button _countrySettingsButton;
    private Button _controlSettingsButton;
    private Button _exitButton;
    private VisualElement _root;
    private CompositeDisposable _disposables = new();
    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        FindButtons();
        SetupSubscriptions();
    }

    private void FindButtons()
    {
        _startButton = _root.Q<Button>("StartButton");
        _countrySettingsButton = _root.Q<Button>("SettingsButton");
        _controlSettingsButton = _root.Q<Button>("ControlSettingsButton");
        _exitButton = _root.Q<Button>("ExitButton");
    }

    private void SetupSubscriptions()
    {
        _startButton.clicked += () => SceneManager.LoadSceneAsync("SampleScene");

        _countrySettingsButton.clicked += () =>
        {
            _root.style.display = DisplayStyle.None;
            _ui.Show();
        };

        _controlSettingsButton.clicked += () =>
        {
            _root.style.display = DisplayStyle.None;
            _controlSettings.Show();
        };

        _exitButton.clicked += () => Application.Quit();

        _ui.OnClosed
            .Subscribe(_ =>
            {
                _ui.Hide();
                _root.style.display = DisplayStyle.Flex;
            })
            .AddTo(_disposables);

        _controlSettings.OnClosed
            .Subscribe(_ =>
            {
                _controlSettings.Hide();
                _root.style.display = DisplayStyle.Flex;
            })
            .AddTo(_disposables);

    }

    private void OnDestroy() => _disposables?.Dispose();

}
